using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Main Menu Buttons")]
    public GameObject startButton;
    public GameObject settingsButton;
    public GameObject creditsButton;
    public GameObject quitButton;

    [Header("Level Select Buttons")]
    public GameObject tutorialButton;
    public GameObject arenaButton;
    public GameObject backButton;

    public void OnStartButtonPressed()
    {
        // Hide main menu buttons
        startButton.SetActive(false);
        settingsButton.SetActive(false);
        creditsButton.SetActive(false);
        quitButton.SetActive(false);

        // Show level select buttons
        tutorialButton.SetActive(true);
        arenaButton.SetActive(true);
        backButton.SetActive(true);
    }

    public void OnBackButtonPressed()
    {
        // Show main menu buttons
        startButton.SetActive(true);
        settingsButton.SetActive(true);
        creditsButton.SetActive(true);
        quitButton.SetActive(true);

        // Hide level select buttons
        tutorialButton.SetActive(false);
        arenaButton.SetActive(false);
        backButton.SetActive(false);
    }

    public void OnTutorialButtonPressed()
    {
        SceneManager.LoadScene("TutorialLevel");
    }

    public void OnArenaButtonPressed()
    {
        SceneManager.LoadScene("ArenaLevel");
    }

    public void OnSettingsButtonPressed()
    {
        Debug.Log("Settings clicked");
    }

    public void OnCreditsButtonPressed()
    {
        Debug.Log("Credits clicked");
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
