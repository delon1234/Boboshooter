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

    // Will change Bullet to be universal (Player -> Enemy, Enemy -> Player) later, but for now it only damages enemies
    private void OnTriggerEnter2D(Collider2D collision)
    {   
        BasicEnemy enemy = collision.GetComponent<BasicEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Debug.Log($"Enemy: {enemy.name} takes {damage} damage");
        }
        Destroy(gameObject); // Destroy the bullet after it hits something
    }

}
