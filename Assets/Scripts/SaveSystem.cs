using UnityEngine;
using UnityEngine.SceneManagement;

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
    public GameObject resultsCanvas; // Assign in Inspector

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Clear stats when scene loads (new session)
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ClearStats();
        gameEnded = false;
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

    // Called when player dies to save current session stats and show UI
    public void OnPlayerDeath()
    {
        EndGame();

        // Immediately clear stats after saving to prepare for next session (e.g., restart)
        ClearStats();
    }

    public void ClearStats()
    {
        PlayerPrefs.DeleteKey("EnemiesKilled");
        PlayerPrefs.DeleteKey("TimeSurvived");
        PlayerPrefs.DeleteKey("HasDash");
        PlayerPrefs.DeleteKey("HasDoubleJump");
        PlayerPrefs.Save();

        enemiesKilled = 0;
        timeSurvived = 0f;
        hasDash = false;
        hasDoubleJump = false;
        killsFromSpawnerA = 0;
        killsFromSpawnerB = 0;

        Debug.Log("Stats Cleared.");
    }

    public void EndGame()
    {
        if (gameEnded) return;
        gameEnded = true;

        enemiesKilled = killsFromSpawnerA + killsFromSpawnerB;
        SaveStats();

        if (resultsCanvas != null)
        {
            resultsCanvas.SetActive(true);
        }

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

    public void SetDashCollected()
    {
        hasDash = true;
    }

    public void SetDoubleJumpCollected()
    {
        hasDoubleJump = true;
    }
}
