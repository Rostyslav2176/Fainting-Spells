using UnityEngine;

public class PlayerProjectileCasting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 20f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Create a ray from the camera to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Calculate direction from fire point to hit point
            Vector3 direction = (hit.point - firePoint.position).normalized;

            // Instantiate the projectile
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(direction));

            // Apply force via Rigidbody
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = direction * projectileSpeed;
            }
        }
    }
}
