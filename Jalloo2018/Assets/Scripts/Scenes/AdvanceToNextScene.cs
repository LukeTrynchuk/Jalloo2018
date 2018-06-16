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
            Scene nextScene = GetNextScene();
            AdvanceToScene(nextScene);
        }
        #endregion

        #region Utility Methods
        private void AdvanceToScene(Scene sceneToLoad)
		{
            if (!sceneToLoad.IsValid()) return;
            SceneManager.LoadScene(sceneToLoad.name);
		}
		
		private Scene GetNextScene()
		{
            Scene currentScene = SceneManager.GetActiveScene();
            Scene nextScene = SceneManager.GetSceneByBuildIndex(currentScene.buildIndex + 1);
            return nextScene;
		}
        #endregion
    }
}
