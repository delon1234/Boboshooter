using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;


public class Bullet : MonoBehaviour
{
    /*
    Values from previous shot persists after being returned to and retrieved from Object Pool
    */
    private Rigidbody2D rb;
    // States of Bullet which can be modified during Update(); thus should have private local copy
    private float damage;
    private float speed;
    private float lifetime;

    private ObjectPool<Bullet> associatedPool;
    private Coroutine lifetimeCoroutine;

    private void Awake() { // Cache references earliest on initialization
        rb = GetComponent<Rigidbody2D>();
    }

    // Do not require Start() as BulletPattern spawns Bullet and calls Initialize

    public void Initialize(WeaponStats weaponStats, ObjectPool<Bullet> pool)
    {
        associatedPool = pool;
        // 1. Initializes bullet with weaponStats
        damage = weaponStats.damage;
        speed = weaponStats.projectileSpeed;
        lifetime = weaponStats.projectileLifetime;
        // 2. Set movement
        rb.linearVelocity = transform.right * weaponStats.projectileSpeed; // Move the bullet in the direction it's facing (right)
        transform.localScale = Vector3.one * weaponStats.projectileScale; // For upgrades that increase scale of bullet
        // 3. Return bullet to pool after lifetime to prevent bullets from permanenently persisting when shot outside of map
        if (lifetimeCoroutine != null) {
            StopCoroutine(lifetimeCoroutine);
        }
        lifetimeCoroutine = StartCoroutine(DeactivateAfterDelay(lifetime));
    }

    private IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ReleaseToPool();
    }
    
    public void ReleaseToPool()
    {
        if (lifetimeCoroutine != null)
        {
            StopCoroutine(lifetimeCoroutine);
            lifetimeCoroutine = null;
        }
        // In case of not using PoolManager
        if (associatedPool != null)
        {
            associatedPool.Release(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {   
        // Universal logic for bullet to deal damage to Player/Enemy
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(new DamageInfo(damage, gameObject, transform.right));
            Debug.Log($"Entity: {damageable} takes {damage} damage");
        }
        ReleaseToPool(); // Release the bullet back to the pool after dealing damage
    }
}
