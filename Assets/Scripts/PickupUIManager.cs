using UnityEngine;
using TMPro;
using System.Collections;

public class PickupUIManager : MonoBehaviour
{
    public static PickupUIManager Instance;

    public TextMeshProUGUI pickupText;
    public float displayTime = 2f;

    private Coroutine currentRoutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (pickupText != null)
            pickupText.gameObject.SetActive(false);
    }

    public void ShowPickupMessage(string message)
    {
        if (pickupText == null)
            return;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowMessageRoutine(message));
    }

    private IEnumerator ShowMessageRoutine(string message)
    {
        pickupText.text = message;
        pickupText.gameObject.SetActive(true);

        yield return new WaitForSeconds(displayTime);

        pickupText.gameObject.SetActive(false);
    }
}
