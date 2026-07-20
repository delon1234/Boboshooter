using UnityEngine;

/// <summary>
/// Multiplies bullet damage by a flat multiplier.
/// </summary>
[CreateAssetMenu(menuName = "Upgrades/Static/Damage Upgrade")]
public class DamageUpgrade : ScriptableObject, IStaticModifier
{
    [Tooltip("Multiplier applied to base damage (e.g. 1.25 = +25% damage).")]
    [Min(0f)]
    public float damageMultiplier = 1.25f;

    public WeaponStats ModifyStaticStats(WeaponStats stats)
    {
        stats.damage *= damageMultiplier;
        return stats;
    }
}
