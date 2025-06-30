using UnityEngine;

public class SkullExplodeState : State
{
    public float explosionRadius = 5f;
    public int damageAmount = 50;
    public LayerMask damageLayerMask;
    public GameObject explosionEffect;

    private bool hasExploded = false;

    public override State RunCurrentState()
    {
        if (!hasExploded)
        {
            Explode();
        }
        
        return null;
    }

    private void Explode()
    {
        hasExploded = true;
        Debug.Log("Exploded");

        // Spawn explosion visual effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Damage all relevant objects in radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, damageLayerMask);
        foreach (Collider hit in hitColliders)
        {
            // Damage player if present
            PlayerHealth player = hit.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damageAmount);
                continue;
            }

            // Damage enemies if present
            EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount);
            }
        }

        // Destroy self after explosion
        Destroy(transform.root.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
