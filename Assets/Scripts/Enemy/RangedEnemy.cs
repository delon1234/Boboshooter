using Unity.VisualScripting;
using UnityEngine;

public class RangedEnemy : BasicEnemy
{
    // Distance to shoot from
    private float SHOOTING_DISTANCE = 5f;
    private float TOLERANCE = 0.5f;



    // Range Movement
    // Walk towards player if far, away if close
    protected override void WalkLogic()
    {
        Vector2 direction = GetDirectionToPlayer();  
        if (GetDistanceToPlayer() < SHOOTING_DISTANCE - TOLERANCE) {     
            direction = direction * -1;        
        } else if (GetDistanceToPlayer() > SHOOTING_DISTANCE + TOLERANCE)
        {
            direction = direction * speed;
        } else
        {
            direction = Vector2.zero;
        }
        rb.linearVelocity = direction;
    }
}