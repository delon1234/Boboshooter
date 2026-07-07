using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    /* PlayerHealth is a wrapper around HealthComponent that enables player-specific logic to be applied when taking damage before HealthComponent applies common damage logic across entities.
     */
    [SerializeField] HealthComponent healthComponent;
    [SerializeField] public float onHitInvulnDuration = 1f;

    // Expose properties from HealthComponent without duplicating the state variables (Single source of truth)
    public bool IsInvulnerable => healthComponent.IsInvulnerable;
    public float CurrentHealth => healthComponent.CurrentHealth;
    public float MaxHealth => healthComponent.MaxHealth;

    /* Forward Events Sub/Unsub to private HealthComponent */

    public event Action<HealthInfo> OnHealthChange
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
        healthComponent.ApplyDamage(damageInfo);
        healthComponent.GainInvulnerability(onHitInvulnDuration);
    }

    public void GainInvulnerability(float invulnerabilityDuration)
    {
        healthComponent.GainInvulnerability(invulnerabilityDuration);
    }

    public void Heal(float amount)
    {
        // Generic heal method for player
        healthComponent.RestoreHealth(amount);
    }

    public void HealFully() // For Level Ascension
    {
        healthComponent.RestoreHealth(MaxHealth - CurrentHealth);
    }

    public void UpgradeMaxHealth(float amount)
    {
        // Upgrades to health are full hearts
        healthComponent.IncreaseMaxHealth(amount, false);
        Heal(amount);
    }

    /* Tests */
    [ContextMenu("Test Heal 2 HP")]
    private void TestHeal()
    {
        Heal(2f);
    }

    [ContextMenu("Test Heal Full")]
    private void TestHealFully()
    {
        HealFully();
    }

    [ContextMenu("Test Upgrade Max HP by 1")]
    private void TestUpgrade()
    {
        UpgradeMaxHealth(1f);
    }

}