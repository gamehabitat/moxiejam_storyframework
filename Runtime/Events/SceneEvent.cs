using UnityEngine;
using UnityEngine.Events;

namespace StoryFramework.Events
{
    /// <summary>
    /// Event listener for a lockable object.
    /// </summary>
    [AddComponentMenu("MoxieJam/StoryFramework/Events/Scene Event")]
    public class SceneEvent : MonoBehaviour
    {
        /// <summary>
        /// Called on start of this scene.
        /// </summary>
        [SerializeField]
        UnityEvent onSceneLoaded;

        void Start()
        {
            onSceneLoaded.Invoke();
        }
    }
}