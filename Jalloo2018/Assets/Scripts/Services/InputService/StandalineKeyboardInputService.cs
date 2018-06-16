namespace DogHouse.Jalloo.Services
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
    using DogHouse.Core.Services;
    using System;

    public class StandalineKeyboardInputService : MonoBehaviour, IInputService
    {
        #region Public Variables
        public event Action UpPressed;
        public event Action DownPressed;
        public event Action RightPressed;
        public event Action LeftPressed;
        public event Action InteractPressed;
        #endregion

        #region Main Methods
        void Start() => RegisterService();

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.W)) UpPressed?.Invoke();
            if (Input.GetKeyDown(KeyCode.A)) LeftPressed?.Invoke();
            if (Input.GetKeyDown(KeyCode.S)) DownPressed?.Invoke();
            if (Input.GetKeyDown(KeyCode.D)) RightPressed?.Invoke();
            if (Input.GetKeyDown(KeyCode.Space)) InteractPressed?.Invoke();
        }
		
        public void RegisterService()
		{
            ServiceLocator.Register<IInputService>(this);
		}
        #endregion
    }
}
