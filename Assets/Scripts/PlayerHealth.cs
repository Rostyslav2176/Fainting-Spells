using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;
    void Start()
    {
      currentHealth = maxHealth;  
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
        
        SaveSystem.Instance.EndGame();

        GameObject gameOverPanel = GameObject.Find("GameOverPanel");
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        
        Destroy(gameObject);
    }
}
