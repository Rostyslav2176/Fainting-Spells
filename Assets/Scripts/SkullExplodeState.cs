using UnityEngine;

public class SkullExplodeState : State
{
    public float explosionRadius = 5f;
    public int damageAmount = 20;
    public LayerMask damageLayerMask;
    public GameObject explosionEffect;
    public float explosionEffectDuration = 3f;

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
        
        if (explosionEffect != null)
        {
            GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(effect, explosionEffectDuration);
        }
        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, damageLayerMask);
        foreach (Collider hit in hitColliders)
        {
            PlayerHealth player = hit.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damageAmount);
                continue;
            }

            EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
            if (enemy != null && enemy != GetComponentInParent<EnemyHealth>())
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
