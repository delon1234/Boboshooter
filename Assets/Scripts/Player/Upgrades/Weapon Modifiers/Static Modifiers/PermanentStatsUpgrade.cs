using UnityEngine;

// Applies all the Permanent Upgrades to Stats at the same time
// This is treated as a single source - being the PermanentUpgrade Shop source
// Does not need to be an SO because all the data already comes from MetaDataLookup
public class PermanentStatModifier : IStaticModifier
{
    public WeaponStats ModifyStaticStats(WeaponStats stats)
    {
        stats.damage *= 1f + MetaDataLookup.GetEffectValueByType(PermanentUpgradeType.Damage) / 100;
        stats.fireRate *= 1f + MetaDataLookup.GetEffectValueByType(PermanentUpgradeType.FireRate) / 100;
        return stats;
    }
}