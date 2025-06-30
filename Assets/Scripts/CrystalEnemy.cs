using UnityEngine;

public class CrystalEnemy : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public float floatAmplitude = 0.5f;
    public float floatFrequency = 1f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Rotate around Y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // Float up and down
        float newY = startPosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
