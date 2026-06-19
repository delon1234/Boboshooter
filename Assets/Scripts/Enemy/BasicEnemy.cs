using System;
using UnityEngine;

// Parent Class for all Enemies
public abstract class BasicEnemy : MonoBehaviour
{
    [Header("Stats")]
    protected float maxHealth = 5;
    private float currentHealth;
    protected float collisionDamage = 5;
    protected float speed = 3f;

    // Player Collision Damage Logic
    private bool onPlayer = false;
    private float timer;
    private float damageInterval = 0.5f;

    protected Rigidbody2D rb;
    protected Transform player;
    protected PlayerHealth playerHealth;

    // Event, fired to all RoomRuntimes about an instanced enemy death
    public static event Action<BasicEnemy> OnEnemyDied;

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    // Projectiles or damage dealing objects will call this to handle health checks
    public void TakeDamage(float amt)
    {
        currentHealth -= amt;
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
        return (player.position - transform.position).normalized;
    }
    protected float GetDistanceToPlayer()
    {
        return Vector2.Distance(transform.position, player.position);
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
        playerHealth.TakeDamage(amt);
    }

    // Damage Logic
    private void DamageLogic()
    {
        timer -= Time.deltaTime;
        if (onPlayer & timer <= 0f)
        {   
            DealDamageToPlayer(collisionDamage);
            timer = damageInterval;
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