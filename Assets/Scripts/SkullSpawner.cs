using UnityEngine;
using System.Collections.Generic;

public class SkullSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 3f;
    public int maxEnemies = 10;

    private float timer = 0f;
    private List<GameObject> activeEnemies = new();

    private const string spawnerID = "A";

    void Update()
    {
        activeEnemies.RemoveAll(e => e == null);

        timer += Time.deltaTime;

        if (timer >= spawnInterval && activeEnemies.Count < maxEnemies)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoints.Length == 0) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Vector3 dir = (Vector3.zero - spawnPoint.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(dir);

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, lookRotation);
        activeEnemies.Add(enemy);

        EnemyHealth health = enemy.GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.spawnerID = spawnerID;
        }
    }
}
