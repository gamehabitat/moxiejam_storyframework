using UnityEngine;

namespace StoryFramework.Audio
{
    /// <summary>
    /// Starts playing a sound effect clip. 
    /// </summary>
    [AddComponentMenu("MoxieJam/StoryFramework/Audio/Play Sound")]
    public class PlaySound : MonoBehaviour
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
        /// Start playing as soon as the script is activated.
        /// </summary>
        [SerializeField]
        bool playOnStart;

        void Start()
        {
            if (playOnStart)
            {
                Play();
            }
        }

        /// <summary>
        /// Play the selected audio clip.
        /// </summary>
        public void Play()
        {
            if (audioClip)
            {
                Game.Instance.AudioManager.PlayEffect(audioClip, volume);
            }
        }
    }
}