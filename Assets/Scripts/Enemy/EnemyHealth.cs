using System;
using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    /* EnemyHealth is a wrapper around HealthComponent that enables enemy-specific logic to be applied when taking damage before HealthComponent applies common damage logic across entities.
     */
    [SerializeField] HealthComponent healthComponent;
    // Event fired to all RoomRuntimes about an instanced enemy death
    public static event Action<BasicEnemy> OnEnemyDied;

    private void OnEnable()
    {
        healthComponent.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        healthComponent.OnDeath -= HandleDeath;
    }

    private void HandleDeath()
    {
        // healthComponent.OnDeath triggers OnEnemyDied event
        OnEnemyDied?.Invoke(GetComponent<BasicEnemy>());
        Destroy(gameObject);
    }

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
        // Enemy-specific logic for damage dealt
        // 1. Get IDamageModifer components
        // 2. Apply pipeline
        // 3. Forward damage to healthComponent
        if (healthComponent.IsInvulnerable) return;
        healthComponent.ApplyDamage(damageInfo);
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
        // Upgrades to health are "full" - applies flat heal
        Heal(amount);
        healthComponent.IncreaseMaxHealth(amount, false);
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

    [ContextMenu("Test Upgrade Max HP by 20")]
    private void TestUpgrade()
    {
        UpgradeMaxHealth(20f);
    }
}
