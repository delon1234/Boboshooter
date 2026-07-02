using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class HealthComponent : MonoBehaviour
{
    /* HealthComponent attached to GameObject is an abstraction that handles common health functionality for all entities, 
     * such as taking damage, dying, and notifying other components about health changes.
     * Entities can subscribe different events to enable entity-specific behaviours to health changes
     */
    [Header("Health Settings")]
    [field: SerializeField]
    public float MaxHealth { get; private set; } = 5f; 
    public float CurrentHealth { get; private set; } // Float for max flexibility in damage calculations
    // Invincibility property to allow for temporary invincibility
    // 1. Player Dash, 2. Enemy Invincibility Frames, 3. After taking damage (small time window)
    // Public setter to allow other components to enable/disable invulnerability
    [SerializeField] public bool testInvulnerable = true;
    [SerializeField] public float invulnerableDuration = 5.0f;
    public bool IsInvulnerable { get; set; } = false;


    /* Events to notify when health changes, allowing other components to react accordingly */
    // Health Bar UI
    public event Action<float, float> OnHealthChange; // (health, MaxHealth)
    // On taking damage; OnHitSound from various sources, status effects from enemies
    public event Action<DamageInfo> OnTakingDamage;
    // On death; Death Animation, Game Over Screen
    public event Action OnDeath;
    // Dual-purpose event; Gaining/Losing Invulnerability;
    public event Action<bool> OnInvulnerabilityChanged;

    private Coroutine invulnerabilityCoroutine;
    private float invulnerabilityEndTime;

    private void Awake() // Unity's Awake method is called when the script instance is being loaded
    {
        CurrentHealth = MaxHealth; // Initialize health to MaxHealth at the start
    }

    private void Start()
    {
        // Notify subscribers about the initial health state
        OnHealthChange?.Invoke(CurrentHealth, MaxHealth);
        // Testing;
        if (testInvulnerable)
        {
            GainInvulnerability(invulnerableDuration);
            print($"Invulnerable for {invulnerableDuration}");
        }
    }

    public void ApplyDamage(DamageInfo damageInfo)
    {
        if (IsInvulnerable) {
            // Add UI feedback for (Hitting invulnerable player animation)
            print($"Invincible");
            return; 
        }
        // 1. Reduce health by the damage amount (instead of clamping to 1 to enable future changes)
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
        OnHealthChange?.Invoke(CurrentHealth, MaxHealth);
        print($"OnHealthChange: Current HP: {CurrentHealth}, Max HP: {MaxHealth}");
    }

    public void GainInvulnerability(float duration)
    {
        // Player can gain invulnerability from 1. Dashing, 2. Taking a hit (i-frame)
        // Ensure longer duration takes priority / dash (shorter) do not override onHit invuln window
        float endTime = Time.time + duration;
        if (endTime <= invulnerabilityEndTime) return;
        invulnerabilityEndTime = endTime;
        // If was previously invincible, stop the old timer and use latest timer
        if (invulnerabilityCoroutine != null)
        {
            StopCoroutine(invulnerabilityCoroutine);
            print($"stopped previous invulnerability coroutine: {invulnerabilityCoroutine}");
        }
        // Start the new timer
        invulnerabilityCoroutine = StartCoroutine(InvulnerabilityRoutine(duration));
    }
    private IEnumerator InvulnerabilityRoutine(float duration)
    {
        if (!IsInvulnerable) {
            // Prevents double triggering of events
            IsInvulnerable = true;
            OnInvulnerabilityChanged?.Invoke(true); // Trigger UI for invulnerability
            print($"Invulnerability lasts from {Time.time} to {Time.time + duration}");
        }
        yield return new WaitForSeconds(duration); // Pause and return control to Unity
        IsInvulnerable = false;
        OnInvulnerabilityChanged?.Invoke(false); // Trigger UI to return to normal
        invulnerabilityCoroutine = null;
        print("Invulnerability ended");
    }

    public void RestoreHealth(float amount) {
        if (amount <= 0) { return; }
        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
        // Alternative approach - ensures CurrentHealth never goes negative
        // CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0f, MaxHealth);
        OnHealthChange?.Invoke(CurrentHealth, MaxHealth);
    }

    public void IncreaseMaxHealth (float amount, bool healProportionally)
    {
        if (amount <= 0) { return; }
        float oldMaxHealth = MaxHealth;
        MaxHealth += amount;
        if (healProportionally)
        {
            if (oldMaxHealth > 0)
            {
                // Set current health to old proportion
                CurrentHealth = (CurrentHealth / oldMaxHealth) * MaxHealth; 
            }
            else
            {
                CurrentHealth = MaxHealth; // Prevents division by 0
            }
        }
        OnHealthChange?.Invoke(CurrentHealth, MaxHealth);
    }
}
