using UnityEngine;

public class SkullCollisionDetection : MonoBehaviour
{
    public SkullChaseState chaseState;
    public SkullIdleState idleState;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Obstacle"))
        {
            if (idleState != null) idleState.closeToPlayer = true;
            if (chaseState != null) chaseState.closeToPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Obstacle"))
        {
            if (idleState != null) idleState.closeToPlayer = false;
            if (chaseState != null) chaseState.closeToPlayer = false;
        }
    }
}