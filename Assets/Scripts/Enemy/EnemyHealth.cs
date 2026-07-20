using System;
using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable, IHealth, IInvulnerable
{
    /* EnemyHealth is a wrapper around HealthComponent that enables enemy-specific logic to be applied when taking damage before HealthComponent applies common damage logic across entities.
     */
    [SerializeField] HealthComponent healthComponent;
    [SerializeField] private bool canBeInvulnerable = true; // True only for Bosses/Elites
    // Event fired to all RoomRuntimes about an instanced enemy death
    public event Action<BasicEnemy> OnEnemyDied;

    // private void Start() {
    //     // Testing
    //     healthComponent.GainInvulnerability(5);
    // }

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
    }

    // Expose properties from HealthComponent without duplicating the state variables (Single source of truth)
    public bool IsInvulnerable => healthComponent.IsInvulnerable;
    public float CurrentHealth => healthComponent.CurrentHealth;
    public float MaxHealth => healthComponent.MaxHealth;

    // Testing for invulnerable enemies
    public event Action<bool> OnInvulnerabilityChanged
    {
        add => healthComponent.OnInvulnerabilityChanged += value;
        remove => healthComponent.OnInvulnerabilityChanged -= value;
    }

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
        if (!canBeInvulnerable) return; // Normal enemies dont gain invuln
        healthComponent.GainInvulnerability(invulnerabilityDuration);
    }

    public void Heal(float amount)
    {
        // Generic heal method for enemy
        healthComponent.RestoreHealth(amount);
    }

    public void HealFully()
    {
        healthComponent.RestoreHealth(MaxHealth - CurrentHealth);
    }

    public void UpgradeMaxHealth(float amount)
    {
        // Upgrades to health are "full" - applies flat heal
        healthComponent.IncreaseMaxHealth(amount, false);
        Heal(amount);
    }

    /* Tests */
    [ContextMenu("Test Heal 10 HP")]
    private void TestHeal()
    {
        Heal(10f);
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
