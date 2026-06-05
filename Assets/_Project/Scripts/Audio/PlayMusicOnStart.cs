using UnityEngine;

namespace PixDash.Audio
{
    public class PlayMusicOnStart : MonoBehaviour
    {
        [Header("Music Settings")]
        [Tooltip("The audio clip to play as background music when this scene starts.")]
        [SerializeField] private AudioClip musicClip;
        [SerializeField] private bool loop = true;

        [Header("AudioManager Prefab (Fallback)")]
        [Tooltip("If the AudioManager does not exist in the scene, it will be instantiated from this prefab.")]
        [SerializeField] private GameObject audioManagerPrefab;

        private void Start()
        {
            Debug.Log($"[MusicStartDebug] Start dipanggil pada objek '{gameObject.name}'. Clip musik: {(musicClip != null ? musicClip.name : "KOSONG (NULL)")}");

            // Jika AudioManager belum ada di scene (misalnya play langsung dari level editor)
            if (AudioManager.Instance == null && audioManagerPrefab != null)
            {
                Debug.Log("[MusicStartDebug] AudioManager.Instance null. Melakukan Instantiate prefab...");
                Instantiate(audioManagerPrefab);
            }

            // Putar musik jika AudioManager ada dan clip tersedia
            if (AudioManager.Instance != null)
            {
                if (musicClip != null)
                {
                    Debug.Log($"[MusicStartDebug] Memerintahkan AudioManager untuk memutar lagu: {musicClip.name}");
                    AudioManager.Instance.PlayMusic(musicClip, loop);
                }
                else
                {
                    Debug.LogWarning("[MusicStartDebug] Gagal memutar musik karena 'musicClip' kosong (null) di Inspector!");
                }
            }
            else
            {
                Debug.LogError("[MusicStartDebug] Gagal memutar musik karena 'AudioManager.Instance' tidak ditemukan!");
            }
        }
    }
}
