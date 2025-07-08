using UnityEngine;
using System.Collections.Generic;

public class EnemyKillTracker : MonoBehaviour
{
    public static EnemyKillTracker Instance { get; private set; }

    private Dictionary<string, int> killCounts = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterKill(string spawnerID)
    {
        if (string.IsNullOrEmpty(spawnerID))
            spawnerID = "Unknown";

        if (!killCounts.ContainsKey(spawnerID))
            killCounts[spawnerID] = 0;

        killCounts[spawnerID]++;
    }

    public int GetKillCount(string spawnerID)
    {
        return killCounts.TryGetValue(spawnerID, out int count) ? count : 0;
    }

    public void ResetKills()
    {
        killCounts.Clear();
    }

    public IReadOnlyDictionary<string, int> GetAllKills() => killCounts;
}
