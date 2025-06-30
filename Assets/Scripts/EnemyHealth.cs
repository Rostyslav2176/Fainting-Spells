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

    void Start()
    {
        currentHealth = maxHealth;
        tutorialPortalSpawner = GetComponent<TutorialPortalSpawner>();

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
