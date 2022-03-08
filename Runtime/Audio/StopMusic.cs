using UnityEngine;

namespace StoryFramework.Audio
{
    /// <summary>
    /// Stops playing the music. 
    /// </summary>
    [AddComponentMenu("MoxieJam/StoryFramework/Audio/Stop Music")]
    public class StopMusic : MonoBehaviour
    {
        /// <summary>
        /// Stop playing as soon as the script is activated.
        /// </summary>
        [SerializeField]
        bool stopOnStart;

        void Start()
        {
            if (stopOnStart)
            {
                Stop();
            }
        }

        /// <summary>
        /// Stop the music.
        /// </summary>
        public void Stop()
        {
            Game.Instance.AudioManager.StopMusic();
        }
    }
}