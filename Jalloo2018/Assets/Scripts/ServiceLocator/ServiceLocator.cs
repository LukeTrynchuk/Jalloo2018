namespace DogHouse.Core.Services
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using System;

	public sealed class ServiceLocator 
	{
		#region Private Variables
		private static ServiceLocator m_instance;
		private static Dictionary < string, object > servicesDictionary;
		private static Dictionary < string, List<Action>> m_callbackHandles;
		#endregion

		#region Main Methods
		public static void Register < T >(T service)
		{
			Initialize ();

			if (ServiceAlreadyRegistered<T> ()) 
			{
				ReplaceService<T> (service);
				DispatchRegistrationHandles<T> ();
				return;
			}
			servicesDictionary[typeof(T).Name] = service;
			DispatchRegistrationHandles<T> ();
		}

		public static void Unregister<T>(object Service)
		{
			Initialize ();

			if (!ServiceAlreadyRegistered<T> ())
				return;

			if (GetService<T> ().Equals (Service)) 
			{
				UnregisterService<T> ();
			}
		}

		public static T GetService < T >()
		{
			Initialize ();

			T instance = default(T);
			if(servicesDictionary.ContainsKey(typeof(T).Name) == true)
			{
				instance = (T) servicesDictionary[typeof(T).Name];
				return instance;
			}
			return default(T);
		}

		public static void AddOnRegisterHandle<T>(Action callback)
		{
			Initialize ();

			if (ServiceAlreadyRegistered<T> ()) 
			{
				callback ();
			}

			AddHandle<T> (callback);
		}
		#endregion

		#region Utility Methods
		private static bool ServiceAlreadyRegistered<T>()
		{
			if (servicesDictionary.ContainsKey (typeof(T).Name)) 
			{
				return true;
			}

			return false;
		}

		private static void ReplaceService<T>(T NewService)
		{
			Debug.LogWarning ("Service : " + typeof(T).Name + " is being replaced.");
			servicesDictionary[typeof(T).Name] = NewService;
		}

		private static void DispatchRegistrationHandles<T>()
		{
			if (HandleTypeAlreadyRegistered<T>()) 
			{
				DispatchHandles (m_callbackHandles [typeof(T).Name]);
			}

			//RemoveHandles<T> ();
		}
		#endregion

		#region Low Level Functions
		private static void RemoveHandles<T>()
		{
			if (!HandleTypeAlreadyRegistered<T> ())
				return;

			m_callbackHandles.Remove (typeof(T).Name);
		}

		private static void Initialize()
		{
			if (m_instance != null)
				return;

			m_instance = new ServiceLocator ();
			servicesDictionary = new Dictionary < string, object >();
			m_callbackHandles = new Dictionary < string, List<Action>> ();
		}

		private static void AddHandle<T>(Action Handle)
		{
			if(!HandleTypeAlreadyRegistered<T>())
				m_callbackHandles [typeof(T).Name] = new List<Action> ();

			m_callbackHandles [typeof(T).Name].Add (Handle);
		}

		private static void DispatchHandles(List<Action> Handles)
		{
			foreach (Action handle in Handles) 
			{
				handle ();
			}
		}

		private static bool HandleTypeAlreadyRegistered<T>()
		{
			if (m_callbackHandles.ContainsKey (typeof(T).Name))
				return true;
			return false;
		}

		private static void UnregisterService<T>()
		{
			servicesDictionary [typeof(T).Name] = default(T);
		}

		#endregion
	}
}
