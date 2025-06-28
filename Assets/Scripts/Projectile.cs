using UnityEngine;
using System.Collections.Generic;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 5f;
    public GameObject hitEffect;
    
    public Dictionary<string, int> damageTable = new Dictionary<string, int>()
    {
        { "Skull", 10 },
        { "Eye", 15 },
        { "Crystal", 20 }
    };

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out EnemyHealth enemy))
        {
            string enemyTag = collision.gameObject.tag;
            
            int damage = damageTable.ContainsKey(enemyTag) ? damageTable[enemyTag] : 5;

            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
