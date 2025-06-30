using UnityEngine;
using System.Collections;

public class TutorialEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject spawnEffectPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 1f;

    private bool hasSpawned = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasSpawned && other.CompareTag("Player"))
        {
            hasSpawned = true;
            StartCoroutine(SpawnWithEffect());
        }
    }

    private IEnumerator SpawnWithEffect()
    {
        Vector3 spawnPosition = spawnPoint ? spawnPoint.position : transform.position;

        GameObject spawnEffectInstance = null;

        if (spawnEffectPrefab)
        {
            spawnEffectInstance = Instantiate(spawnEffectPrefab, spawnPosition, Quaternion.identity);
            StartCoroutine(DisableEffectAfterDelay(spawnEffectInstance, 1.5f));
        }

        yield return new WaitForSeconds(spawnDelay);

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    private IEnumerator DisableEffectAfterDelay(GameObject effect, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (effect != null)
        {
            effect.SetActive(false);
        }
    }
}