using UnityEngine;

public class TutorialCrystalEnemy : MonoBehaviour
{
    public float rotationSpeed = 45f;
    public float floatAmplitude = 0.5f;
    public float floatFrequency = 1f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Rotate around the Y-axis
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}