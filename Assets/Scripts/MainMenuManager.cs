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
    public GameObject settingsText;
    public GameObject controlsText;
    public GameObject creditsPanel;
    public GameObject backFromCreditsButton;
    public GameObject backFromSettingsButton;

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
        startButton.SetActive(false);
        settingsButton.SetActive(false);
        creditsButton.SetActive(false);
        quitButton.SetActive(false);
        
        creditsText.SetActive(true);
        creditsPanel.SetActive(true);
        backFromCreditsButton.SetActive(true);
    }

    public void OnBackFromCreditsPressed()
    {
        creditsText.SetActive(false);
        creditsPanel.SetActive(false);
        backFromCreditsButton.SetActive(false);
        
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
        startButton.SetActive(false);
        creditsButton.SetActive(false);
        quitButton.SetActive(false);
        settingsButton.SetActive(false);
        
        settingsText.SetActive(true);
        controlsText.SetActive(true);
        backFromSettingsButton.SetActive(true);
    }

    public void OnBackFromSettingsPressed()
    {
        settingsText.SetActive(false); 
        controlsText.SetActive(false);
        backFromSettingsButton.SetActive(false);
        
        startButton.SetActive(true);
        settingsButton.SetActive(true);
        creditsButton.SetActive(true);
        quitButton.SetActive(true); 
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
