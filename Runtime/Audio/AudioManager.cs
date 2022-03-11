using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Pool;

namespace StoryFramework.Audio
{
    /// <summary>
    /// Handles playing of music and sound effects.
    /// </summary>
    [AddComponentMenu("MoxieJam/StoryFramework/Audio/Audio Manager")]
    public class AudioManager : MonoBehaviour
    {
        [SerializeField]
        AudioSource musicSource;

        [SerializeField]
        AudioSource soundEffectSource;

        IObjectPool<AudioSource> audioSourcePool;

        void Awake()
        {
            audioSourcePool = new ObjectPool<AudioSource>(
                CreateAudioSourcePoolItem,
                GetAudioSourceFromPool,
                ReleaseAudioSourceToPool,
                DestroyAudioSourcePoolItem,
                true, 10, 50);
        }

        void OnDestroy()
        {
            audioSourcePool.Clear();
        }

        public void PlayMusic(AudioClip musicClip, float volume = 1.0f, bool loop = true)
        {
            musicSource.volume = volume;
            musicSource.loop = loop;
            if (musicSource.clip == musicClip)
            {
                return;
            }
            musicSource.clip = musicClip;
            musicSource.Play();
        }

        public void StopMusic()
        {
            musicSource.Stop();
        }

        public void PlayEffect(AudioClip audioClip, float volume = 1.0f)
        {
            Assert.IsNotNull(audioClip, "You need to specify a valid audio clip to play a sound.");
            soundEffectSource.clip = audioClip;
            soundEffectSource.volume = volume;
            soundEffectSource.Play();
            
        }

        public void PlayOneShoot(AudioClip audioClip, float volume)
        {
            soundEffectSource.PlayOneShot(audioClip, volume);
        }

        AudioSource CreateAudioSourcePoolItem()
        {
            var obj = new GameObject("AudioSource", typeof(AudioSource));
            obj.transform.SetParent(transform);
            return obj.GetComponent<AudioSource>();
        }

        void DestroyAudioSourcePoolItem(AudioSource obj)
        {
            Destroy(obj);
        }

        void GetAudioSourceFromPool(AudioSource obj)
        {
            obj.clip = null;
            obj.enabled = true;
        }

        void ReleaseAudioSourceToPool(AudioSource obj)
        {
            obj.Stop();
            obj.clip = null;
            obj.enabled = false;
        }
    }
}