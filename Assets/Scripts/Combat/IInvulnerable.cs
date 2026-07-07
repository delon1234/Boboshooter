public interface IInvulnerable
{
    bool IsInvulnerable { get; }
    void GainInvulnerability(float duration);
}
