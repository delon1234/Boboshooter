using UnityEngine;

/// <summary>
/// A collision-only enemy variant that relies solely on contact/collision damage
/// inherited from <see cref="BasicEnemy"/> to damage the player while chasing them directly.
/// </summary>
public class CollisionEnemy : BasicEnemy
{
    /// <summary>
    /// Continuously chases the player at movement speed.
    /// Collision damage, animations, facing direction, and death sequences are automatically handled by BasicEnemy.
    /// </summary>
    protected override void WalkLogic()
    {
        if (isDead || playerTransform == null)
        {
            if (rb != null) rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 moveDirection = GetDirectionToPlayer();
        rb.linearVelocity = moveDirection * speed;
    }
}
