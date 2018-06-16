using UnityEngine;

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
        public PlayFieldPosition Position => m_position;
        public EntityType Type => m_type;
        #endregion

        #region Protected Variables
        protected PlayFieldPosition m_position;
        protected EntityType m_type;
        #endregion
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

    public struct PlayFieldPosition
    {
        public uint X;
        public uint Y;

        public PlayFieldPosition(int x, int y)
        {
            X = (uint)x;
            Y = (uint)y;
        }
    }
}
