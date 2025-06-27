using UnityEngine;

public class TutorialText : MonoBehaviour
{
    public GameObject uiTextObject;
    public string playerTag = "Player";

    void Start()
    {
        if (uiTextObject != null)
            uiTextObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (uiTextObject != null)
                uiTextObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (uiTextObject != null)
                uiTextObject.SetActive(false);
        }
    }
}