using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EyeAttackState : State
{
    [Header("Movement")]
    public Transform eyeBody;
    public float attackRange = 18f;

    [Header("Orbit Movement")]
    public float orbitBaseRadius = 6f;
    public float orbitRadiusVariation = 1f;
    public float orbitSpeed = 1f;
    private float orbitAngle = 0f;
    private float repositionTimer = 0f;
    public float repositionInterval = 1.5f;

    [Header("Flamethrower")]
    public Transform firePoint;
    public GameObject flameEffectPrefab;
    public GameObject chargeEffectPrefab;
    public float flameDuration = 5f;
    public float flameRadius = 3f;
    public float flameLength = 10f;
    public float damagePerTick = 3f;
    public float tickInterval = 0.5f;
    public float chargeTime = 0.5f;

    private bool isCharging = false;
    private bool flameActive = false;
    private Coroutine flameCoroutine;

    private GameObject playerObject;
    private Transform player;
    private NavMeshAgent agent;
    private Transform enemyRoot;

    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) player = playerObject.transform;
        else Debug.LogError("Player with tag 'Player' not found.");

        if (eyeBody == null) Debug.LogError("Missing eyeBody reference.");

        agent = GetComponentInParent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent not found on parent.");
        }
        else
        {
            agent.updateRotation = false;
            enemyRoot = agent.transform;
        }
    }

    public override State RunCurrentState()
    {
        if (player == null || agent == null) return this;

        RotateEnemyRootTowardPlayer();

        float distanceToPlayer = Vector3.Distance(enemyRoot.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            RotateTowardPlayer();

            if (!isCharging && !flameActive)
            {
                OrbitAroundPlayer();
                Shooting();
            }
        }
        else
        {
            agent.SetDestination(player.position);
        }

        return this;
    }

    private void RotateEnemyRootTowardPlayer()
    {
        if (enemyRoot == null || player == null) return;

        Vector3 direction = player.position - enemyRoot.position;
        direction.y = 0f;
        if (direction.sqrMagnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        enemyRoot.rotation = Quaternion.Slerp(enemyRoot.rotation, targetRotation, Time.deltaTime * 5f);
    }

    private void RotateTowardPlayer()
    {
        if (eyeBody == null || player == null) return;

        Vector3 directionToPlayer = player.position - eyeBody.position;
        directionToPlayer.y = 0f;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        eyeBody.rotation = Quaternion.Slerp(eyeBody.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OrbitAroundPlayer()
    {
        repositionTimer -= Time.deltaTime;

        if (repositionTimer <= 0f)
        {
            float angleOffset = Random.Range(-15f, 15f);
            float radiusOffset = Random.Range(-orbitRadiusVariation, orbitRadiusVariation);

            orbitAngle += orbitSpeed * 20f + angleOffset;
            if (orbitAngle >= 360f) orbitAngle -= 360f;

            float radius = orbitBaseRadius + radiusOffset;

            Vector3 offset = new Vector3(
                Mathf.Cos(orbitAngle * Mathf.Deg2Rad),
                0f,
                Mathf.Sin(orbitAngle * Mathf.Deg2Rad)
            ) * radius;

            Vector3 targetPosition = player.position + offset;

            if (NavMesh.SamplePosition(targetPosition, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))
            {
                agent.isStopped = false;
                agent.SetDestination(navHit.position);
            }

            repositionTimer = repositionInterval;
        }
    }

    private void Shooting()
    {
        if (flameCoroutine == null)
            flameCoroutine = StartCoroutine(ActivateFlamethrower());
    }

    private IEnumerator ActivateFlamethrower()
    {
        isCharging = true;
        agent.isStopped = true;

        GameObject chargeEffect = null;
        if (chargeEffectPrefab != null && firePoint != null)
        {
            chargeEffect = Instantiate(chargeEffectPrefab, firePoint.position, firePoint.rotation, firePoint);
        }

        yield return new WaitForSeconds(chargeTime);

        if (chargeEffect != null) Destroy(chargeEffect);

        flameActive = true;

        GameObject flameVFX = null;
        if (flameEffectPrefab != null)
        {
            flameVFX = Instantiate(flameEffectPrefab, firePoint.position, firePoint.rotation, firePoint);
            flameVFX.transform.parent = firePoint;
            Destroy(flameVFX, flameDuration);
        }

        float elapsed = 0f;

        while (elapsed < flameDuration)
        {
            if (IsPlayerInFlame())
            {
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage((int)damagePerTick);
                }
            }

            elapsed += tickInterval;
            yield return new WaitForSeconds(tickInterval);
        }

        flameActive = false;
        isCharging = false;
        flameCoroutine = null;
        agent.isStopped = false;
    }

    private bool IsPlayerInFlame()
    {
        if (player == null || firePoint == null) return false;

        Vector3 toPlayer = player.position - firePoint.position;
        toPlayer.y = 0f;

        float distance = toPlayer.magnitude;
        if (distance > flameLength) return false;

        Vector3 flatToPlayer = new Vector3(toPlayer.x, 0f, toPlayer.z);
        Vector3 flameDirection = new Vector3(firePoint.forward.x, 0f, firePoint.forward.z);

        float sideOffset = Vector3.Cross(flameDirection.normalized, flatToPlayer.normalized).magnitude * distance;

        return sideOffset <= flameRadius;
    }

    private void OnDrawGizmosSelected()
    {
        if (firePoint == null) return;

        Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f);

        Vector3 startPos = firePoint.position;
        Vector3 endPos = startPos + firePoint.forward * flameLength;

        int segments = 10;
        float segmentHeight = flameLength / segments;

        for (int i = 0; i <= segments; i++)
        {
            float height = i * segmentHeight;
            Vector3 center = startPos + firePoint.forward * height;
            Gizmos.DrawWireSphere(center, flameRadius);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawLine(startPos, endPos);
    }
}
