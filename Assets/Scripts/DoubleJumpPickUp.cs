using UnityEngine;

public class DoubleJumpPickUp : MonoBehaviour
{
    public AudioClip pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null && !player.hasDoubleJump)
        {
            player.hasDoubleJump = true;
            SaveSystem.Instance?.SetDoubleJumpCollected();

            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            Destroy(gameObject);
        }
    }
}
