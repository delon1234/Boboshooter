using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    private static readonly List<Bullet> activeBullets = new();
    public static IReadOnlyList<Bullet> ActiveBullets => activeBullets;

    private static int enemyBulletLayer = -1;
    private static int playerBulletLayer = -1;
    private static int enemyLayer = -1;
    private static int playerLayer = -1;

    private Rigidbody2D rb;
    private float damage;
    private float speed;
    private float lifetime;

    public GameObject Owner { get; private set; }

    private ObjectPool<Bullet> associatedPool;
    private Coroutine lifetimeCoroutine;

    private Vector3 originalScale;

    public bool IsEnemyBullet
    {
        get
        {
            if (enemyBulletLayer == -1) enemyBulletLayer = LayerMask.NameToLayer("EnemyBullet");
            return gameObject.layer == enemyBulletLayer;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;

        if (enemyBulletLayer == -1) enemyBulletLayer = LayerMask.NameToLayer("EnemyBullet");
        if (playerBulletLayer == -1) playerBulletLayer = LayerMask.NameToLayer("PlayerBullet");
        if (enemyLayer == -1) enemyLayer = LayerMask.NameToLayer("Enemy");
        if (playerLayer == -1) playerLayer = LayerMask.NameToLayer("Player");
    }

    private void OnEnable()
    {
        activeBullets.Add(this);
    }

    private void OnDisable()
    {
        activeBullets.Remove(this);
    }

    /// <summary>
    /// Despawns all active bullets in the scene back to their pools.
    /// </summary>
    public static void DespawnAllBullets()
    {
        for (int i = activeBullets.Count - 1; i >= 0; i--)
        {
            if (activeBullets[i] != null)
            {
                activeBullets[i].ReleaseToPool();
            }
        }
    }

    /// <summary>
    /// Despawns all active enemy bullets in the scene.
    /// </summary>
    public static void DespawnAllEnemyBullets()
    {
        for (int i = activeBullets.Count - 1; i >= 0; i--)
        {
            if (activeBullets[i] != null && activeBullets[i].IsEnemyBullet)
            {
                activeBullets[i].ReleaseToPool();
            }
        }
    }

    /// <summary>
    /// Despawns all active player bullets in the scene.
    /// </summary>
    public static void DespawnAllPlayerBullets()
    {
        for (int i = activeBullets.Count - 1; i >= 0; i--)
        {
            if (activeBullets[i] != null && !activeBullets[i].IsEnemyBullet)
            {
                activeBullets[i].ReleaseToPool();
            }
        }
    }

    /// <summary>
    /// Despawns all active bullets shot by a specific owner GameObject.
    /// </summary>
    public static void DespawnBulletsFromOwner(GameObject owner)
    {
        if (owner == null) return;
        for (int i = activeBullets.Count - 1; i >= 0; i--)
        {
            if (activeBullets[i] != null && activeBullets[i].Owner == owner)
            {
                activeBullets[i].ReleaseToPool();
            }
        }
    }

    public void Initialize(WeaponStats weaponStats, ObjectPool<Bullet> pool, GameObject owner = null)
    {
        associatedPool = pool;
        Owner = owner;

        // 1. Initializes bullet with weaponStats
        damage = weaponStats.damage;
        speed = weaponStats.projectileSpeed;
        lifetime = weaponStats.projectileLifetime;

        // 2. Set movement
        rb.linearVelocity = transform.right * weaponStats.projectileSpeed;
        float finalScaleMultiplier = weaponStats.projectileScale > 0f ? weaponStats.projectileScale : 1f;
        transform.localScale = originalScale * finalScaleMultiplier;

        // 3. Return bullet to pool after lifetime
        if (lifetimeCoroutine != null)
        {
            StopCoroutine(lifetimeCoroutine);
        }
        lifetimeCoroutine = StartCoroutine(DeactivateAfterDelay(lifetime));
    }

    // Overload for backwards compatibility
    public void Initialize(WeaponStats weaponStats, ObjectPool<Bullet> pool, bool isEnemyBullet)
    {
        Initialize(weaponStats, pool);
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
        // Ignore same team collisions using layer checks
        GameObject hitObject = collision.attachedRigidbody != null ? collision.attachedRigidbody.gameObject : collision.gameObject;
        int hitLayer = hitObject.layer;

        if (IsEnemyBullet && (hitLayer == enemyLayer || hitObject.CompareTag("Enemy") || hitObject.GetComponent<BasicEnemy>() != null)) return;
        if (!IsEnemyBullet && (hitLayer == playerLayer || hitObject.CompareTag("Player") || hitObject.GetComponent<Player>() != null)) return;

        // Universal logic for bullet to deal damage to Player/Enemy
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(new DamageInfo(damage, gameObject, transform.right));
            Debug.Log($"Entity: {damageable} takes {damage} damage");
        }
        ReleaseToPool();
    }
}

