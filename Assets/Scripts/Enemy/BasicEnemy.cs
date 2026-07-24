using System;
using System.Collections;
using UnityEngine;

// Parent Class for all Enemies
public abstract class BasicEnemy : MonoBehaviour
{
    [Header("Stats")]
    public EnemyHealth Health { get; private set; }
    // Each enemy will have own serialisedfields in inspector
    [SerializeField] protected float collisionDamage = 1;
    [SerializeField] protected float speed = 3f;

    [Header("Visuals & Animation")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected bool defaultFacingLeft = true;
    [SerializeField] protected float deathDelay = 0.5f;

    // Player Collision Damage Logic
    private bool onPlayer = false;
    private float timer;
    [SerializeField] private float collisionDamageInterval = 0.5f;

    protected Rigidbody2D rb;
    protected Player player;
    protected Transform playerTransform;
    protected Rigidbody2D playerRb;
    protected Collider2D selfCollider;
    protected bool isDead;

    private EnemyHealthBar healthBar;

    // Parameter hashes for high-performance Animator updates
    protected static readonly int SpeedHash  = Animator.StringToHash("speed");
    protected static readonly int AttackHash = Animator.StringToHash("attack");
    protected static readonly int HurtHash   = Animator.StringToHash("hurt");
    protected static readonly int DieHash    = Animator.StringToHash("die");

    // Pickup Drops. Each Enemy has own LootTable
    [SerializeField] private LootTable lootTable;
    [SerializeField] private PickupBehaviour pickupPrefab;
    [SerializeField] protected Vector3 lootDropOffset = Vector3.zero;
    private RoomRuntime roomRuntime;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Health = GetComponent<EnemyHealth>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        selfCollider = GetComponent<Collider2D>();

        if (animator == null)       animator       = GetComponentInChildren<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        // Refactored to use Player Instance instead of GameObject.FindWithTag
        player = Player.Instance;
        if (player != null)
        {
            playerTransform = player.transform;
            playerRb = player.GetComponent<Rigidbody2D>();
        }

        // Resolve roomRuntime for enemies pre-placed in scenes
        if (roomRuntime == null)
        {
            roomRuntime = GetComponentInParent<RoomRuntime>();
            if (roomRuntime == null && transform.parent != null)
            {
                roomRuntime = transform.parent.GetComponentInChildren<RoomRuntime>();
            }
        }
    }

    public void Initialize(RoomRuntime roomRuntime)
    {
        this.roomRuntime = roomRuntime;
    }

    public void TakeDamage(DamageInfo damageinfo)
    {
        Health.TakeDamage(damageinfo);
    }

    // Useful info for Enemy Variations to know direction to player
    protected Vector2 GetDirectionToPlayer()
    {
        if (playerTransform == null) return Vector2.zero;
        return (playerTransform.position - transform.position).normalized;
    }

    protected float GetDistanceToPlayer()
    {
        if (playerTransform == null) return float.MaxValue;
        return Vector2.Distance(transform.position, playerTransform.position);
    }

    protected Vector2 GetPredictedPlayerPosition(float leadTime = 0.35f)
    {
        if (playerTransform == null) return transform.position;
        if (playerRb != null)
        {
            return (Vector2)playerTransform.position + (playerRb.linearVelocity * leadTime);
        }
        return playerTransform.position;
    }

    protected Vector2 GetPredictiveDirectionToPlayer(float leadTime = 0.35f)
    {
        if (playerTransform == null) return Vector2.zero;
        Vector2 target = GetPredictedPlayerPosition(leadTime);
        return (target - (Vector2)transform.position).normalized;
    }

    // Collide with Player, start timer for damaging
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            onPlayer = true;
            timer = 0f;
        }
    }

    // Stop dealing Damage
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            onPlayer = false;
        }
    }

    protected void DealDamageToPlayer(float amt)
    {
        DamageInfo dmg = new DamageInfo(amt, gameObject, GetDirectionToPlayer());
        player.TakeDamage(dmg);
    }

    // Damage Logic
    private void DamageLogic()
    {
        timer -= Time.deltaTime;
        if (onPlayer && timer <= 0f)
        {   
            DealDamageToPlayer(collisionDamage);
            timer = collisionDamageInterval;
        }
    }

    protected virtual void UpdateFacingDirection()
    {
        if (playerTransform != null && spriteRenderer != null)
        {
            Vector2 dir = GetDirectionToPlayer();
            if (Mathf.Abs(dir.x) > 0.05f)
            {
                bool movingRight = dir.x > 0f;
                spriteRenderer.flipX = defaultFacingLeft ? movingRight : !movingRight;
            }
        }
    }

    protected virtual float GetAnimationSpeed()
    {
        return rb != null ? rb.linearVelocity.sqrMagnitude : 0f;
    }

    protected virtual void UpdateAnimator()
    {
        if (animator != null)
        {
            animator.SetFloat(SpeedHash, GetAnimationSpeed());
        }
    }

    protected virtual void HandleHurt(DamageInfo damageInfo)
    {
        if (isDead || animator == null) return;
        animator.ResetTrigger(HurtHash);
        animator.SetTrigger(HurtHash);
    }

    protected virtual void TriggerAttackAnimation()
    {
        if (animator != null)
        {
            animator.ResetTrigger(AttackHash);
            animator.SetTrigger(AttackHash);
        }
    }

    // Each type of enemy must implement their own Walk Pattern
    protected abstract void WalkLogic();

    protected void DropLoot()
    {
        // No Serialized Loot Table, maybe never drops loot
        if (lootTable == null)
        {
            return;
        }
        // Chooses random loot, if null, means No Loot, return
        LootWeightEntry RandomLoot = WeightedRandom.Pick(lootTable.LootDropTable);
        if (RandomLoot.definition == null)
        {
            return;
        }
        PickupBehaviour PickupObject = Instantiate(pickupPrefab, transform.position + lootDropOffset, Quaternion.identity, roomRuntime != null ? roomRuntime.transform : null);
        PickupObject.Initialize(RandomLoot.definition);
    }

    // Common Handling of Enemy Death
    public virtual void OnEnemyDeath(BasicEnemy enemy)
    {
        if (isDead) return;
        isDead = true;

        if (rb != null)           rb.linearVelocity = Vector2.zero;
        if (selfCollider != null) selfCollider.enabled = false;
        if (animator != null)      animator.SetTrigger(DieHash);

        // Despawn bullets shot by this specific enemy on death
        Bullet.DespawnBulletsFromOwner(gameObject);

        StartCoroutine(HandleDeathRoutine());
    }

    protected virtual IEnumerator HandleDeathRoutine()
    {
        yield return new WaitForSeconds(deathDelay);
        DropLoot();
        Destroy(gameObject);
    }

    // Subscribe to its own EnemyDeath and TakingDamage
    protected virtual void OnEnable()
    {
        if (Health == null) Health = GetComponent<EnemyHealth>();
        if (Health != null)
        {
            Health.OnEnemyDied += OnEnemyDeath;
            Health.OnTakingDamage += HandleHurt;
        }
    }

    protected virtual void OnDisable()
    {
        if (Health != null)
        {
            Health.OnEnemyDied -= OnEnemyDeath;
            Health.OnTakingDamage -= HandleHurt;
        }
    }

    protected virtual void Update()
    {
        if (isDead) return;

        DamageLogic();
        UpdateFacingDirection();
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        WalkLogic();
    }
}