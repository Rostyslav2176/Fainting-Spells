using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
     [System.Serializable]
    public class SpawnPoint
    {
        public Transform spawnTransform;
        public float activationTime;
        public List<EnemySpawnData> enemySpawnList;
        public int maxEnemiesAtOnce = 5;

        [HideInInspector] public bool isActive = false;
        [HideInInspector] public int currentSpawnIndex = 0;
        [HideInInspector] public float spawnTimer = 0f;
        [HideInInspector] public List<GameObject> activeEnemies = new List<GameObject>();
    }

    [System.Serializable]
    public class EnemySpawnData
    {
        public GameObject enemyPrefab;
        public float spawnDelay;
    }

    public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

    private void Start()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("No spawn points assigned.");
            return;
        }

        StartCoroutine(SpawnCycle());
    }

    IEnumerator SpawnCycle()
    {
        while (true)
        {
            if (Timer.Instance.TimeRemaining <= 0f)
            {
                Debug.Log("Spawning stopped. Game session ended.");
                yield break;
            }

            float elapsed = Timer.Instance.ElapsedTime;

            foreach (var point in spawnPoints)
            {
                // Activate the spawn point
                if (!point.isActive && elapsed >= point.activationTime)
                {
                    point.isActive = true;
                    Debug.Log($"Spawn point activated at {elapsed} seconds.");
                }

                // Handle active spawn point
                if (point.isActive && point.enemySpawnList.Count > 0)
                {
                    point.spawnTimer += Time.deltaTime;
                    var spawnData = point.enemySpawnList[point.currentSpawnIndex];

                    if (point.spawnTimer >= spawnData.spawnDelay)
                    {
                        if (point.activeEnemies.Count < point.maxEnemiesAtOnce)
                        {
                            SpawnEnemy(point, spawnData);
                            point.spawnTimer = 0f;
                            
                            point.currentSpawnIndex++;
                            if (point.currentSpawnIndex >= point.enemySpawnList.Count)
                                point.currentSpawnIndex = 0;
                        }
                    }
                }
            }

            yield return null;
        }
    }

    void SpawnEnemy(SpawnPoint point, EnemySpawnData spawnData)
    {
        GameObject enemy = Instantiate(spawnData.enemyPrefab, point.spawnTransform.position, Quaternion.identity);
        point.activeEnemies.Add(enemy);

        EnemyHealth health = enemy.GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.SetSpawner(this, point);
        }
    }

    public void NotifyEnemyDeath(SpawnPoint point, GameObject enemy)
    {
        if (point.activeEnemies.Contains(enemy))
        {
            point.activeEnemies.Remove(enemy);
        }
    }
}