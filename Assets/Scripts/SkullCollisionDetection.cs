using UnityEngine;

public class SkullCollisionDetection : MonoBehaviour
{
    public SkullChaseState chaseState;

    private void OnTriggerEnter(Collider other)
    {
        // Check if collided with player or object
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Obstacle"))
        {
            if (chaseState != null)
            {
                chaseState.closeToPlayer = true;
            }
        }
    }
}
