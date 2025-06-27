using UnityEngine;
using UnityEngine.UI;

public class TutorialPortalTrigger : MonoBehaviour
{
    public Text nextStageText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (nextStageText != null)
                nextStageText.enabled = true;
            else
                Debug.LogWarning("NextStageText not assigned.");
        }
    }
}
