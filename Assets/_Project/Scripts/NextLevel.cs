using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Mengecek apakah yang menyentuh adalah si Hero
        if (collision.CompareTag("Player") || collision.name.Contains("Hero"))
        {
            // Langsung panggil nama scene berikutnya
            // Pastikan tulisannya harus SAMA PERSIS (huruf besar kecilnya)
            SceneManager.LoadScene("Scene2");
        }
    }
}