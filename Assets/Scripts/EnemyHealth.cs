using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public GameObject deathEffectPrefab;

    private TutorialPortalSpawner tutorialPortalSpawner;
    private EnemySpawn spawner;
    private EnemySpawn.SpawnPoint spawnPoint;

    private bool isDead = false;

    [Header("Optional Exploding Logic")]
    public SkullExplodeState explodeState;

    // Global control for damageability based on Crystal enemies
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

        if (spawner == null || spawnPoint == null)
        {
            Debug.LogWarning($"Enemy '{gameObject.name}' does not have a linked spawner. Spawning info may be missing.");
        }
    }

    public void SetSpawner(EnemySpawn spawnerRef, EnemySpawn.SpawnPoint pointRef)
    {
        spawner = spawnerRef;
        spawnPoint = pointRef;
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

        if (spawner != null && spawnPoint != null)
        {
            spawner.NotifyEnemyDeath(spawnPoint, gameObject);
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
