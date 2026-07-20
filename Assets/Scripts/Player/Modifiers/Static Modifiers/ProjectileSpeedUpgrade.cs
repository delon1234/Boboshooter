using UnityEngine;

/// <summary>
/// Multiplies projectile speed by a flat multiplier.
/// </summary>
[CreateAssetMenu(menuName = "Upgrades/Static/Projectile Speed Upgrade")]
public class ProjectileSpeedUpgrade : ScriptableObject, IStaticModifier
{
    [Tooltip("Multiplier applied to projectile speed (e.g. 1.3 = +30% speed).")]
    [Min(0f)]
    public float speedMultiplier = 1.3f;

    public WeaponStats ModifyStaticStats(WeaponStats stats)
    {
        stats.projectileSpeed *= speedMultiplier;
        return stats;
    }
}
