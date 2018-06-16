namespace DogHouse.Core.Services
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using System;

	public class ServiceReference<T> where T: class
	{
		#region Public Variables
		public T Reference
		{
			get
			{
				return ServiceLocator.GetService<T> ();
			}
		}
		#endregion

		#region Private Variables
		private T instance;
		#endregion

		#region Main Methods

		public void AddRegistrationHandle(Action Handle)
		{
			ServiceLocator.AddOnRegisterHandle<T> (Handle);
		}

		public bool isRegistered()
		{
			if (Reference == null)
				return false;
			return true;
		}
		#endregion
	}
}
