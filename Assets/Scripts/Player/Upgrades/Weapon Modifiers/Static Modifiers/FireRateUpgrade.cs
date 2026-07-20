using UnityEngine;

/// <summary>
/// Multiplies the weapon's fire rate by a flat multiplier.
/// </summary>
[CreateAssetMenu(menuName = "Upgrades/Static/Fire Rate Upgrade")]
public class FireRateUpgrade : ScriptableObject, IStaticModifier
{
    [Tooltip("Multiplier applied to base fire rate (e.g. 1.2 = +20% fire rate).")]
    [Min(0f)]
    public float fireRateMultiplier = 1.2f;

    public WeaponStats ModifyStaticStats(WeaponStats stats)
    {
        stats.fireRate *= fireRateMultiplier;
        return stats;
    }
}
