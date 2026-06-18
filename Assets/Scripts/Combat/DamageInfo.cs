using System;
using UnityEngine;

public readonly struct DamageInfo
{
    /* DamageInfo is a struct to pass damage-related information to entities 
     - Utilises Parameter Object Pattern to ensure scalability and maintainability as more properties can be added without changing method signatures
     - Readonly struct for immutability
     */

    /* Properties */
    // 1. Readonly properties for immutability
    public float Amount { get; }
    public GameObject Attacker { get; }
    public Vector2 HitDirection { get; }   // Direction of the attack (useful for knockback for shotgun)

    public DamageInfo(float amount, GameObject attacker, Vector2 hitDirection)
    {
        Amount = amount;
        Attacker = attacker;
        HitDirection = hitDirection;
    }

}

