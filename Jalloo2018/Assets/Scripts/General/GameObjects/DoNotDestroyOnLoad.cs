using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DogHouse.Jalloo.General
{
    /// <summary>
    /// The Do Not Destroy on Load component can 
    /// be attached to any game object that should
    /// not be destroyed on load.
    /// </summary>
    public class DoNotDestroyOnLoad : MonoBehaviour
    {
        #region Main Methods
        private void Awake() => DontDestroyOnLoad(gameObject);
		#endregion
	}
}
