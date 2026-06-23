using Unity.VisualScripting;
using UnityEngine;

public class MeleeEnemy : BasicEnemy
{
    private float TOLERANCE = 0.3f;
    private void Awake()
    {
        // "Constructor"
        maxHealth = 100;
        collisionDamage = 1f;
        speed = 4f;
    }

    // Melee Movement, just follow player
    protected override void WalkLogic()
    {
        // Walk to player
        if (GetDistanceToPlayer() > TOLERANCE)
        {
            Vector2 direction = GetDirectionToPlayer();
            direction = direction * speed;
            rb.linearVelocity = direction;
        } else
        {
            rb.linearVelocity = Vector2.zero;
        }

    }
}