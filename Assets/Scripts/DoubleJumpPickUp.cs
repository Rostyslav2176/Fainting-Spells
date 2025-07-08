using UnityEngine;

public class DoubleJumpPickUp : MonoBehaviour
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
        if (player != null && !player.hasDoubleJump)
        {
            player.hasDoubleJump = true;
            SaveSystem.Instance?.SetDoubleJumpCollected();

            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            if (activeEffect != null)
                Destroy(activeEffect);

            PickupUIManager.Instance?.ShowPickupMessage("Double Jump Unlocked");

            Destroy(gameObject);
        }
    }
}
