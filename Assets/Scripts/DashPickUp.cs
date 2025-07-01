using UnityEngine;

public class DashPickUp : MonoBehaviour
{
    public AudioClip pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null && !player.hasDash)
        {
            player.hasDash = true;
            SaveSystem.Instance?.SetDashCollected();
            Destroy(gameObject);
        }
    }
}