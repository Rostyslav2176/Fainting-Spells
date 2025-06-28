using UnityEngine;

public class PlayerProjectileCasting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePointLeft;
    public Transform firePointRight;
    public float projectileForce = 20f;
    public Camera playerCamera;

    public float fireRate = 0.2f;
    private float nextFireTime = 0f;

    public bool isPaused = false;

    private bool useRightHand = true;

    void Update()
    {
        if (isPaused) return;

        bool clicked = Input.GetMouseButtonDown(0);
        bool held = Input.GetMouseButton(0);

        if ((clicked || held) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        Transform firePoint = useRightHand ? firePointRight : firePointLeft;
        useRightHand = !useRightHand;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Vector3 targetDirection;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            targetDirection = (hit.point - firePoint.position).normalized;
        }
        else
        {
            targetDirection = ray.direction;
        }

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(targetDirection));
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.linearVelocity = targetDirection * projectileForce;

        Collider playerCollider = GetComponent<Collider>();
        Collider projectileCollider = projectile.GetComponent<Collider>();
        if (playerCollider != null && projectileCollider != null)
        {
            Physics.IgnoreCollision(playerCollider, projectileCollider);
        }
    }
}
