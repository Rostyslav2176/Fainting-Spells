using UnityEngine;

public class FloatAnim : MonoBehaviour
{
    public float floatAmplitude = 0.25f;
    public float floatFrequency = 1f;

    private Vector3 _startPos;

    private void Start()
    {
        _startPos = transform.localPosition;
    }

    private void Update()
    {
        Vector3 newPos = _startPos;
        newPos.y += Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.localPosition = newPos;

        transform.Rotate(Vector3.up * 50f * Time.deltaTime);
    }
}
