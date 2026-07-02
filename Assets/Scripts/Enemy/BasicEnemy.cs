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

    private void DealDamageToPlayer(float amt)
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

    private void Update()
    {
        DamageLogic();
    }

    private void FixedUpdate()
    {
        WalkLogic();
    }
}