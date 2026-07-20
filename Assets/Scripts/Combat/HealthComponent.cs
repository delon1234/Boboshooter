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

    // Public setter to allow other components to enable/disable invulnerability
    [SerializeField] public bool testInvulnerable = true;
    [SerializeField] public float invulnerableDuration = 5.0f;
        
    // Invincibility property to allow for temporary invincibility
    // 1. Player Dash, 2. Enemy Invincibility Frames, 3. onHit
    #region Invulnerability variables
    public bool IsInvulnerable { get; set; } = false;
    private Coroutine invulnerabilityCoroutine;
    private float invulnerabilityEndTime;
    #endregion

    /* Events to notify when health changes, allowing other components to react accordingly */
    #region Events for other components
    // Health Bar UI
    public event Action<HealthInfo> OnHealthChange;
    // On taking damage; OnHitSound from various sources, status effects from enemies
    public event Action<DamageInfo> OnTakingDamage;
    // On death; Death Animation, Game Over Screen
    public event Action OnDeath;
    // Dual-purpose event; Gaining/Losing Invulnerability;
    public event Action<bool> OnInvulnerabilityChanged;
    #endregion

    private HealthInfo CurrentHealthInfo => new HealthInfo(CurrentHealth, MaxHealth);

    private void NotifyHealthChanged() // Helper method to inform health changes
    {
        OnHealthChange?.Invoke(CurrentHealthInfo);
    }

    private void Awake() // Unity's Awake method is called when the script instance is being loaded
    {
        CurrentHealth = MaxHealth;
    }

    private void Start()
    {
        // Notify subscribers about the initial health state
        NotifyHealthChanged();
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
            return; 
        }
        // 1. Reduce health by the damage amount (Clamping to 1 can be done in PlayerHealth)
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
        NotifyHealthChanged();
        print($"OnHealthChange: Current HP: {CurrentHealth}, Max HP: {MaxHealth}");
    }

    public void GainInvulnerability(float duration)
    {
        // Ensure invuln duration prioritizes longest duration in event of multiple invulnerability gains 
        // E.g. dash (shorter, more recent) do not override onHit invuln window
        float endTime = Time.time + duration;
        if (endTime <= invulnerabilityEndTime) return;
        invulnerabilityEndTime = endTime;
        // If was previously invincible, stop the old timer and use latest timer
        if (invulnerabilityCoroutine != null)
        {
            StopCoroutine(invulnerabilityCoroutine);
            print($"stopped previous invulnerability coroutine: {invulnerabilityCoroutine}");
        }
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
        NotifyHealthChanged();
    }

    public void IncreaseMaxHealth (float amount, bool healProportionally)
    {
        if (amount <= 0) { return; }
        float oldMaxHealth = MaxHealth;
        MaxHealth += amount;
        if (healProportionally) // Maintains health % while increasing max health
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
        NotifyHealthChanged();
    }
}
