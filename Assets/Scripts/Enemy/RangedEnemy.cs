using Unity.VisualScripting;
using UnityEngine;

public class RangedEnemy : BasicEnemy
{
    [Header("Stats")]
    private float speed = 2.5f;

    private void Awake()
    {
        // "Constructor"
        maxHealth = 100;
        collisionDamage = 10;
    }
    
    // Melee Movement, just follow player
    protected override void WalkLogic()
    {
        // Walk to player
        Vector2 direction = GetDirectionToPlayer();
        direction = direction * speed;
        rb.linearVelocity = direction;
    }
}