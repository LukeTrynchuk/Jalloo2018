using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DogHouse.Core.Services;
using DogHouse.Jalloo.Services;
using DogHouse.Jalloo.Levels;

public class PlayerMovement : Entity {

    ServiceReference<IInputService> inputService = new ServiceReference<IInputService>();


    PlayfieldData levelData;

    
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

    // Update is called once per frame
    void Update () {

        
    }

    void UpPressed()
    {
        if(BoundCheck(currentPosition.X, currentPosition.Y + 1))
        {
            int nextPosY = currentPosition.Y + 1;
            UpdatePosition(currentPosition.X, nextPosY);
        }
    }

    void DownPressed()
    {
        if (BoundCheck(currentPosition.X, currentPosition.Y - 1))
        {
            int nextPosY = currentPosition.Y - 1;
            UpdatePosition(currentPosition.X, nextPosY);
        }
    }

    void RightPressed()
    {
        if (BoundCheck(currentPosition.X + 1, currentPosition.Y))
        {
            int nextPosX = currentPosition.X + 1;
            UpdatePosition(nextPosX, currentPosition.Y);
        }
    }

    void LeftPressed()
    {
        if (BoundCheck(currentPosition.X - 1,currentPosition.Y))
        {
            int nextPosX = currentPosition.X - 1;
            UpdatePosition(nextPosX, currentPosition.Y);
        }
    }

    void Interact()
    {
       
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
}
