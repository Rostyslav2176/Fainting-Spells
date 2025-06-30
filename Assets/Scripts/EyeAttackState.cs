using System;
using UnityEngine;
using System.Collections;

public class EyeAttackState : State
{
     [Header("Movement")]
    public Transform eyeBody;
    public float attackRange = 18f;
    public float moveSpeed = 2f;
    public float moveAmplitude = 1f;
    private float movementTimer = 0f;
    private Vector3 startLocalPos;

    [Header("Laser")]
    public Transform firePoint;
    public float fireInterval = 2f;
    public float laserDuration = 0.1f;
    public float laserRange = 20f;
    public LayerMask hitMask;
    public LineRenderer lineRenderer;
    private float fireCooldown = 0f;
    private Vector3 lockedTargetPosition;

    [Header("Charge")]
    public float chargeTime = 0.5f;
    public GameObject chargeEffectPrefab;
    private bool isCharging = false;

    private GameObject playerObject;
    private Transform player;
    private UnityEngine.AI.NavMeshAgent agent;

    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) player = playerObject.transform;
        else Debug.LogError("Player with tag 'Player' not found.");

        if (eyeBody != null)
        {
            startLocalPos = eyeBody.localPosition;
        }
        else
        {
            Debug.LogError("Missing eyeBody reference.");
        }

        agent = GetComponentInParent<UnityEngine.AI.NavMeshAgent>();
        if (agent == null) Debug.LogError("NavMeshAgent not found on parent.");

        if (lineRenderer != null) lineRenderer.enabled = false;
    }

    public override State RunCurrentState()
    {
        if (player == null) return this;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            if (agent != null) agent.SetDestination(transform.position); // stop movement
            if (!isCharging)
            {
                RotateTowardPlayer();
                SideToSideMovement(); // ⬅️ Move side to side only when not charging
            }
            Shooting();
        }
        else
        {
            if (agent != null) agent.SetDestination(player.position);
        }

        return this;
    }

    private void SideToSideMovement()
    {
        if (eyeBody == null) return;

        movementTimer += Time.deltaTime * moveSpeed;
        float offset = Mathf.Sin(movementTimer) * moveAmplitude;
        Vector3 newLocalPos = startLocalPos + eyeBody.right * offset;
        eyeBody.localPosition = newLocalPos;
    }

    private void RotateTowardPlayer()
    {
        if (eyeBody == null || player == null) return;

        Vector3 directionToPlayer = player.position - eyeBody.position;
        directionToPlayer.y = 0f;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        eyeBody.rotation = Quaternion.Slerp(eyeBody.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void Shooting()
    {
        if (isCharging) return;

        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            StartCoroutine(ChargeAndFireLaser());
            fireCooldown = fireInterval;
        }
    }

    private IEnumerator ChargeAndFireLaser()
    {
        isCharging = true;
        lockedTargetPosition = player.position;

        GameObject chargeEffect = null;
        if (chargeEffectPrefab != null && firePoint != null)
        {
            chargeEffect = Instantiate(chargeEffectPrefab, firePoint.position, firePoint.rotation, firePoint);
        }

        yield return new WaitForSeconds(chargeTime);

        if (chargeEffect != null) Destroy(chargeEffect);

        FireLaser();
        isCharging = false;
    }

    private void FireLaser()
    {
        if (firePoint == null) return;

        Vector3 origin = firePoint.position;
        Vector3 direction = (lockedTargetPosition - origin).normalized;

        Ray ray = new Ray(origin, direction);
        RaycastHit hit;
        Vector3 endPoint = origin + direction * laserRange;

        if (Physics.Raycast(ray, out hit, laserRange, hitMask))
        {
            endPoint = hit.point;

            if (hit.collider.CompareTag("Player"))
            {
                PlayerHealth playerHealth = hit.collider.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(1);
                }
            }
        }

        if (lineRenderer != null)
        {
            StartCoroutine(ShowLaserLine(origin, endPoint));
        }
    }

    private IEnumerator ShowLaserLine(Vector3 start, Vector3 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.enabled = true;

        yield return new WaitForSeconds(laserDuration);

        lineRenderer.enabled = false;
    }
}
