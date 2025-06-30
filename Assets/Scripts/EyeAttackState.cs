using System.Collections;
using UnityEngine;

public class EyeAttackState : State
{
    [Header("Movement")]
    public Transform eyeBody;
    public float attackRange = 18f;
    public float moveSpeed = 2f;
    public float moveAmplitude = 1f;
    private float movementTimer = 0f;
    private Vector3 startLocalPos;

    [Header("Flamethrower")]
    public Transform firePoint;
    public float fireInterval = 2f;
    public float flameDuration = 0.5f;
    public float flameRange = 10f;
    public float flameAngle = 45f;
    public GameObject flameEffectPrefab;
    public LayerMask hitMask;
    private float fireCooldown = 0f;

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
            startLocalPos = eyeBody.localPosition;
        else
            Debug.LogError("Missing eyeBody reference.");

        agent = GetComponentInParent<UnityEngine.AI.NavMeshAgent>();
        if (agent == null) Debug.LogError("NavMeshAgent not found on parent.");
    }

    public override State RunCurrentState()
    {
        if (player == null) return this;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            if (agent != null) agent.SetDestination(transform.position);
            if (!isCharging)
            {
                RotateTowardPlayer();
                SideToSideMovement();
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
            StartCoroutine(ChargeAndFireFlamethrower());
            fireCooldown = fireInterval;
        }
    }

    private IEnumerator ChargeAndFireFlamethrower()
    {
        isCharging = true;

        GameObject chargeEffect = null;
        if (chargeEffectPrefab != null && firePoint != null)
        {
            chargeEffect = Instantiate(chargeEffectPrefab, firePoint.position, firePoint.rotation, firePoint);
        }

        yield return new WaitForSeconds(chargeTime);

        if (chargeEffect != null) Destroy(chargeEffect);

        FireFlamethrower();
        isCharging = false;
    }

    private void FireFlamethrower()
    {
        if (firePoint == null || player == null) return;

        // Spawn flame VFX
        GameObject flameVFX = null;
        if (flameEffectPrefab != null)
        {
            flameVFX = Instantiate(flameEffectPrefab, firePoint.position, firePoint.rotation, firePoint);
            Destroy(flameVFX, flameDuration);
        }

        // Damage player if in cone
        Vector3 toPlayer = player.position - firePoint.position;
        float distance = toPlayer.magnitude;

        if (distance <= flameRange)
        {
            float angleToPlayer = Vector3.Angle(firePoint.forward, toPlayer.normalized);
            if (angleToPlayer <= flameAngle / 2f)
            {
                RaycastHit hit;
                if (Physics.Raycast(firePoint.position, toPlayer.normalized, out hit, flameRange, hitMask))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        PlayerHealth playerHealth = hit.collider.GetComponent<PlayerHealth>();
                        if (playerHealth != null)
                        {
                            playerHealth.TakeDamage(1);
                        }
                    }
                }
            }
        }
    }
}
