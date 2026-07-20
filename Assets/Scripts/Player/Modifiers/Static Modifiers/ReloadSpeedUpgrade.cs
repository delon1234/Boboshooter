using UnityEngine;

/// <summary>
/// Multiplies reload time by a flat multiplier. Values below 1.0 speed up reloading.
/// </summary>
[CreateAssetMenu(menuName = "Upgrades/Static/Reload Speed Upgrade")]
public class ReloadSpeedUpgrade : ScriptableObject, IStaticModifier
{
    [Tooltip("Multiplier applied to reload time (e.g. 0.75 = 25% faster reload).")]
    [Min(0.01f)]
    public float reloadTimeMultiplier = 0.75f;

    public WeaponStats ModifyStaticStats(WeaponStats stats)
    {
        stats.reloadTime *= reloadTimeMultiplier;
        return stats;
    }
}
