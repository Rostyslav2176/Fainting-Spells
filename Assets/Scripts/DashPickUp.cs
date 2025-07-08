using UnityEngine;

public class DashPickUp : MonoBehaviour
{
    public AudioClip pickupSound;
    public GameObject spawnEffectPrefab;

    private GameObject activeEffect;

    private void Start()
    {
        if (spawnEffectPrefab != null)
        {
            activeEffect = Instantiate(spawnEffectPrefab, transform.position, Quaternion.identity, transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null && !player.hasDash)
        {
            player.hasDash = true;
            SaveSystem.Instance?.SetDashCollected();

            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            if (activeEffect != null)
                Destroy(activeEffect);

            PickupUIManager.Instance?.ShowPickupMessage("Dash Unlocked");

            Destroy(gameObject);
        }
    }
}