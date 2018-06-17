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
    Ball heldBall;
    
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
        }
    }

    void UpPressed()
    {
        Move(direction.UP);
    }

    void DownPressed()
    {
        Move(direction.DOWN);
    }

    void RightPressed()
    {
        Move(direction.RIGHT);
    }

    void LeftPressed()
    {
        Move(direction.LEFT);
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
            heldBall = ball.GetComponent<Ball>();

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

    bool IsValidMoveWithBall(direction direct)
    {
        if (dir == direction.LEFT || dir == direction.RIGHT)
        {
            if (direct == direction.UP || direct == direction.DOWN)
            {
                return false;
            }
        }
        if (dir == direction.UP || dir == direction.DOWN)
        {
            if (direct == direction.RIGHT || direct == direction.LEFT)
            {
                return false;
            }
        }
        return true;
    }

    void AttemptTurn(direction direct)
    {
        Debug.Log("TURNING");

        PlayfieldPosition ballFacingBounds = currentPosition;
        PlayfieldPosition playerFacingBounds = currentPosition;
        direction inverse = dir;

        switch(direct)
        {
            case direction.UP:
                ballFacingBounds.Y += 1;
                ballFacingBounds.X += dir == direction.RIGHT ? 1 : -1;

                playerFacingBounds.Y += 1;
                inverse = direction.DOWN;
                break;
            case direction.DOWN:
                ballFacingBounds.Y -= 1;
                ballFacingBounds.X += dir == direction.RIGHT ? 1 : -1;

                playerFacingBounds.Y -= 1;
                inverse = direction.UP;
                break;
            case direction.RIGHT:
                ballFacingBounds.X += 1;
                ballFacingBounds.Y += dir == direction.UP ? 1 : -1;

                playerFacingBounds.X += 1;
                inverse = direction.LEFT;
                break;
            case direction.LEFT:
                ballFacingBounds.X -= 1;
                ballFacingBounds.Y += dir == direction.UP ? 1 : -1;

                playerFacingBounds.X -= 1;
                inverse = direction.RIGHT;
                break;
        }
        if(BoundCheck(ballFacingBounds.X,ballFacingBounds.Y))
        {
            PlayfieldPosition ballOldPosition = heldBall.Position;
            
            UpdatePosition(ballOldPosition.X, ballOldPosition.Y);
            Rotate(direct);
            heldBall.UpdatePosition(ballFacingBounds.X, ballFacingBounds.Y);
            return;
        }

        if(BoundCheck(playerFacingBounds.X, playerFacingBounds.Y))
        {
            PlayfieldPosition oldPlayerPosition = currentPosition;

            UpdatePosition(playerFacingBounds.X, playerFacingBounds.Y);
            Rotate(inverse);
            heldBall.UpdatePosition(oldPlayerPosition.X, oldPlayerPosition.Y);
            return;
        }
    }

    void MoveWithBall(direction direct)
    {
        if(!IsValidMoveWithBall(direct))
        {
            AttemptTurn(direct);
            return;
        }

        PlayfieldPosition position = currentPosition;
        PlayfieldPosition ballPosition = heldBall.Position;
        PlayfieldPosition boundPosition = currentPosition;

        switch (direct)
        {
            case direction.UP:
                position.Y += 1;
                ballPosition.Y += 1;
                boundPosition.Y += dir == direction.UP?2:1;
                break;
            case direction.DOWN:
                position.Y -= 1;
                ballPosition.Y -= 1;
                boundPosition.Y -= dir == direction.DOWN ? 2 : 1;
                break;
            case direction.RIGHT:
                position.X += 1;
                ballPosition.X += 1;
                boundPosition.X += dir == direction.RIGHT ? 2 : 1;
                break;
            case direction.LEFT:
                position.X -= 1;
                ballPosition.X -= 1;
                boundPosition.X -= dir == direction.LEFT ? 2 : 1;
                break;
        }
        if (BoundCheck(boundPosition.X, boundPosition.Y))
        {
            UpdatePosition(position.X, position.Y);
            heldBall.UpdatePosition(ballPosition.X,ballPosition.Y);

        }
    }
    void MoveWithoutBall(direction direct)
    {
        PlayfieldPosition position = currentPosition;
        switch(direct)
        {
            case direction.UP:
                position.Y += 1;
                break;
            case direction.DOWN:
                position.Y -= 1;
                break;
            case direction.RIGHT:
                position.X += 1;
                break;
            case direction.LEFT:
                position.X -= 1;
                break;
        }
        if (BoundCheck(position.X, position.Y))
        {
            UpdatePosition(position.X, position.Y);
            Rotate(direct);
        }
    }
    void Move(direction direct)
    {
        if (!hasBall)
        {
            MoveWithoutBall(direct);
        }
        else
        {
            MoveWithBall(direct);
        }
    }
}
