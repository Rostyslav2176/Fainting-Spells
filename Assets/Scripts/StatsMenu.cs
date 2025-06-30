using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StatsMenu : MonoBehaviour
{
    public GameObject resultsMenu;
    public TMP_Text killCountText;
    public TMP_Text timeSurvivedText;
    public Button restartButton;

    void Start()
    {
        int kills = PlayerPrefs.GetInt("EnemiesKilled", 0);
        float time = PlayerPrefs.GetFloat("TimeSurvived", 0f);
        
        killCountText.text = $"Enemies Killed: {kills}";
        timeSurvivedText.text = $"Time Survived: {FormatTime(time)}";

        restartButton.onClick.AddListener(RestartGame);
    }
    
    string FormatTime(float seconds)
    {
        int mins = Mathf.FloorToInt(seconds / 60f);
        int secs = Mathf.FloorToInt(seconds % 60f);
        return $"{mins:D2}:{secs:D2}";
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
