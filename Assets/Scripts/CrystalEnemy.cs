using UnityEngine;

public class CrystalEnemy : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public float floatAmplitude = 0.5f;
    public float floatFrequency = 1f;

    private Vector3 startPosition;
    private GameObject warningCanvas;

    void Start()
    {
        startPosition = transform.position;

        // Look for the inactive WarningCanvas by name
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (var obj in allObjects)
        {
            if (obj.name == "CrystalEnemyText")
            {
                warningCanvas = obj;
                break;
            }
        }

        if (warningCanvas != null)
        {
            warningCanvas.SetActive(true);
            StartCoroutine(DisableWarningAfterDelay(3f));
        }
        else
        {
            Debug.LogWarning("WarningCanvas not found (even among inactive objects).");
        }
    }

    void Update()
    {
        // Rotate around Y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // Float up and down
        float newY = startPosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private System.Collections.IEnumerator DisableWarningAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (warningCanvas != null)
        {
            warningCanvas.SetActive(false);
        }
    }
}
