using UnityEngine;

public readonly struct HealthInfo
{
    /* HealthInfo is a struct to pass health-related information to healthbars 
     - Utilises Parameter Object Pattern to ensure scalability and maintainability as more properties can be added without changing method signatures
     - Readonly struct for immutability to prevent side effects when passing to other systems via events
     */

    /* Properties */
    // 1. Readonly properties for immutability
    public float CurrentHealth { get; }
    public float MaxHealth { get; }
    

    public HealthInfo(float currentHealth, float maxHealth)
    {
        CurrentHealth = currentHealth;
        MaxHealth = maxHealth;
    }

    public override string ToString()
    // Debugging purposes; provides a string representation of struct
    {
        return $"HealthInfo: CurrentHealth={CurrentHealth}, MaxHealth={MaxHealth}";
    }
}

