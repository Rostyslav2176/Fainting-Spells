using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 5f;
    public GameObject hitEffect;

    void Start()
    {
        Destroy(gameObject, lifeTime); // Auto-destroy failsafe
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) return;

        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject); // Destroy projectile on any collision
    }
}
