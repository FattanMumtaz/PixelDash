using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI Score")]
    [Tooltip("Text component to display the score.")]
    public TextMeshProUGUI scoreText;
    [Tooltip("Prefix before the score number (e.g., 'x' or empty for just the number).")]
    public string scorePrefix = "";

    [Header("UI Panels")]
    [Tooltip("Panel yang muncul ketika menang (Level Complete).")]
    public GameObject winPanel;
    [Tooltip("Panel yang muncul ketika kalah (Game Over).")]
    public GameObject losePanel;

    [Header("UI Panel Scores")]
    [Tooltip("Teks skor akhir pada panel menang (opsional).")]
    public TextMeshProUGUI winScoreText;
    [Tooltip("Teks skor akhir pada panel kalah (opsional).")]
    public TextMeshProUGUI loseScoreText;

    private int score = 0;

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        // Memaksa scorePrefix kosong agar mengabaikan nilai 'x' yang tersimpan di Inspector
        scorePrefix = "";

        // Pastikan hanya GameManager pada GameObject bernama "GameManager" yang aktif
        if (gameObject.name != "GameManager")
        {
            Debug.LogWarning($"[GameManagerDebug] Menghapus komponen GameManager duplikat pada GameObject '{gameObject.name}' (ID: {GetInstanceID()}). Silakan hapus script ini secara manual di Unity Inspector agar Hierarchy rapi.");
            Destroy(this);
            return;
        }

        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        // Temukan Text_Score otomatis jika kosong di Inspector
        if (scoreText == null)
        {
            GameObject findTextObj = GameObject.Find("Text_Score");
            if (findTextObj != null)
            {
                scoreText = findTextObj.GetComponent<TextMeshProUGUI>();
                if (scoreText != null)
                {
                    Debug.Log("[GameManagerDebug] Berhasil menemukan 'Text_Score' secara otomatis di Scene!");
                }
            }
        }

        FindPanelsInScene();

        UpdateScoreUI();

        Debug.Log($"[GameManagerDebug] Start Selesai pada GameObject '{gameObject.name}' (ID: {GetInstanceID()}). Status Panel -> WinPanel: {(winPanel != null ? $"{winPanel.name} (Scene: {winPanel.scene.name})" : "NULL")}, LosePanel: {(losePanel != null ? $"{losePanel.name} (Scene: {losePanel.scene.name})" : "NULL")}");

        // Sembunyikan panel hasil saat game mulai
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }

    // Fungsi tambah skor
    public void TambahSkor(int jumlah)
    {
        score += jumlah;
        Debug.Log($"[GameManagerDebug] TambahSkor: skor bertambah menjadi {score}");
        UpdateScoreUI();
    }

    // Update tulisan score
    void UpdateScoreUI()
    {
        // Cari cadangan jika masih null
        if (scoreText == null)
        {
            GameObject findTextObj = GameObject.Find("Text_Score");
            if (findTextObj != null)
            {
                scoreText = findTextObj.GetComponent<TextMeshProUGUI>();
            }
        }

        if (scoreText != null)
        {
            Debug.Log($"[GameManagerDebug] UpdateScoreUI: Mengubah teks pada GameObject '{scoreText.gameObject.name}' menjadi '{scorePrefix}{score}'");
            scoreText.text = scorePrefix + score;
        }
        else
        {
            Debug.LogWarning("[GameManagerDebug] UpdateScoreUI: scoreText bernilai NULL! Tidak ada UI yang di-update.");
        }
    }

    // Menang
    public void Win()
    {
        Debug.Log($"[GameManagerDebug] Win() dipanggil pada GameObject '{gameObject.name}' (ID: {GetInstanceID()})! Status winPanel: {(winPanel != null ? winPanel.name : "NULL")}");
        
        if (winPanel == null || winScoreText == null)
        {
            Debug.Log("[GameManagerDebug] Fallback: Mencari panel kemenangan saat Win()...");
            FindPanelsInScene();
        }

        Time.timeScale = 0;

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        if (winScoreText != null)
        {
            winScoreText.text = score.ToString();
        }

        // Logic Pembuka Level Berikutnya
        UnlockNextLevel();
    }

    private void UnlockNextLevel()
    {
        // Mendapatkan nama scene yang aktif saat ini
        string sceneName = SceneManager.GetActiveScene().name;
        
        // Ekstrak angka dari nama scene (misal "Scene1" -> 1, "Level2" -> 2)
        string numberString = System.Text.RegularExpressions.Regex.Replace(sceneName, "[^0-9]", "");
        int currentLevel = 1;

        if (!string.IsNullOrEmpty(numberString) && int.TryParse(numberString, out int parsedLevel))
        {
            currentLevel = parsedLevel;
        }

        // Level berikutnya yang terbuka adalah level saat ini + 1
        int nextLevel = currentLevel + 1;
        int highestUnlocked = PlayerPrefs.GetInt("HighestUnlockedLevel", 1);

        if (nextLevel > highestUnlocked)
        {
            PlayerPrefs.SetInt("HighestUnlockedLevel", nextLevel);
            PlayerPrefs.Save();
            Debug.Log("Level Baru Terbuka! Level tertinggi saat ini: " + nextLevel);
        }
    }

    // Kalah
    public void Lose()
    {
        Debug.Log($"[GameManagerDebug] Lose() dipanggil pada GameObject '{gameObject.name}' (ID: {GetInstanceID()})! Status losePanel: {(losePanel != null ? losePanel.name : "NULL")}");
        
        if (losePanel == null || loseScoreText == null)
        {
            Debug.Log("[GameManagerDebug] Fallback: Mencari panel kekalahan saat Lose()...");
            FindPanelsInScene();
        }

        Time.timeScale = 0;

        if (losePanel != null)
        {
            losePanel.SetActive(true);
        }

        if (loseScoreText != null)
        {
            loseScoreText.text = score.ToString();
        }
    }

    private void FindPanelsInScene()
    {
        GameObject canvasResultObj = null;
        
        // Cari Canvas_Result di semua scene yang sedang di-load (baik active maupun inactive)
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.isLoaded)
            {
                foreach (GameObject obj in scene.GetRootGameObjects())
                {
                    if (obj.name == "Canvas_Result")
                    {
                        canvasResultObj = obj;
                        break;
                    }
                }
            }
            if (canvasResultObj != null) break;
        }

        if (canvasResultObj != null)
        {
            // transform.Find dapat menemukan child yang inactive
            Transform winTrans = canvasResultObj.transform.Find("WinPanel");
            if (winTrans != null)
            {
                winPanel = winTrans.gameObject;
                Debug.Log("[GameManagerDebug] WinPanel berhasil dihubungkan otomatis.");
            }

            Transform loseTrans = canvasResultObj.transform.Find("LosePanel");
            if (loseTrans != null)
            {
                losePanel = loseTrans.gameObject;
                Debug.Log("[GameManagerDebug] LosePanel berhasil dihubungkan otomatis.");
            }
        }
        else
        {
            Debug.LogWarning("[GameManagerDebug] Canvas_Result tidak ditemukan di scene!");
        }

        // Teks skor menang/kalah
        if (winPanel != null && (winScoreText == null || !winScoreText.gameObject.scene.IsValid()))
        {
            Transform scoreTrans = winPanel.transform.Find("Text_WinScore");
            if (scoreTrans == null) scoreTrans = winPanel.transform.Find("Text_Score");
            if (scoreTrans != null)
            {
                winScoreText = scoreTrans.GetComponent<TextMeshProUGUI>();
                Debug.Log("[GameManagerDebug] WinScoreText berhasil dihubungkan.");
            }
        }

        if (losePanel != null && (loseScoreText == null || !loseScoreText.gameObject.scene.IsValid()))
        {
            Transform scoreTrans = losePanel.transform.Find("Text_LoseScore");
            if (scoreTrans == null) scoreTrans = losePanel.transform.Find("Text_Score");
            if (scoreTrans != null)
            {
                loseScoreText = scoreTrans.GetComponent<TextMeshProUGUI>();
                Debug.Log("[GameManagerDebug] LoseScoreText berhasil dihubungkan.");
            }
        }
    }

    /// <summary>
    /// Restarts the current active scene.
    /// </summary>
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Loads the Main Menu scene.
    /// </summary>
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene"); // Ganti jika nama scene menu utama berbeda
    }

    /// <summary>
    /// Loads the next level in the build settings index.
    /// </summary>
    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            // Jika tidak ada level berikutnya, kembali ke menu utama
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}