using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    /* PlayerHealth is a wrapper around HealthComponent that enables player-specific logic to be applied when taking damage before HealthComponent applies common damage logic across entities.
     */

    [SerializeField] HealthComponent healthComponent;

    /* Forward Events Sub/Unsub to private HealthComponent */

    public event Action<float, float> OnHealthChange
    {
        add => healthComponent.OnHealthChange += value;
        remove => healthComponent.OnHealthChange -= value;
    }

    // On taking damage; Camera Shake, OnHitSound
    public event Action<DamageInfo> OnTakingDamage
    {
        add => healthComponent.OnTakingDamage += value;
        remove => healthComponent.OnTakingDamage -= value;
    }
    // On death; Death Animation, Game Over Screen
    public event Action OnDeath
    {
        add => healthComponent.OnDeath += value;
        remove => healthComponent.OnDeath -= value;
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        // Player-specific logic for damage dealt (E.g. damage reduction/i-frame for dash)
        // If dashing, ignore damage

        // Else forward to HealthComponent to apply damage
        healthComponent.ApplyDamage(damageInfo);
    }
}