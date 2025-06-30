using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;
    public GameObject resultsCanvas;
    
    private PlayerMovement playerMovement;
    private PlayerCamera playerCamera;
    private PlayerProjectileCasting playerProjectileCasting;
    
    public TextMeshProUGUI healthText;

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
        if (Timer.Instance != null)
        {
            Timer.Instance.enabled = false;
        }

        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.EndGame();
        }

        if (resultsCanvas != null)
        {
            resultsCanvas.SetActive(true);
        }

        Time.timeScale = 0f;

        if (playerMovement) playerMovement.isPaused = true;
        if (playerCamera) playerCamera.isPaused = true;
        if (playerProjectileCasting) playerProjectileCasting.isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
