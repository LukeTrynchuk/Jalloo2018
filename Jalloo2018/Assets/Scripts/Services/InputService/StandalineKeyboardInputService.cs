namespace DogHouse.Jalloo.Services
{

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
    using DogHouse.Core.Services;

	public class StandalineKeyboardInputService : MonoBehaviour , IInputService
	{
		#region Private Variables
		private int m_numberOfJoysticks;
		#endregion

		#region Main Methods
		void Awake()
		{
			SetNumberOfJoySticks ();
			if (m_numberOfJoysticks == 0)
				RegisterService ();
		}

		void Update()
		{
			#if UNITY_STANDALONE
			if (Input.GetJoystickNames ().Length == 0 && m_numberOfJoysticks > 0) 
			{
				RegisterService ();
			}

			SetNumberOfJoySticks ();
			#endif
		}

		public Vector2 GetMovementVector()
		{
			bool left = Input.GetKeyDown (KeyCode.LeftArrow);
			bool right = Input.GetKeyDown (KeyCode.RightArrow);
			bool up = Input.GetKeyDown (KeyCode.UpArrow);
			bool down = Input.GetKeyDown (KeyCode.DownArrow);
			Vector2 deltaMovement = new Vector2 ();

			if (left ^ right) 
			{
				if (left)
					deltaMovement.x = -1;
				if (right)
					deltaMovement.x = 1;
			}

			if (up ^ down) 
			{
				if (up)
					deltaMovement.y = 1;
				if (down)
					deltaMovement.y = -1;
			}

			return deltaMovement;
		}

		public bool PressedSelect()
		{
			if (Input.GetKeyDown (KeyCode.A))
				return true;
			return false;
		}

		public bool PressedCancel()
		{
			if (Input.GetKeyDown (KeyCode.Z))
				return true;
			return false;
		}

		public bool PressedMenu()
		{
			if (Input.GetKeyDown (KeyCode.X))
				return true;
			return false;
		}

		public void RegisterService()
		{
			#if UNITY_STANDALONE
				ServiceLocator.Register<IInputService>(this);
			#elif
				gameObject.SetActive(false);
			#endif
		}
		#endregion

		#region Low Level Functions

		private void SetNumberOfJoySticks()
		{
			m_numberOfJoysticks = Input.GetJoystickNames ().Length;
		}

		#endregion
	}
}
