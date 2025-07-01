using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }

    public int enemiesKilled = 0;
    public float timeSurvived = 0f;

    public bool hasDash = false;
    public bool hasDoubleJump = false;

    private bool gameEnded = false;

    public int killsFromSpawnerA = 0;
    public int killsFromSpawnerB = 0;

    [Header("UI References")]
    public GameObject resultsCanvas; // Assign this in the Inspector

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

    public void AddKillFromSpawner(string spawnerID)
    {
        switch (spawnerID)
        {
            case "A":
                killsFromSpawnerA++;
                break;
            case "B":
                killsFromSpawnerB++;
                break;
        }
    }

    public void OnPlayerDeath()
    {
        EndGame();
    }

    public void EndGame()
    {
        if (gameEnded) return;
        gameEnded = true;

        enemiesKilled = killsFromSpawnerA + killsFromSpawnerB;
        SaveStats();

        // Show end menu if assigned
        if (resultsCanvas != null)
        {
            resultsCanvas.SetActive(true);
        }

        // Pause game and show cursor
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void SaveStats()
    {
        PlayerPrefs.SetInt("EnemiesKilled", enemiesKilled);
        PlayerPrefs.SetFloat("TimeSurvived", timeSurvived);
        PlayerPrefs.SetInt("HasDash", hasDash ? 1 : 0);
        PlayerPrefs.SetInt("HasDoubleJump", hasDoubleJump ? 1 : 0);
        PlayerPrefs.Save();

        Debug.Log("Stats Saved.");
    }

    public void SetDashCollected() => hasDash = true;
    public void SetDoubleJumpCollected() => hasDoubleJump = true;
}
