using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 5f;
    public GameObject hitEffect;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out EnemyHealth enemy))
        {
            enemy.TakeDamage(10);
        }

        Destroy(gameObject);
    }
}
