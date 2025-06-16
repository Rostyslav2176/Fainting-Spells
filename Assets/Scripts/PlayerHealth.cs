using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;
    void Start()
    {
      currentHealth = maxHealth;  
    }

    private void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            //player is dead
        }
    }
}
