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

    public string spawnerID = "";

    public static class EnemyDamageControl
    {
        public static int crystalEnemyCount = 0;
        public static bool AreEnemiesDamageable => crystalEnemyCount == 0;
    }

    void Start()
    {
        currentHealth = maxHealth;
        tutorialPortalSpawner = GetComponent<TutorialPortalSpawner>();

        if (CompareTag("Crystal"))
        {
            EnemyDamageControl.crystalEnemyCount++;
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

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
                RegisterKill();
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

        if (CompareTag("Crystal"))
        {
            EnemyDamageControl.crystalEnemyCount = Mathf.Max(0, EnemyDamageControl.crystalEnemyCount - 1);
        }

        if (deathEffectPrefab != null)
        {
            GameObject effect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 3f);
        }

        RegisterKill();

        if (tutorialPortalSpawner != null)
        {
            tutorialPortalSpawner.Spawn();
        }

        Destroy(gameObject);
    }

    private void RegisterKill()
    {
        if (SaveSystem.Instance != null && !string.IsNullOrEmpty(spawnerID))
        {
            SaveSystem.Instance.AddKillFromSpawner(spawnerID);
        }
    }
    
    public static void ResetEnemyDamageState()
    {
        EnemyDamageControl.crystalEnemyCount = 0;
    }
}
