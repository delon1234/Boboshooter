using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    /* PlayerHealth is a wrapper around HealthComponent that enables player-specific logic to be applied when taking damage before HealthComponent applies common damage logic across entities.
     */
    [Header("Components")] 
    [SerializeField] HealthComponent healthComponent;
    [SerializeField] public float dashInvulnDuration = 0.2f;
    [SerializeField] public float onHitInvulnDuration = 1f;

    // Expose properties from HealthComponent without duplicating the state variables (Single source of truth)
    public bool IsInvulnerable => healthComponent.IsInvulnerable;
    public float CurrentHealth => healthComponent.CurrentHealth;
    public float MaxHealth => healthComponent.MaxHealth;

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
        if (healthComponent.IsInvulnerable) return; // If dashing/just take damage, return
        // Else forward to HealthComponent to apply damage
        healthComponent.ApplyDamage(damageInfo);
        // Gain temporary invulnerability after taking damage (i-frame)
        healthComponent.GainInvulnerability(onHitInvulnDuration);
    }
}