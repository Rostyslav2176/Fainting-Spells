using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;
    public GameObject deathEffectPrefab;
     
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
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }
                
        Destroy(gameObject);
    }
}
