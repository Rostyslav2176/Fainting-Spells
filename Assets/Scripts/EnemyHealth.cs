using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;
    public GameObject deathEffectPrefab;

    private TutorialPortalSpawner tutorialPortalSpawner;
    private EnemySpawn spawner;
    private EnemySpawn.SpawnPoint spawnPoint;

    void Start()
    {
        currentHealth = maxHealth;
        tutorialPortalSpawner = GetComponent<TutorialPortalSpawner>();
    }

    public void SetSpawner(EnemySpawn spawnerRef, EnemySpawn.SpawnPoint pointRef)
    {
        spawner = spawnerRef;
        spawnPoint = pointRef;
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
