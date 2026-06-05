using UnityEngine;

public class Coin : MonoBehaviour
{
    [Tooltip("Jumlah skor yang didapat saat mengambil koin ini.")]
    public int scoreValue = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"[CoinDebug] Tabrakan dengan: {collision.gameObject.name}, Tag: {collision.tag}");

        if (collision.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                Debug.Log($"[CoinDebug] Koin berhasil diambil. Menambahkan skor: {scoreValue}");
                GameManager.Instance.TambahSkor(scoreValue);
            }
            else
            {
                Debug.LogError("[CoinDebug] GameManager.Instance TIDAK DITEMUKAN di scene! Teks skor tidak bisa di-update.");
            }

            Destroy(gameObject);
        }
    }
}