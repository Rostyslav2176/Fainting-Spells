using UnityEngine;
using UnityEngine.UI;

public class TutorialPortalTrigger : MonoBehaviour
{
    private GameObject nextStageCanvas;

    private void Start()
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "NextStage")
            {
                nextStageCanvas = obj;
                break;
            }
        }

        if (nextStageCanvas == null)
        {
            Debug.LogWarning("NextStage canvas not found.");
        }
        else
        {
            nextStageCanvas.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (nextStageCanvas != null)
                nextStageCanvas.SetActive(true);
            else
                Debug.LogWarning("NextStage canvas reference not found.");
        }
    }
}
