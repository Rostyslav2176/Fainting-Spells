using UnityEngine;

public class EnemyHealth : MonoBehaviour
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
    }
    
    private void Death()
    {
        Destroy(gameObject);
    }
}
