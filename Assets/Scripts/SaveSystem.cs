using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }

    public int enemiesKilled = 0;
    public float timeSurvived = 0f;

    private bool gameEnded = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (!gameEnded)
        {
            timeSurvived = Timer.Instance != null ? Timer.Instance.ElapsedTime : timeSurvived;
        }
    }

    public void AddKill()
    {
        enemiesKilled++;
    }

    public void EndGame()
    {
        if (gameEnded) return;

        gameEnded = true;

        SaveStats();
    }

    private void SaveStats()
    {
        PlayerPrefs.SetInt("EnemiesKilled", enemiesKilled);
        PlayerPrefs.SetFloat("TimeSurvived", timeSurvived);
        PlayerPrefs.Save();
        Debug.Log("Stats Saved.");
    }
}
