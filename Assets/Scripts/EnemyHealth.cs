using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public GameObject deathEffectPrefab;

    private TutorialPortalSpawner tutorialPortalSpawner;

    private bool isDead = false;

    [Header("Optional Exploding Logic")]
    public SkullExplodeState explodeState;
    
    public static class EnemyDamageControl
    {
        public static int crystalEnemyCount = 0;
        public static bool AreEnemiesDamageable => crystalEnemyCount == 0;
    }

    void Start()
    {
        currentHealth = maxHealth;
        tutorialPortalSpawner = GetComponent<TutorialPortalSpawner>();

        // Count Crystal-tagged enemies
        if (CompareTag("Crystal"))
        {
            EnemyDamageControl.crystalEnemyCount++;
        }
    }
    
    public void TakeDamage(int amount)
    {
        if (isDead) return;

        // Block damage for non-Crystal enemies if any Crystal is still alive
        if (!EnemyDamageControl.AreEnemiesDamageable && !CompareTag("Crystal"))
        {
            Debug.Log($"{gameObject.name} is currently undamageable while Crystal enemies are alive.");
            return;
        }

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            if (explodeState != null)
            {
                isDead = true;
                explodeState.RunCurrentState();
            }
            else
            {
                Death();
            }
        }
    }

    public void Death()
    {
        if (isDead) return;
        isDead = true;

        // Decrease Crystal enemy count if this was one
        if (CompareTag("Crystal"))
        {
            EnemyDamageControl.crystalEnemyCount = Mathf.Max(0, EnemyDamageControl.crystalEnemyCount - 1);
        }

        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.AddKill();
        }

        if (tutorialPortalSpawner != null)
        {
            tutorialPortalSpawner.Spawn();
        }

        Destroy(gameObject);
    }
}
