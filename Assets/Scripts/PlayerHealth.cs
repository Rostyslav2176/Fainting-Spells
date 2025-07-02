using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;

    private PlayerMovement playerMovement;
    private PlayerCamera playerCamera;
    private PlayerProjectileCasting playerProjectileCasting;

    public TextMeshProUGUI healthText;
    public GameObject gameOverPanel;

    void Start()
    {
        currentHealth = maxHealth;
        playerMovement = Object.FindFirstObjectByType<PlayerMovement>();
        playerCamera = Object.FindFirstObjectByType<PlayerCamera>();
        playerProjectileCasting = Object.FindFirstObjectByType<PlayerProjectileCasting>();

        UpdateHealthUI();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Death();
        }

        Debug.Log(currentHealth);
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = currentHealth.ToString();
        }
    }

    private void Death()
    {
        // Disable timer
        if (Timer.Instance != null)
        {
            Timer.Instance.enabled = false;
        }

        // Save and finalize stats and show results UI
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.OnPlayerDeath();
        }

        // Pause the game
        Time.timeScale = 0f;

        // Disable player input
        if (playerMovement) playerMovement.isPaused = true;
        if (playerCamera) playerCamera.isPaused = true;
        if (playerProjectileCasting) playerProjectileCasting.isPaused = true;

        // Unlock and show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
}

