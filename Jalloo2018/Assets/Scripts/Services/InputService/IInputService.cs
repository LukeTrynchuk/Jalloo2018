namespace DogHouse.Jalloo.Services
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
    using DogHouse.Core.Services;

	public interface IInputService : IService 
	{
		Vector2 GetMovementVector();
		bool PressedSelect();
		bool PressedCancel();
		bool PressedMenu();
	}
}
