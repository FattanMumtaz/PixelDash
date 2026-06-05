using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using PixDash.Player;

namespace PixDash.UI
{
    public class HealthBarUI : MonoBehaviour
    {
        public Health health;

        [Header("Heart Icons System")]
        [Tooltip("List of heart GameObjects in order (Heart 1, Heart 2, Heart 3).")]
        public GameObject[] hearts;

        [Header("Classic Bar System (Optional)")]
        public Image fillImage;
        public TextMeshProUGUI healthText;

        private void Start()
        {
            // Jika kolom Health di Inspector kosong, cari Player secara otomatis di Scene
            if (health == null)
            {
                PixDash.Player.Player player = FindFirstObjectByType<PixDash.Player.Player>();
                if (player != null)
                {
                    health = player.GetComponent<Health>();
                }
            }

            if (health != null)
            {
                health.onHealthChanged.AddListener(UpdateHealthBar);
                UpdateHealthBar(health.currentHealth);
            }
        }

        private void UpdateHealthBar(int currentHealth)
        {
            // Perbarui tampilan sistem ikon hati (Heart System)
            if (hearts != null && hearts.Length > 0)
            {
                for (int i = 0; i < hearts.Length; i++)
                {
                    if (hearts[i] != null)
                    {
                        Image heartImg = hearts[i].GetComponent<Image>();
                        if (heartImg != null)
                        {
                            // Nonaktifkan gambarnya saja agar layout tidak bergeser/melar
                            heartImg.enabled = (i < currentHealth);
                        }
                        else
                        {
                            // Fallback jika tidak ada komponen Image
                            hearts[i].SetActive(i < currentHealth);
                        }
                    }
                }
            }

            // Perbarui tampilan bar (jika masih digunakan)
            if (fillImage != null && health != null)
            {
                float fillAmount = (float)currentHealth / health.maxHealth;
                fillImage.fillAmount = fillAmount;
            }

            if (healthText != null && health != null)
            {
                healthText.text = $"{currentHealth} / {health.maxHealth}";
            }
        }

        private void OnDestroy()
        {
            if (health != null)
            {
                health.onHealthChanged.RemoveListener(UpdateHealthBar);
            }
        }
    }
}
