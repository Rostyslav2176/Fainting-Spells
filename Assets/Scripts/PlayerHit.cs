using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    public GameObject hitCanvasObject;
    public float flashDuration = 0.2f;

    private float flashTimer;
    private bool flashActive = false;

    void Start()
    {
        if (hitCanvasObject == null)
            hitCanvasObject = gameObject;

        hitCanvasObject.SetActive(false);
    }

    void Update()
    {
        if (flashActive)
        {
            flashTimer -= Time.unscaledDeltaTime;
            if (flashTimer <= 0f)
            {
                hitCanvasObject.SetActive(false);
                flashActive = false;
            }
        }
    }

    public void TriggerHitEffect()
    {
        flashTimer = flashDuration;
        hitCanvasObject.SetActive(true);
        flashActive = true;
    }
}
