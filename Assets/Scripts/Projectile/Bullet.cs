using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Will add Object Pooling later for better performance, but for now we will just instantiate and destroy bullets
    // Will change to ScriptableObject later for more flexible bullet types
    [SerializeField] private float speed = 10f;
    [SerializeField] private float damage = 1f;

    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed; // Move the bullet in the direction it's facing (right)
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
        Destroy(gameObject); // Destroy the bullet after it hits something
    }

}
