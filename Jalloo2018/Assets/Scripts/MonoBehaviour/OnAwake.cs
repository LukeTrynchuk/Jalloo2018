using UnityEngine;
using UnityEngine.Events;

namespace DogHouse.Jalloo.Monobehaviours
{
    /// <summary>
    /// On start will invoke a method in the
    /// on start method. Other objects and 
    /// components can subscribe to the event.
    /// </summary>
    public class OnAwake : MonoBehaviour
    {
        #region Private Variables
        [SerializeField]
        private UnityEvent m_onAwake;
        #endregion

        #region Main Methods
        private void Awake() => m_onAwake?.Invoke();
        #endregion
    }
}
