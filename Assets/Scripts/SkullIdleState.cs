using UnityEngine;

public class SkullIdleState : State
{
    [Header("State References")]
    public SkullChaseState chaseState;

    [Header("Floating Animation")]
    public float moveSpeed = 1.0f;
    public float amplitude = 1.0f;

    [Header("Field of View Settings")]
    public float viewRadius = 5f;
    [Range(0, 360)] public float viewAngle = 90f;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public Transform eyePosition;

    private Transform player;
    private Transform skullBody;
    private Vector3 startPos;
    private bool canSeePlayer;

    private void Start()
    {
        skullBody = transform.parent.parent.Find("SkullBody");
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (skullBody == null || player == null)
        {
            Debug.LogError("Missing SkullBody or Player reference!");
            return;
        }

        startPos = skullBody.localPosition;

        if (skullBody != null)
        {
            skullBody.localPosition = startPos;
        }
    }

    public override State RunCurrentState()
    {
        // Idle floating animation
        float verticalMovement = Mathf.Sin(Time.time * moveSpeed) * amplitude;
        Vector3 newLocalPos = startPos + Vector3.up * verticalMovement;
        skullBody.localPosition = newLocalPos;

        // Player detection
        canSeePlayer = IsPlayerInSight();

        return canSeePlayer ? chaseState : this;
    }

    private bool IsPlayerInSight()
    {
        if (player == null) return false;

        Vector3 origin = eyePosition != null ? eyePosition.position : transform.parent.parent.position;
        Vector3 dirToPlayer = (player.position - origin).normalized;

        // Check angle
        if (Vector3.Angle(transform.parent.parent.forward, dirToPlayer) < viewAngle / 2f)
        {
            float distToPlayer = Vector3.Distance(origin, player.position);

            // Check for obstacle
            if (!Physics.Raycast(origin, dirToPlayer, distToPlayer, obstacleMask))
            {
                return true;
            }
        }

        return false;
    }

    //Visual detection range
    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying && transform.parent == null) return;

        Transform originTransform = transform.parent?.parent;
        if (originTransform == null) return;

        Vector3 origin = eyePosition != null ? eyePosition.position : originTransform.position;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, viewRadius);

        Vector3 forward = originTransform.forward;
        Vector3 left = Quaternion.Euler(0, -viewAngle / 2, 0) * forward;
        Vector3 right = Quaternion.Euler(0, viewAngle / 2, 0) * forward;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(origin, origin + left * viewRadius);
        Gizmos.DrawLine(origin, origin + right * viewRadius);
    }
}