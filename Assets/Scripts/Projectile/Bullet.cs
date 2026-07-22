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
    private bool isEnemyBullet;

    private Vector3 originalScale;

    private void Awake() { // Cache references earliest on initialization
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
    }

    // Do not require Start() as BulletPattern spawns Bullet and calls Initialize

    public void Initialize(WeaponStats weaponStats, ObjectPool<Bullet> pool, bool isEnemyBullet)
    {
        associatedPool = pool;
        this.isEnemyBullet = isEnemyBullet;

        // Set Layer so physics ignores collisions on the same team
        int targetLayer = LayerMask.NameToLayer(isEnemyBullet ? "EnemyBullet" : "PlayerBullet");
        if (targetLayer != -1)
        {
            gameObject.layer = targetLayer;
        }

        // 1. Initializes bullet with weaponStats
        damage = weaponStats.damage;
        speed = weaponStats.projectileSpeed;
        lifetime = weaponStats.projectileLifetime;
        // 2. Set movement
        rb.linearVelocity = transform.right * weaponStats.projectileSpeed; // Move the bullet in the direction it's facing (right)
        float finalScaleMultiplier = weaponStats.projectileScale > 0f ? weaponStats.projectileScale : 1f;
        transform.localScale = originalScale * finalScaleMultiplier; // Apply multiplier on top of prefab's original scale
        // 3. Return bullet to pool after lifetime to prevent bullets from permanenently persisting when shot outside of map
        if (lifetimeCoroutine != null) {
            // Prevents double-release bug when Initialize() is called twice on same bullet
            StopCoroutine(lifetimeCoroutine);
        }
        lifetimeCoroutine = StartCoroutine(DeactivateAfterDelay(lifetime));
    }

    private IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ReleaseToPool();
    }
    
    // No need for OnGet() and OnRelease() as state is set in Initialize()

    public void ReleaseToPool()
    {
        if (lifetimeCoroutine != null)
        {
            // Prevents double-release to pool bug when bullet hits enemy
            StopCoroutine(lifetimeCoroutine); // Stops coroutine before release
            lifetimeCoroutine = null;
        }
        // Prevents NullReference in case of not using PoolManager (e.g. Instantiate)
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
        // Ignore same team collisions
        // Check the attached Rigidbody's GameObject if present (e.g. for child colliders like DamageArea)
        GameObject hitObject = collision.attachedRigidbody != null ? collision.attachedRigidbody.gameObject : collision.gameObject;
        if (isEnemyBullet && (hitObject.CompareTag("Enemy") || hitObject.GetComponent<BasicEnemy>() != null)) return;
        if (!isEnemyBullet && (hitObject.CompareTag("Player") || hitObject.GetComponent<Player>() != null)) return;

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
