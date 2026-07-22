using System;
using UnityEngine;

// Parent Class for all Enemies
public abstract class BasicEnemy : MonoBehaviour
{
    [Header("Stats")]
    public EnemyHealth Health { get; private set; }
    // Each enemy will have own serialisedfields in inspector
    [SerializeField] protected float collisionDamage = 1;
    [SerializeField] protected float speed = 3f;

    // Player Collision Damage Logic
    private bool onPlayer = false;
    private float timer;
    [SerializeField] private float collisionDamageInterval = 0.5f;

    protected Rigidbody2D rb;
    protected Player player;
    protected Transform playerTransform;
    
    private EnemyHealthBar healthBar;

    // Pickup Drops. Each Enemy has own LootTable
    [SerializeField] private LootTable lootTable;
    [SerializeField] private PickupBehaviour pickupPrefab;
    private RoomRuntime roomRuntime;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Health = GetComponent<EnemyHealth>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();
    }

    private void Start()
    {
        // Refactored to use Player Instance instead of GameObject.FindWithTag
        player = Player.Instance;
        playerTransform = player.transform;
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
        return (playerTransform.position - transform.position).normalized;
    }
    protected float GetDistanceToPlayer()
    {
        return Vector2.Distance(transform.position, playerTransform.position);
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
        if (onPlayer & timer <= 0f)
        {   
            DealDamageToPlayer(collisionDamage);
            timer = collisionDamageInterval;
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
        PickupBehaviour PickupObject = Instantiate(pickupPrefab, transform.position, Quaternion.identity, roomRuntime.transform);
        PickupObject.Initialize(RandomLoot.definition);
    }

    // Common Handling of Enemy Death
    // Might want to separate Destroy() for Visual Animations in future
    // Handles Loot Drop
    public virtual void OnEnemyDeath(BasicEnemy enemy)
    {
        DropLoot();
        Destroy(gameObject);
    }

    // Subscribe to its own EnemyDeath
    protected virtual void OnEnable()
    {
        if (Health == null) Health = GetComponent<EnemyHealth>();
        if (Health != null)
        {
            Health.OnEnemyDied += OnEnemyDeath;
        }
    }

    protected virtual void OnDisable()
    {
        if (Health != null)
        {
            Health.OnEnemyDied -= OnEnemyDeath;
        }
    }

    private void Update()
    {
        DamageLogic();
    }

    private void FixedUpdate()
    {
        WalkLogic();
    }
}