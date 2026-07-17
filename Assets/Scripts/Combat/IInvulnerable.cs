using System;

public interface IInvulnerable
{
    bool IsInvulnerable { get; }
    event Action<bool> OnInvulnerabilityChanged;
    void GainInvulnerability(float duration);
}
