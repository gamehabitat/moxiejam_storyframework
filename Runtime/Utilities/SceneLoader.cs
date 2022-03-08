using UnityEngine;

namespace StoryFramework
{
    /// <summary>
    /// A utility for loading a new scene.
    /// </summary>
    [AddComponentMenu("MoxieJam/StoryFramework/Utilities/Scene Loader")]
    public class SceneLoader : MonoBehaviour
    {
        /// <summary>
        /// What scene to load.
        /// </summary>
        [SerializeField]
        SceneRef sceneToLoad;

        // TODO: Add support for additive loading.
        //[SerializeField]
        //bool additiveLoad;
        
        /// <summary>
        /// Starts loading the scene.
        /// </summary>
        public void LoadScene()
        {
            Game.LoadScene(sceneToLoad.sceneName, false, sceneNameLoaded => Debug.Log($"Scene \"{sceneNameLoaded}\" loaded."));
        }

        /// <summary>
        /// Starts loading a scene.
        /// </summary>
        /// <param name="sceneName">Name of the scene to load</param>
        public void LoadScene(string sceneName)
        {
            Game.LoadScene(sceneName, false, sceneNameLoaded => Debug.Log($"Scene \"{sceneNameLoaded}\" loaded."));
        }
    }
}