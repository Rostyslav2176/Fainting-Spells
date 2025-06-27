using UnityEngine;

public class EnemySpawner : MonoBehaviour
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

    private System.Collections.IEnumerator SpawnWithEffect()
    {
        Vector3 spawnPosition = spawnPoint ? spawnPoint.position : transform.position;
        
        if (spawnEffectPrefab)
        {
            Instantiate(spawnEffectPrefab, spawnPosition, Quaternion.identity);
        }
        
        yield return new WaitForSeconds(spawnDelay);

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
