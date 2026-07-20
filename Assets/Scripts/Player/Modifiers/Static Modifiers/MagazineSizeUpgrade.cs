using UnityEngine;

/// <summary>
/// Increases magazine size by a flat amount.
/// </summary>
[CreateAssetMenu(menuName = "Upgrades/Static/Magazine Size Upgrade")]
public class MagazineSizeUpgrade : ScriptableObject, IStaticModifier
{
    [Tooltip("Flat number of extra bullets added to the magazine.")]
    [Min(0)]
    public int extraBullets = 5;

    public WeaponStats ModifyStaticStats(WeaponStats stats)
    {
        stats.magazineSize += extraBullets;
        return stats;
    }
}
