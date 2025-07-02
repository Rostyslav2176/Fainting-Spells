using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StatsMenu : MonoBehaviour
{
    public TMP_Text killCountText;
    public TMP_Text timeSurvivedText;
    public TMP_Text dashText;
    public TMP_Text doubleJumpText;

    void Start()
    {
        int kills = PlayerPrefs.GetInt("EnemiesKilled", 0);
        float time = PlayerPrefs.GetFloat("TimeSurvived", 0f);
        bool hadDash = PlayerPrefs.GetInt("HadDash", 0) == 1;
        bool hadDoubleJump = PlayerPrefs.GetInt("HadDoubleJump", 0) == 1;

        killCountText.text = $"Enemies Killed: {kills}";
        timeSurvivedText.text = $"Time Survived: {FormatTime(time)}";
        dashText.text = $"Dash: {(hadDash ? "Used" : "Not Used")}";
        doubleJumpText.text = $"Double Jump: {(hadDoubleJump ? "Used" : "Not Used")}";
    }

    string FormatTime(float seconds)
    {
        int mins = Mathf.FloorToInt(seconds / 60f);
        int secs = Mathf.FloorToInt(seconds % 60f);
        return $"{mins:D2}:{secs:D2}";
    }
}
