using UnityEngine;
using DogHouse.Jalloo.Levels;
using DogHouse.Core.Services;

namespace DogHouse.Jalloo.Services
{
    /// <summary>
    /// The ILevelGenerator is an interface for 
    /// any level generator. A level generator will
    /// generate the levels for the game.
    /// </summary>
    public interface ILevelGenerator : IService
    {
        event System.Action m_onLevelGenerated;
        event System.Action<PlayfieldData> m_onLevelDataGenerated;

        void GenerateLevel(string filePath);
    }
}
