using UnityEngine;
using DogHouse.Core.Services;
using System;

namespace DogHouse.Jalloo.Services
{
    public class GamepadInputService : MonoBehaviour, IInputService
    {
        #region Public Variables
        public event Action UpPressed;
        public event Action DownPressed;
        public event Action RightPressed;
        public event Action LeftPressed;
        public event Action InteractPressed;
        public event Action<Vector2> RotationChanged;
        #endregion

        #region Private Variables
        private Vector2 m_leftJoystick;
        private Vector2 m_rightJoystick;
        #endregion

        #region Main Methods
        void Start() => RegisterService();

        public void RegisterService()
        {
            ServiceLocator.Register<IInputService>(this);
        }

        void Update()
        {
#if UNITY_STANDALONE_WIN
			m_rightJoystick.x = Input.GetAxis("Win_Rightjoystick_Hor");
			m_rightJoystick.y = Input.GetAxis("Win_Rightjoystick_Ver");
			
			m_leftJoystick.x = Input.GetAxis("Win_Leftjoystick_Hor");
			m_leftJoystick.y = Input.GetAxis("Win_Leftjoystick_Ver");
#else
            m_rightJoystick.x = Input.GetAxis("OSX_Rightjoystick_Hor");
            m_rightJoystick.y = Input.GetAxis("OSX_Rightjoystick_Ver");

            m_leftJoystick.x = Input.GetAxis("OSX_Leftjoystick_Hor");
            m_leftJoystick.y = Input.GetAxis("OSX_Leftjoystick_Ver");
#endif

            Debug.Log(m_leftJoystick);

            CheckJoystickValues();
            CheckInteractButton();

            m_leftJoystick = Vector2.zero;
            m_rightJoystick = Vector2.zero;
        }
        #endregion

        #region Utility Methods
        private void CheckJoystickValues()
        {
            if (m_leftJoystick.magnitude > 0f)
            {
                CheckLeftJoystickValues();
            }

            if (m_rightJoystick.magnitude > 0f)
            {
                CheckRightJoystickValues();
            }
        }

        private void CheckRightJoystickValues() => RotationChanged?.Invoke(m_rightJoystick);

        private void CheckLeftJoystickValues()
        {
            if (m_leftJoystick.x < 0.45f) LeftPressed?.Invoke();
            if (m_leftJoystick.x > 0.45f) RightPressed?.Invoke();
            if (m_leftJoystick.y < 0.45f) DownPressed?.Invoke();
            if (m_leftJoystick.y > 0.45f) UpPressed?.Invoke();
        }

        private void CheckInteractButton()
        {

#if UNITY_STANDALONE_WIN
            if(Input.GetButton("Win_Interact"))
            {
                InteractPressed?.Invoke();
            }
#else
            if (Input.GetButton("OSX_Interact"))
            {
                InteractPressed?.Invoke();
            }
            #endif
        }
#endregion
    }
}
