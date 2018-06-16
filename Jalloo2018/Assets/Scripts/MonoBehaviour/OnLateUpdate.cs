using UnityEngine;
using UnityEngine.Events;

namespace DogHouse.Jalloo.Monobehaviours
{
    /// <summary>
    /// On start will invoke a method in the
    /// on start method. Other objects and 
    /// components can subscribe to the event.
    /// </summary>
    public class OnLateUpdate : MonoBehaviour 
    {
        #region Private Variables
        [SerializeField]
        private UnityEvent m_onLateUpdate;
        #endregion

        #region Main Methods
        private void LateUpdate() => m_onLateUpdate?.Invoke();
        #endregion
    }
}
