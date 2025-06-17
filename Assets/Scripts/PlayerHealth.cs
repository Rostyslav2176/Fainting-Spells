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
        Destroy(gameObject);
    }
}
