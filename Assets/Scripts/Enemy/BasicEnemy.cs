using UnityEngine;

// Parent Class for all Enemies
public class BasicEnemy : MonoBehaviour
{
    [Header("Stats")]
    protected float maxHealth;
    private float currentHealth;
    protected float collisionDamage;

    // Player Collision Damage Logic
    private bool onPlayer = false;
    private float timer;
    private float damageInterval = 0.5f;

    protected Rigidbody2D rb;
    protected Transform player;
    protected PlayerHealth playerHealth;

    private void Awake()
    {
        
    }

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
    // For now just Destroy enemey, maybe have animations next time?
    private void Die()
    {
        Destroy(gameObject);
    }

    // Useful info for Enemy Variations to know direction to player
    protected Vector2 GetDirectionToPlayer()
    {
        return (player.position - transform.position).normalized;
    }

    // Collide with Player, start timer for damaging
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Take Damage 
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

    protected virtual void Update()
    {
        // Damage Logic
        timer -= Time.deltaTime;
        if (onPlayer & timer <= 0f)
        {   
            DealDamageToPlayer(collisionDamage);
            timer = damageInterval;
        }
    }
}