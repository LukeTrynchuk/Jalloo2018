using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DogHouse.Core.Services;
using DogHouse.Jalloo.Services;
using DogHouse.Jalloo.Levels;

public class PlayerMovement : MonoBehaviour {

    ServiceReference<IInputService> inputService = new ServiceReference<IInputService>();

    Vector3[,] gridPositions;

    PlayfieldPosition currentPosition;
    int arraySize = 10;

	// Use this for initialization
	void Start () {
        gridPositions = new Vector3[arraySize,arraySize];
        for (int x = 0; x < arraySize; x++)
        {
            for (int y = 0; y < arraySize; y++)
            {
                gridPositions[x, y] = new Vector3(-arraySize/2 + x, 0, arraySize/2 - y);
            }
        }
    }

    private void OnEnable()
    {
        inputService.AddRegistrationHandle(HandleInputRegister);
        currentPosition.X = (int)transform.position.x;
        currentPosition.Y = (int)transform.position.z;
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
        if(currentPosition.Y+1>=0)
        {
            currentPosition.Y += 1;
            transform.position = new Vector3(currentPosition.X, 0,currentPosition.Y);
        }
    }

    void DownPressed()
    {
        if (currentPosition.Y - 1 >= 0)
        {
            currentPosition.Y -= 1;
            transform.position = new Vector3(currentPosition.X, 0, currentPosition.Y);
        }
    }

    void RightPressed()
    {
        if (currentPosition.X + 1 <= arraySize-1)
        {
            currentPosition.X += 1;
            transform.position = new Vector3(currentPosition.X, 0, currentPosition.Y);
        }
    }

    void LeftPressed()
    {
        if (currentPosition.X - 1 >= 0)
        {
            currentPosition.X -= 1;
            transform.position = new Vector3(currentPosition.X, 0, currentPosition.Y);
            Debug.Log(currentPosition.X);
        }
    }

    void Interact()
    {
       
    }
}
