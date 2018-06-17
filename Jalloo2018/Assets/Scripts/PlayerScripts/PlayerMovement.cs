using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DogHouse.Core.Services;
using DogHouse.Jalloo.Services;
using DogHouse.Jalloo.Levels;

public enum direction { RIGHT,LEFT,UP,DOWN}

public class PlayerMovement : Entity {

    ServiceReference<IInputService> inputService = new ServiceReference<IInputService>();


    PlayfieldData levelData;
    bool hasBall = false;
    direction dir = direction.UP;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        inputService.AddRegistrationHandle(HandleInputRegister);
    }

    void HandleInputRegister()
    {
        inputService.Reference.DownPressed += DownPressed;
        inputService.Reference.UpPressed += UpPressed;
        inputService.Reference.RightPressed += RightPressed;
        inputService.Reference.LeftPressed += LeftPressed;
        inputService.Reference.InteractPressed += Interact;
        inputService.Reference.RotationChanged += RotatePlayer;

    }

    private void OnDisable()
    {
        if(inputService.isRegistered())
        {
            inputService.Reference.DownPressed -= DownPressed;
            inputService.Reference.UpPressed -= UpPressed;
            inputService.Reference.RightPressed -= RightPressed;
            inputService.Reference.LeftPressed -= LeftPressed;
            inputService.Reference.InteractPressed -= Interact;
            inputService.Reference.RotationChanged -= RotatePlayer;
        }
    }

    // Update is called once per frame
    void Update () {

        
    }

    void UpPressed()
    {
        if(BoundCheck(currentPosition.X, currentPosition.Y + 1))
        {
            int nextPosY = currentPosition.Y + 1;
            UpdatePosition(currentPosition.X, nextPosY);
            if(!hasBall)
            {
                Rotate(direction.UP);
            }          
        }
    }

    void DownPressed()
    {
        if (BoundCheck(currentPosition.X, currentPosition.Y - 1))
        {
            int nextPosY = currentPosition.Y - 1;
            UpdatePosition(currentPosition.X, nextPosY);
            if(!hasBall)
            {
                Rotate(direction.DOWN);
            }  
        }
    }

    void RightPressed()
    {
        if (BoundCheck(currentPosition.X + 1, currentPosition.Y))
        {
            int nextPosX = currentPosition.X + 1;
            UpdatePosition(nextPosX, currentPosition.Y);
            if (!hasBall)
            {
                Rotate(direction.RIGHT);
            }
        }
    }

    void LeftPressed()
    {
        if (BoundCheck(currentPosition.X - 1,currentPosition.Y))
        {
            int nextPosX = currentPosition.X - 1;
            UpdatePosition(nextPosX, currentPosition.Y);
            if(!hasBall)
            {
                Rotate(direction.LEFT);
            }
        }
    }

    void RotatePlayer(Vector2 rotation)
    {
        if(hasBall)
        {

        }
    }

    void Interact()
    {
        if(!hasBall)
        {
            switch (dir)
            {
                case direction.UP:
                    AttemptGrab(currentPosition.X, currentPosition.Y + 1);
                    break;
                case direction.DOWN:
                    AttemptGrab(currentPosition.X, currentPosition.Y - 1);
                    break;
                case direction.RIGHT:
                    AttemptGrab(currentPosition.X + 1, currentPosition.Y);
                    break;
                case direction.LEFT:
                    AttemptGrab(currentPosition.X - 1, currentPosition.Y);
                    break;
            }
        }
    }

    void AttemptGrab(int x, int y)
    {
        if (levelManager.Reference.GetLevelData().fieldData[x, y] == EntityType.BALL)
        {
            
            GameObject ball = GetBall(x, y);
            if(ball != null)
            {
                ball.transform.parent = this.transform;
                hasBall = true;
            }
            else
            {
                Debug.LogError("Something fucked Up hardcore");
            }
        }
    }

    GameObject GetBall(int x, int y)
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");

        for (int i = 0; i < balls.Length; i++)
        {
            Entity entity = balls[i].GetComponent<Entity>();
            if(entity.Position.X == x && entity.Position.Y == y)
            {
                return balls[i];
            }
        }
        return null;
    }

    bool BoundCheck(int x, int y)
    {
        if(!levelManager.isRegistered())
        {
            return false;
        }
        levelData = levelManager.Reference.GetLevelData();
        if(levelData.fieldData[x,y] == EntityType.EMPTY || levelData.fieldData[x, y] == EntityType.PANEL)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Rotate(direction direct)
    {
        switch(direct)
        {
            case direction.RIGHT:
                transform.rotation = Quaternion.Euler(new Vector3(0, 90f, 0));
                break;
            case direction.LEFT:
                transform.rotation = Quaternion.Euler(new Vector3(0, 270f, 0));
                break;
            case direction.DOWN:
                transform.rotation = Quaternion.Euler(new Vector3(0, 180f, 0));
                break;
            case direction.UP:
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                break;
        }
        dir = direct;  
    }
}
