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

    [Header("Credits UI")]
    public GameObject creditsText;
    public GameObject backFromCreditsButton;

    public void OnStartButtonPressed()
    {
        startButton.SetActive(false);
        settingsButton.SetActive(false);
        creditsButton.SetActive(false);
        quitButton.SetActive(false);

        tutorialButton.SetActive(true);
        arenaButton.SetActive(true);
        backButton.SetActive(true);
    }

    public void OnBackButtonPressed()
    {
        startButton.SetActive(true);
        settingsButton.SetActive(true);
        creditsButton.SetActive(true);
        quitButton.SetActive(true);

        tutorialButton.SetActive(false);
        arenaButton.SetActive(false);
        backButton.SetActive(false);
    }

    public void OnCreditsButtonPressed()
    {
        // Hide all menu buttons
        startButton.SetActive(false);
        settingsButton.SetActive(false);
        creditsButton.SetActive(false);
        quitButton.SetActive(false);

        // Show credits text and back-from-credits button
        creditsText.SetActive(true);
        backFromCreditsButton.SetActive(true);
    }

    public void OnBackFromCreditsPressed()
    {
        // Hide credits elements
        creditsText.SetActive(false);
        backFromCreditsButton.SetActive(false);

        // Restore main menu buttons
        startButton.SetActive(true);
        settingsButton.SetActive(true);
        creditsButton.SetActive(true);
        quitButton.SetActive(true);
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

    public void OnQuitButtonPressed()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
