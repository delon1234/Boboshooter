using System;
using UnityEngine;

// Parent Class for all Enemies
public abstract class BasicEnemy : MonoBehaviour
{
    [Header("Stats")]
    private float currentHealth;
    [SerializeField] protected float maxHealth = 5;
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

    // Event
    // fired to all RoomRuntimes about an instanced enemy death
    public static event Action<BasicEnemy> OnEnemyDied;
    // fired for UI updates
    public event Action<OnEnemyHealthChangedArgs> OnEnemyHealthChanged;

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        // Refactored to use Player Instance instead of GameObject.FindWithTag
        player = Player.Instance;
        playerTransform = player.transform;
    }

    // Finds the script from the Enemy Instance
    private void Awake()
    {
        healthBar = GetComponentInChildren<EnemyHealthBar>();
    }

    // Projectiles or damage dealing objects will call this to handle health checks
    public void TakeDamage(float amt)
    {
        currentHealth -= amt;
        OnEnemyHealthChanged?.Invoke(new OnEnemyHealthChangedArgs(amt, currentHealth, maxHealth));
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Handling for Enemy Death
    // For now just Destroy enemy, maybe have animations next time?
    private void Die()
    {
        // Sends out a trigger that this specific enemy died
        OnEnemyDied?.Invoke(this);
        Destroy(gameObject);
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

    private void DealDamageToPlayer(float amt)
    {
        // Prevents redundant calls to TakeDamage and creating too many DamageInfo on heap
        if (player.Health.IsInvulnerable) return;
        DamageInfo dmg = new DamageInfo(amt, gameObject, GetDirectionToPlayer());
        player.Health.TakeDamage(dmg);
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

    private void Update()
    {
        DamageLogic();
    }

    private void FixedUpdate()
    {
        WalkLogic();
    }
}