using UnityEngine;
using DogHouse.Core.Services;
using DogHouse.Jalloo.Services;

namespace DogHouse.Jalloo.Levels
{
    /// <summary>
    /// The Entity class is an abstract
    /// class that all objects on a play
    /// field should enherit from.
    /// </summary>
    public abstract class Entity : MonoBehaviour
    {
        #region Public Variables
        public PlayfieldPosition Position => currentPosition;
        public EntityType Type => m_type;
        #endregion

        #region Protected Variables
        protected EntityType m_type;
        #endregion

        protected PlayfieldPosition currentPosition;
        protected ServiceReference<ILevelManager> levelManager = new ServiceReference<ILevelManager>();

        protected virtual void OnEnable()
        {
            currentPosition.X = (int)transform.position.x;
            currentPosition.Y = (int)transform.position.z;
        }
        public void UpdatePosition(int x, int y)
        {
            PlayfieldPosition previous = currentPosition;
            currentPosition.X = x;
            currentPosition.Y = y;
            transform.position = new Vector3(currentPosition.X, 0, currentPosition.Y);
            if(levelManager.isRegistered())
            {
                levelManager.Reference.UpdateMap(this, previous.X, previous.Y);
            }
        }

    }

    public enum EntityType
    {
        BALL,
        PLAYER,
        WALL,
        PANEL,
        ENEMY,
        EMPTY,
        INVALID
    }

    public struct PlayfieldPosition
    {
        public int X;
        public int Y;

        public PlayfieldPosition(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
