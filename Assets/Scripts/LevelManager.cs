using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    [Header("Loading UI")]
    public GameObject loadingScreen;
    public Slider progressBar;

    public void LoadArenaLevel()
    {
        StartCoroutine(LoadLevelAsync("ArenaLevel"));
    }

    public void LoadTutorialLevel()
    {
        StartCoroutine(LoadLevelAsync("TutorialLevel"));
    }

    IEnumerator LoadLevelAsync(string sceneName)
    {
        loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            if (progressBar != null)
                progressBar.value = progress;

            yield return null;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}