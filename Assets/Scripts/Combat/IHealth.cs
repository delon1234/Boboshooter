using System;

public interface IHealth
{
    // Read only properties; no setters available for public
    // Implementing class can have private setters
    float CurrentHealth { get; }
    float MaxHealth { get; }
    event Action<HealthInfo> OnHealthChange;
    event Action OnDeath;
}

