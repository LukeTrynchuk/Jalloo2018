namespace DogHouse.Jalloo.Services
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
    using DogHouse.Core.Services;

	public interface IInputService : IService 
	{
        event System.Action UpPressed;
        event System.Action DownPressed;
        event System.Action RightPressed;
        event System.Action LeftPressed;

        event System.Action InteractPressed;
	}
}
