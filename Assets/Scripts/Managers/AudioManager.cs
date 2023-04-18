using System;
using UnityEngine;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        
        [Tooltip("First audio source is music source.")]
        [SerializeField] private AudioSource musicAudioSource;
        [Tooltip("Second audio source is sound effects source.")]
        [SerializeField] private AudioSource effectsAudioSource;
        [Tooltip("Third audio source is ambiance source.")]
        [SerializeField] private AudioSource ambianceAudioSource;

        private void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);
            Instance = this;
            DontDestroyOnLoad(this);
        }


        /// <summary>
        /// Used to play audio clips within the game while keep user settings.
        /// </summary>
        /// <param name="clip">Takes in a audio clip to be played.</param>
        /// <param name="audioLoop">Used to determine if the music should loop.</param>
        public void PlayMusicAudio(AudioClip clip, bool audioLoop)
        {
            if (audioLoop)
                musicAudioSource.loop = true;
            musicAudioSource.clip = clip;
            musicAudioSource.Play();
        }

        /// <summary>
        /// Play a audio clip only one time.
        /// </summary>
        /// <param name="clip">Takes in a clip that will be played once.</param>
        public void PlayEffectAudio(AudioClip clip)
        {
            effectsAudioSource.PlayOneShot(clip);
        }

        /// <summary>
        /// Used to play audio clips within the game while keep user settings.
        /// </summary>
        /// <param name="clip">Takes in a audio clip to be played.</param>
        /// <param name="audioLoop">Used to determine if the music should loop.</param>
        public void PlayAmbianceSource(AudioClip clip, bool audioLoop)
        {
            if (audioLoop)
                ambianceAudioSource.loop = true;
            ambianceAudioSource.clip = clip;
            ambianceAudioSource.Play();
        }
    }
}
