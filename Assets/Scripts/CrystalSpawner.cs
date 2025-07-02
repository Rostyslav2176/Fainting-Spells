using UnityEngine;
using System.Collections.Generic;

public class CrystalSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 3f;
    public int maxEnemies = 10;
    public float spawnDelay = 30f;

    private float timer = 0f;
    private List<GameObject> activeEnemies = new();

    private const string spawnerID = "C";

    void Update()
    {
        if (Timer.Instance == null || Timer.Instance.ElapsedTime < spawnDelay)
            return;

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
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        activeEnemies.Add(enemy);

        EnemyHealth health = enemy.GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.spawnerID = spawnerID;
        }
    }
}
