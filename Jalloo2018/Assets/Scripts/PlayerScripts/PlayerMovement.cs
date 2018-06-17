using UnityEngine;
using DogHouse.Core.Services;
using DogHouse.Jalloo.Services;
using DogHouse.Jalloo.Levels;

public enum Direction { RIGHT,LEFT,UP,DOWN}

public class PlayerMovement : Entity 
{
    ServiceReference<IInputService> inputService = new ServiceReference<IInputService>();


    PlayfieldData levelData;
    bool hasBall = false;
    Direction dir = Direction.UP;
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
        Move(Direction.UP);
    }

    void DownPressed()
    {
        Move(Direction.DOWN);
    }

    void RightPressed()
    {
        Move(Direction.RIGHT);
    }

    void LeftPressed()
    {
        Move(Direction.LEFT);
    }

    void Interact()
    {
        if(!hasBall)
        {
            switch (dir)
            {
                case Direction.UP:
                    AttemptGrab(currentPosition.X, currentPosition.Y + 1);
                    break;
                case Direction.DOWN:
                    AttemptGrab(currentPosition.X, currentPosition.Y - 1);
                    break;
                case Direction.RIGHT:
                    AttemptGrab(currentPosition.X + 1, currentPosition.Y);
                    break;
                case Direction.LEFT:
                    AttemptGrab(currentPosition.X - 1, currentPosition.Y);
                    break;
            }
        }
        else
        {
            heldBall.transform.parent = null;
            hasBall = false;
            heldBall.Launch(dir);
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

    void Rotate(Direction direct)
    {
        switch(direct)
        {
            case Direction.RIGHT:
                transform.rotation = Quaternion.Euler(new Vector3(0, 90f, 0));
                break;
            case Direction.LEFT:
                transform.rotation = Quaternion.Euler(new Vector3(0, 270f, 0));
                break;
            case Direction.DOWN:
                transform.rotation = Quaternion.Euler(new Vector3(0, 180f, 0));
                break;
            case Direction.UP:
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                break;
        }
        dir = direct;  
    }

    bool IsValidMoveWithBall(Direction direct)
    {
        if (dir == Direction.LEFT || dir == Direction.RIGHT)
        {
            if (direct == Direction.UP || direct == Direction.DOWN)
            {
                return false;
            }
        }
        if (dir == Direction.UP || dir == Direction.DOWN)
        {
            if (direct == Direction.RIGHT || direct == Direction.LEFT)
            {
                return false;
            }
        }
        return true;
    }

    void AttemptTurn(Direction direct)
    {
        PlayfieldPosition ballFacingBounds = currentPosition;
        PlayfieldPosition playerFacingBounds = currentPosition;
        Direction inverse = dir;

        switch(direct)
        {
            case Direction.UP:
                ballFacingBounds.Y += 1;
                ballFacingBounds.X += dir == Direction.RIGHT ? 1 : -1;

                playerFacingBounds.Y += 1;
                inverse = Direction.DOWN;
                break;
            case Direction.DOWN:
                ballFacingBounds.Y -= 1;
                ballFacingBounds.X += dir == Direction.RIGHT ? 1 : -1;

                playerFacingBounds.Y -= 1;
                inverse = Direction.UP;
                break;
            case Direction.RIGHT:
                ballFacingBounds.X += 1;
                ballFacingBounds.Y += dir == Direction.UP ? 1 : -1;

                playerFacingBounds.X += 1;
                inverse = Direction.LEFT;
                break;
            case Direction.LEFT:
                ballFacingBounds.X -= 1;
                ballFacingBounds.Y += dir == Direction.UP ? 1 : -1;

                playerFacingBounds.X -= 1;
                inverse = Direction.RIGHT;
                break;
        }
        if(BoundCheck(ballFacingBounds.X,ballFacingBounds.Y))
        {
            PlayfieldPosition ballOldPosition = heldBall.Position;
            PlayfieldPosition oldPlayerPosition = currentPosition;

            UpdatePosition(ballOldPosition.X, ballOldPosition.Y);
            Rotate(direct);
            heldBall.UpdatePosition(ballFacingBounds.X, ballFacingBounds.Y);
            levelManager.Reference.SetTileValue(oldPlayerPosition.X, oldPlayerPosition.Y, EntityType.PLAYER);
            return;
        }

        if(BoundCheck(playerFacingBounds.X, playerFacingBounds.Y))
        {
            PlayfieldPosition oldPlayerPosition = currentPosition;

            UpdatePosition(playerFacingBounds.X, playerFacingBounds.Y);
            Rotate(inverse);
            heldBall.UpdatePosition(oldPlayerPosition.X, oldPlayerPosition.Y);
            levelManager.Reference.SetTileValue(oldPlayerPosition.X, oldPlayerPosition.Y, EntityType.PLAYER);
            return;
        }
    }

    void MoveWithBall(Direction direct)
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
            case Direction.UP:
                position.Y += 1;
                ballPosition.Y += 1;
                boundPosition.Y += dir == Direction.UP?2:1;
                break;
            case Direction.DOWN:
                position.Y -= 1;
                ballPosition.Y -= 1;
                boundPosition.Y -= dir == Direction.DOWN ? 2 : 1;
                break;
            case Direction.RIGHT:
                position.X += 1;
                ballPosition.X += 1;
                boundPosition.X += dir == Direction.RIGHT ? 2 : 1;
                break;
            case Direction.LEFT:
                position.X -= 1;
                ballPosition.X -= 1;
                boundPosition.X -= dir == Direction.LEFT ? 2 : 1;
                break;
        }
        if (BoundCheck(boundPosition.X, boundPosition.Y))
        {
            PlayfieldPosition oldPlayerPosition = currentPosition;
            UpdatePosition(position.X, position.Y);
            heldBall.UpdatePosition(ballPosition.X,ballPosition.Y);
            levelManager.Reference.SetTileValue(oldPlayerPosition.X, oldPlayerPosition.Y, EntityType.PLAYER);
        }
    }
    void MoveWithoutBall(Direction direct)
    {
        PlayfieldPosition position = currentPosition;
        switch(direct)
        {
            case Direction.UP:
                position.Y += 1;
                break;
            case Direction.DOWN:
                position.Y -= 1;
                break;
            case Direction.RIGHT:
                position.X += 1;
                break;
            case Direction.LEFT:
                position.X -= 1;
                break;
        }
        if (BoundCheck(position.X, position.Y))
        {
            UpdatePosition(position.X, position.Y);
            Rotate(direct);
        }
    }
    void Move(Direction direct)
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
