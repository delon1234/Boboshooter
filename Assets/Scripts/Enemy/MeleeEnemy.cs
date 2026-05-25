using Unity.VisualScripting;
using UnityEngine;

public class MeleeEnemy : BasicEnemy
{
    [Header("Stats")]
    private float speed = 4f;

    private void Awake()
    {
        // "Constructor"
        maxHealth = 100;
        collisionDamage = 10;
    }

    // Melee Movement, just follow Player
    protected override void Update()
    {
        // Always handle collision logic
        base.Update();

        // Walk to player
        Vector2 direction = GetDirectionToPlayer();
        direction = direction * speed;
        rb.linearVelocity = direction;
    }
}