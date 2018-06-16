using UnityEngine;
using DogHouse.Core.Services;
using DogHouse.Jalloo.Services;

namespace DogHouse.Jalloo.General
{
    /// <summary>
    /// Generate Level will ask the generator
    /// to generate a level.
    /// </summary>
    public class GenerateLevel : MonoBehaviour
    {
        #region Private Variables
        private ServiceReference<ILevelGenerator> levelGenerator = new ServiceReference<ILevelGenerator>();
        #endregion

        #region Main Methods
        public void GenerateALevel(string filePath)
        {
            if (!levelGenerator.isRegistered()) return;

			string path = Application.dataPath + filePath;
            levelGenerator.Reference.GenerateLevel(path);
        }
        #endregion
    }
}
