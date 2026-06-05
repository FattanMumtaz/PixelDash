using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PixDash.UI
{
    public class MainMenu : MonoBehaviour
    {
        [Header("UI Panels")]
        [Tooltip("The main menu panel containing Play, Options, and Quit buttons.")]
        public GameObject mainPanel;
        [Tooltip("The options/settings panel.")]
        public GameObject optionsPanel;
        [Tooltip("The level selection panel.")]
        public GameObject levelSelectPanel;

        [Header("Level Progress UI")]
        [Tooltip("List of level buttons in order (Level 1, Level 2, Level 3, etc.).")]
        public Button[] levelButtons;

        [Header("Audio Settings UI")]
        [Tooltip("Slider for controlling Music volume.")]
        public Slider musicSlider;
        [Tooltip("Slider for controlling SFX volume.")]
        public Slider sfxSlider;

        private void Start()
        {
            // Ensure only the main panel is active when the game starts
            ShowMainPanel();

            // Initialize the Audio Settings UI
            InitializeAudioUI();
        }

        /// <summary>
        /// Triggered when the Play button is clicked. Opens the Level Select Panel.
        /// </summary>
        public void PlayGame()
        {
            ShowLevelSelectPanel();
        }

        /// <summary>
        /// Loads a specific scene by name (used by level selection buttons).
        /// </summary>
        /// <param name="levelName">Name of the scene to load.</param>
        public void LoadLevel(string levelName)
        {
            // Reset Time.timeScale in case it was paused
            Time.timeScale = 1f;
            SceneManager.LoadScene(levelName);
        }

        /// <summary>
        /// Shows the Main Panel and hides others.
        /// </summary>
        public void ShowMainPanel()
        {
            if (mainPanel != null) mainPanel.SetActive(true);
            if (optionsPanel != null) optionsPanel.SetActive(false);
            if (levelSelectPanel != null) levelSelectPanel.SetActive(false);
        }

        /// <summary>
        /// Shows the Options Panel and hides others.
        /// </summary>
        public void ShowOptionsPanel()
        {
            if (mainPanel != null) mainPanel.SetActive(false);
            if (optionsPanel != null) optionsPanel.SetActive(true);
            if (levelSelectPanel != null) levelSelectPanel.SetActive(false);
        }

        /// <summary>
        /// Shows the Level Select Panel and hides others.
        /// </summary>
        public void ShowLevelSelectPanel()
        {
            if (mainPanel != null) mainPanel.SetActive(false);
            if (optionsPanel != null) optionsPanel.SetActive(false);
            if (levelSelectPanel != null) levelSelectPanel.SetActive(true);

            // Perbarui status tombol level (terkunci/terbuka)
            UpdateLevelButtonsState();
        }

        /// <summary>
        /// Updates the interactable status of level buttons based on player progression.
        /// </summary>
        public void UpdateLevelButtonsState()
        {
            if (levelButtons == null || levelButtons.Length == 0) return;

            // Dapatkan level tertinggi yang terbuka (default level 1 terbuka)
            int highestUnlocked = PlayerPrefs.GetInt("HighestUnlockedLevel", 1);

            for (int i = 0; i < levelButtons.Length; i++)
            {
                int levelNumber = i + 1;

                if (levelNumber <= highestUnlocked)
                {
                    // Level sudah terbuka, bisa diklik
                    levelButtons[i].interactable = true;
                }
                else
                {
                    // Level masih terkunci, tidak bisa diklik
                    levelButtons[i].interactable = false;
                }
            }
        }

        /// <summary>
        /// Initializes the sliders with saved volume settings and adds listeners.
        /// </summary>
        private void InitializeAudioUI()
        {
            if (musicSlider != null)
            {
                float savedMusic = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
                musicSlider.value = savedMusic;
                musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            }

            if (sfxSlider != null)
            {
                float savedSFX = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
                sfxSlider.value = savedSFX;
                sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
            }
        }

        /// <summary>
        /// Callback for when the music volume slider is dragged/changed.
        /// </summary>
        public void OnMusicVolumeChanged(float value)
        {
            PlayerPrefs.SetFloat("MusicVolume", value);
            PlayerPrefs.Save();

            // Notify AudioManager if it exists
            var audioManager = FindFirstObjectByType<PixDash.Audio.AudioManager>();
            if (audioManager != null)
            {
                audioManager.ApplyAudioSettings();
            }
        }

        /// <summary>
        /// Callback for when the SFX volume slider is dragged/changed.
        /// </summary>
        public void OnSFXVolumeChanged(float value)
        {
            PlayerPrefs.SetFloat("SFXVolume", value);
            PlayerPrefs.Save();

            // Notify AudioManager if it exists
            var audioManager = FindFirstObjectByType<PixDash.Audio.AudioManager>();
            if (audioManager != null)
            {
                audioManager.ApplyAudioSettings();
            }
        }

        /// <summary>
        /// Quits the game application.
        /// </summary>
        public void QuitGame()
        {
            Debug.Log("Game is quitting...");
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}
