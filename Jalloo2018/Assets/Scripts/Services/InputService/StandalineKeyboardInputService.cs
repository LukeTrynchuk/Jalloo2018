using UnityEngine;
using DogHouse.Core.Services;
using System;

namespace DogHouse.Jalloo.Services
{
    public class StandalineKeyboardInputService : MonoBehaviour, IInputService
    {
        #region Public Variables
        public event Action UpPressed;
        public event Action DownPressed;
        public event Action RightPressed;
        public event Action LeftPressed;
        public event Action InteractPressed;
        public event Action<Vector2> RotationChanged;
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

            DetermineRotationInput();
        }

        private void DetermineRotationInput()
        {
            float horizontal = Input.GetKeyDown(KeyCode.RightArrow) ? 1 : 0;
            horizontal -= Input.GetKeyDown(KeyCode.LeftArrow) ? 1 : 0;

            float vertical = Input.GetKeyDown(KeyCode.UpArrow) ? 1 : 0;
            vertical -= Input.GetKeyDown(KeyCode.DownArrow) ? 1 : 0;

            RotationChanged?.Invoke(new Vector2(horizontal, vertical));
        }

        public void RegisterService()
		{
            ServiceLocator.Register<IInputService>(this);
		}
        #endregion
    }
}
