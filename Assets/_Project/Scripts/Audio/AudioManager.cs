using UnityEngine;

namespace PixDash.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        private void Awake()
        {
            // Implement Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private void Start()
        {
            // Apply saved audio settings on startup
            ApplyAudioSettings();
        }

        /// <summary>
        /// Applies the saved volume settings from PlayerPrefs to the AudioSources.
        /// </summary>
        public void ApplyAudioSettings()
        {
            float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
            float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

            if (musicSource != null)
            {
                musicSource.volume = musicVolume;
            }

            if (sfxSource != null)
            {
                sfxSource.volume = sfxVolume;
            }
        }

        /// <summary>
        /// Plays background music.
        /// </summary>
        public void PlayMusic(AudioClip clip, bool loop = true)
        {
            if (musicSource == null || clip == null) return;
            
            // Hindari memutar ulang dari awal jika musik yang sama sedang berjalan
            if (musicSource.clip == clip && musicSource.isPlaying) return;
            
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
            musicSource.Play();
        }

        /// <summary>
        /// Plays a one-shot sound effect.
        /// </summary>
        public void PlaySFX(AudioClip clip)
        {
            if (sfxSource == null || clip == null) return;

            sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
            sfxSource.PlayOneShot(clip);
        }
    }
}
