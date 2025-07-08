using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public GameObject menuHUD;

    private bool isPaused = false;

    private PlayerMovement playerMovement;
    private PlayerCamera playerCamera;
    private PlayerProjectileCasting playerProjectileCasting;
    


    void Start()
    {
        playerMovement = Object.FindFirstObjectByType<PlayerMovement>();
        playerCamera = Object.FindFirstObjectByType<PlayerCamera>();
        playerProjectileCasting = Object.FindFirstObjectByType<PlayerProjectileCasting>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenuPanel.SetActive(true);
        menuHUD.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;

        if (playerMovement) playerMovement.isPaused = true;
        if (playerCamera) playerCamera.isPaused = true;
        if(playerProjectileCasting)  playerProjectileCasting.isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        menuHUD.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;

        if (playerMovement) playerMovement.isPaused = false;
        if (playerCamera) playerCamera.isPaused = false;
        if(playerProjectileCasting)  playerProjectileCasting.isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void LoadMainMenu()
	{
        EnemyHealth.ResetEnemyDamageState();
        PickUpStats.Instance?.ResetStats();
    	Time.timeScale = 1f;
    	SceneManager.LoadScene("MainMenu");
	}

    public void RestartGame()
    {
        EnemyHealth.ResetEnemyDamageState();
        PickUpStats.Instance?.ResetStats();
        Time.timeScale = 1f;
        SceneManager.LoadScene("ArenaLevel"); 
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
