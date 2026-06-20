using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    /* HealthComponent attached to GameObject is an abstraction that handles common health functionality for all entities, 
     * such as taking damage, dying, and notifying other components about health changes.
     * Entities can subscribe different events to enable entity-specific behaviours to health changes
     */

    [SerializeField] private int maxHealth;
    public float CurrentHealth { get; private set; } // Float for max flexibility in damage calculations
    // Invincibility property to allow for temporary invincibility
    // 1. Player Dash, 2. Enemy Invincibility Frames, 3. After taking damage (small time window)
    // Public setter to allow other components to enable/disable invincibility
    public bool IsInvincible { get; set; } = false;


    /* Events to notify when health changes, allowing other components to react accordingly */
    // Health Bar UI
    public event Action<float, float> OnHealthChange; // (health, maxHealth)
    // On taking damage; Camera Shake, OnHitSound
    public event Action<DamageInfo> OnTakingDamage;
    // On death; Death Animation, Game Over Screen
    public event Action OnDeath;
    // On Gain Invulnerability; Invulnerability Animation
    public event Action<bool> OnInvulnerabilityChanged;

    private void Awake() // Unity's Awake method is called when the script instance is being loaded
    {
        CurrentHealth = maxHealth; // Initialize health to maxHealth at the start
    }

    public void ApplyDamage(DamageInfo damageInfo)
    {
        if (IsInvincible) {
            // Add UI feedback for invincibility (Player invulnerability animation)
            return; 
        } 
        // 1. Reduce health by the damage amount
        CurrentHealth -= damageInfo.Amount;
        // 2. Invoke the OnTakingDamage event to notify subscribers about the damage taken
        OnTakingDamage?.Invoke(damageInfo);
        print($"OnTakingDamage {damageInfo}");
        // 3. Handle Death
        if (CurrentHealth <= 0) // Check if health has dropped to zero or below; good for float comparisons
        {
            CurrentHealth = 0; // Ensure health doesn't go below zero
            OnDeath?.Invoke(); // Invoke the OnDeath event
            print("Died"); // Debug log for death;  
        }
        // 4. Notify subscribers about the health change, passing current health and max health
        OnHealthChange?.Invoke(CurrentHealth, maxHealth);
        print($"OnHealthChange: Current HP: {CurrentHealth}, Max HP: {maxHealth}");
    }
}
