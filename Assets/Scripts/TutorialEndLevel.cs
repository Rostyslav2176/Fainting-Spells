using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialEndLevel : MonoBehaviour
{
    public string nextSceneName;

    public void LoadNextStage()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextSceneName);
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
