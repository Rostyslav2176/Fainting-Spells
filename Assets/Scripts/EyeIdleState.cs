using System;
using UnityEngine;

public class EyeIdleState : State
{
    [Header("State References")] 
    public EyeAttackState attackState;
    
    [Header("Field of View Settings")]
    public float viewRadius = 5f;
    [Range(0, 360)] public float viewAngle = 90f;
    public LayerMask obstacleMask;
    public Transform eyePosition;

    public Transform player;
    public Transform eyeBody;
    private Vector3 startPos;
    private bool canSeePlayer;
    
    [Header("Idle Movement")]
    public float moveSpeed = 1.0f;
    public float amplitude = 1.0f;

    private void Start()
    {
        if (eyeBody == null || player == null)
        {
            Debug.LogError("Missing EyeBody or Player reference!");
            return;
        }

        startPos = eyeBody.localPosition;

        if (eyeBody != null)
        {
            eyeBody.localPosition = startPos;
        }
    }

    public override State RunCurrentState()
    {
        IdleMovement();
        
        // Player detection
        canSeePlayer = IsPlayerInSight();
        return canSeePlayer ? attackState : this;
    }
    
    private void IdleMovement()
    {
        if (eyeBody == null) return;

        float verticalMovement = Mathf.Sin(Time.time * moveSpeed) * amplitude;
        Vector3 newLocalPos = startPos + Vector3.up * verticalMovement;
        eyeBody.localPosition = newLocalPos;
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
