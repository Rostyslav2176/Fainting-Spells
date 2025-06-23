using UnityEngine;

public class SkullExplodeState : State
{
    public float explosionRadius = 5f;
    public int damageAmount = 50;
    public LayerMask damageLayerMask;

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

        // Damage all relevant objects in radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, damageLayerMask);
        foreach (Collider hit in hitColliders)
        {
            PlayerHealth player = hit.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damageAmount);
            }
        }

        // Add VFX

        // Destroy self
        Destroy(transform.root.gameObject);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
