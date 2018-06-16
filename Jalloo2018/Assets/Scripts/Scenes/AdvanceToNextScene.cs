using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DogHouse.Jalloo.Scenes
{
    /// <summary>
    /// The Advance to next scene component can be
    /// used to advance to the next scene in the
    /// build settings. If there are no more scenes
    /// to load then this will do nothing.
    /// </summary>
    public class AdvanceToNextScene : MonoBehaviour
    {
        #region Main Methods
        public void AdvanceScene()
        {
            Scene currentScene = GetCurrentScene();
            LoadNextScene(currentScene);
        }
        #endregion

        #region Utility Methods
        private void LoadNextScene(Scene currentScene)
		{
            if (IsLastScene(currentScene)) return;
            SceneManager.LoadScene(currentScene.buildIndex + 1);
		}

        private bool IsLastScene(Scene sceneToTest) => 
            sceneToTest.buildIndex >= SceneManager.sceneCountInBuildSettings;

        private Scene GetCurrentScene() => SceneManager.GetActiveScene();
        #endregion
    }
}
