using UnityEngine;

using PixDash.Player;

public class DeadZone : MonoBehaviour
{
    private Vector3 spawnPoint;

    void Start()
    {
        // Temukan Player di awal untuk menyimpan posisi start
        Player player = FindFirstObjectByType<Player>();
        if (player != null)
        {
            spawnPoint = player.transform.position;
        }
        else
        {
            spawnPoint = Vector3.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                // Kurangi 1 nyawa
                playerHealth.TakeDamage(1);

                // Jika player belum mati, respawn ke titik awal
                if (!playerHealth.isDead)
                {
                    // Hentikan kecepatan player agar tidak meluncur setelah respawn
                    Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.linearVelocity = Vector2.zero;
                    }

                    // Pindahkan posisi player ke titik awal
                    collision.transform.position = spawnPoint;
                    
                    Debug.Log("Player jatuh ke lubang! Kurang 1 nyawa, respawn ke titik awal.");
                }
            }
        }
    }
}