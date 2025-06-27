using UnityEngine;
using UnityEngine.UI;

public class CameraSettingsUI : MonoBehaviour
{
    public Slider sensitivitySlider;
    public PlayerCamera playerCamera;

    public Text sensitivityValueText;

    private void Start()
    {
        float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1.0f);
        sensitivitySlider.value = savedSensitivity;
        ApplySensitivity(savedSensitivity);

        sensitivitySlider.onValueChanged.AddListener(ApplySensitivity);
    }

    public void ApplySensitivity(float value)
    {
        if (playerCamera != null)
        {
            playerCamera.senX = value;
            playerCamera.senY = value;

            if (sensitivityValueText != null)
                sensitivityValueText.text = value.ToString("F2");
        }

        PlayerPrefs.SetFloat("MouseSensitivity", value);
    }
}