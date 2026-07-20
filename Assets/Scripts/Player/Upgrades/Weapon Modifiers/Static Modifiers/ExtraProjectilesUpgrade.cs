using UnityEngine;

/// <summary>
/// Adds extra projectiles per shot (feeds into BulletPattern.projectileCount).
/// </summary>
[CreateAssetMenu(menuName = "Upgrades/Static/Extra Projectiles Upgrade")]
public class ExtraProjectilesUpgrade : ScriptableObject, IStaticModifier
{
    [Tooltip("Flat number of additional projectiles fired per shot.")]
    [Min(0)]
    public int extraProjectiles = 1;

    public WeaponStats ModifyStaticStats(WeaponStats stats)
    {
        stats.projectileCount += extraProjectiles;
        return stats;
    }
}
