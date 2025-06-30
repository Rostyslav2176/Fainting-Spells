using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [System.Serializable]
    public class SpawnCondition
    {
        public bool useTimeSinceLastSpawn = false;
        public float timeSinceLastSpawn = 5f;

        public bool useOnDeathTrigger = false;

        public bool useMinuteTrigger = false;
        public int minuteToSpawn = 1;
    }

    [System.Serializable]
    public class EnemySpawnData
    {
        public GameObject enemyPrefab;
        public float spawnDelay;
    }

    [System.Serializable]
    public class SpawnPoint
    {
        public Transform spawnTransform;
        public List<EnemySpawnData> enemySpawnList = new List<EnemySpawnData>();
        public int maxEnemiesAtOnce = 5;
        public SpawnCondition spawnCondition = new SpawnCondition();

        [HideInInspector] public bool isActive = true;
        [HideInInspector] public int currentSpawnIndex = 0;
        [HideInInspector] public float spawnTimer = 0f;
        [HideInInspector] public List<GameObject> activeEnemies = new List<GameObject>();
        [HideInInspector] public float lastSpawnTime = -Mathf.Infinity;
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

    private IEnumerator SpawnCycle()
    {
        while (true)
        {
            if (Timer.Instance != null && Timer.Instance.TimeRemaining <= 0f)
            {
                Debug.Log("Spawning stopped. Game session ended.");
                yield break;
            }

            float elapsed = Timer.Instance != null ? Timer.Instance.ElapsedTime : 0f;

            foreach (var point in spawnPoints)
            {
                if (!point.isActive || point.enemySpawnList.Count == 0)
                    continue;

                point.spawnTimer += Time.deltaTime;

                bool canSpawn = point.activeEnemies.Count < point.maxEnemiesAtOnce;
                bool spawnByTime = point.spawnCondition.useTimeSinceLastSpawn &&
                                   Time.time - point.lastSpawnTime >= point.spawnCondition.timeSinceLastSpawn;

                bool spawnByMinute = point.spawnCondition.useMinuteTrigger &&
                                     Mathf.FloorToInt(elapsed / 60f) >= point.spawnCondition.minuteToSpawn;

                bool spawnReady = (!point.spawnCondition.useTimeSinceLastSpawn && !point.spawnCondition.useMinuteTrigger) ||
                                  spawnByTime || spawnByMinute;

                if (canSpawn && spawnReady &&
                    point.spawnTimer >= point.enemySpawnList[point.currentSpawnIndex].spawnDelay)
                {
                    SpawnEnemy(point);
                    point.spawnTimer = 0f;
                }
            }

            yield return null;
        }
    }

    private void SpawnEnemy(SpawnPoint point)
    {
        if (point.enemySpawnList.Count == 0)
        {
            Debug.LogWarning("No enemy prefabs in spawn list for spawn point: " + point.spawnTransform.name);
            return;
        }

        var spawnData = point.enemySpawnList[point.currentSpawnIndex];

        GameObject enemy = Instantiate(spawnData.enemyPrefab, point.spawnTransform.position, Quaternion.identity);
        point.activeEnemies.Add(enemy);
        point.lastSpawnTime = Time.time;

        EnemyHealth health = enemy.GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.SetSpawner(this, point);
        }
        else
        {
            Debug.LogWarning("Spawned enemy missing EnemyHealth component: " + enemy.name);
        }

        point.currentSpawnIndex = (point.currentSpawnIndex + 1) % point.enemySpawnList.Count;
    }

    public void NotifyEnemyDeath(SpawnPoint point, GameObject enemy)
    {
        if (point.activeEnemies.Contains(enemy))
        {
            point.activeEnemies.Remove(enemy);

            if (SaveSystem.Instance != null)
            {
                SaveSystem.Instance.AddKill();
            }

            if (point.spawnCondition.useOnDeathTrigger)
            {
                SpawnEnemy(point);
            }
        }
    }
}
