using UnityEngine;
using System.Collections.Generic;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 2.5f;
    public GameObject hitEffect;
    
    public Dictionary<string, int> damageTable = new Dictionary<string, int>()
    {
        { "Skull", 20 },
        { "Eye", 10 },
        { "Crystal", 15 }
    };

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hitEffect != null && collision.contacts.Length > 0)
        {
            ContactPoint contact = collision.contacts[0];
            GameObject effect = Instantiate(hitEffect, contact.point, Quaternion.LookRotation(contact.normal));
            Destroy(effect, 1f);
        }

        if (collision.gameObject.TryGetComponent(out EnemyHealth enemy))
        {
            string enemyTag = collision.gameObject.tag;
            int damage = damageTable.ContainsKey(enemyTag) ? damageTable[enemyTag] : 5;
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
