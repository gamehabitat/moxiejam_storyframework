using UnityEngine;
using UnityEngine.Assertions;

namespace StoryFramework.Audio
{
    /// <summary>
    /// Starts playing a music clip. 
    /// </summary>
    [AddComponentMenu("MoxieJam/StoryFramework/Audio/Play Music")]
    public class PlayMusic : MonoBehaviour
    {
        /// <summary>
        /// The Audio file to play.
        /// </summary>
        [SerializeField]
        AudioClip audioClip;

        /// <summary>
        /// Volume level.
        /// </summary>
        [SerializeField]
        float volume = 1.0f;

        /// <summary>
        /// Loop the music.
        /// </summary>
        [SerializeField]
        bool loop = true;

        /// <summary>
        /// Start playing as soon as the script is activated.
        /// </summary>
        [SerializeField]
        bool playOnStart = true;

        void Start()
        {
            Assert.IsNotNull(audioClip, "You need to specify a valid audio clip to play a sound.");

            if (playOnStart)
            {
                Play();
            }
        }

        /// <summary>
        /// Play the selected music.
        /// </summary>
        public void Play()
        {
            Game.Instance.AudioManager.PlayMusic(audioClip, volume, loop);
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