using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;
    public GameObject resultsCanvas;
    
    private PlayerMovement playerMovement;
    private PlayerCamera playerCamera;
    private PlayerProjectileCasting playerProjectileCasting;
    
    void Start()
    {
      currentHealth = maxHealth;
      playerMovement = Object.FindFirstObjectByType<PlayerMovement>();
      playerCamera = Object.FindFirstObjectByType<PlayerCamera>();
      playerProjectileCasting = Object.FindFirstObjectByType<PlayerProjectileCasting>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
         Death();
        }
        Debug.Log(currentHealth);
    }
    
    private void Death()
    {
        // Stop Timer
        if (Timer.Instance != null)
        {
            Timer.Instance.enabled = false;
        }

        // Save Stats
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.EndGame();
        }
        
        if (resultsCanvas != null)
        {
            resultsCanvas.SetActive(true);
        }

        // Pause the game
        Time.timeScale = 0f;
        
        if (playerMovement) playerMovement.isPaused = true;
        if (playerCamera) playerCamera.isPaused = true;
        if(playerProjectileCasting)  playerProjectileCasting.isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
    }
}
