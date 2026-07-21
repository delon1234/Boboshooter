using System;
using UnityEngine;

// will define Permanent Upgrade Information via a ScriptableObject so that it can be configured through Unity Inspector under Assets/Data/PermanentUpgrade
// this is essentially a struct containing relevant information around one upgrade, and PermanentUpgradeState will link upgrades to player achieved level
[CreateAssetMenu(menuName = "Permanent Upgrade")]
public class PermanentUpgradeDefinition : ScriptableObject
{
    public PermanentUpgradeType Type;
    public Sprite Icon;
    public string DisplayName;
    public string DisplayDescription;

    public int[] LevelCosts;
    public int MaxLevel => LevelCosts.Length; // Derive MaxLevel from LevelCosts length

    public float[] LevelEffectValues; // Fine-tuned level effects

    // Helper methods, query information based on given CurrentLevel
    public bool IsMaxLevel(int level)
    {
        return level >= MaxLevel;
    }

    public float GetCurrentEffectValue(int level)
    {
        if (level <= 0)
        {
            return 0;
        }
        return LevelEffectValues[level - 1];
    }

    public float GetNextEffectValue(int level)
    {
        return GetCurrentEffectValue(level + 1);
    }

    public int GetUpgradeCostToNext(int level)
    {
        if (level >= MaxLevel)
        {
            return 0;
        }
        return LevelCosts[level];
    }

    public int GetRefundToPrevious(int level)
    {
        if (level <= 0)
        {
            return 0;
        }
        return LevelCosts[level - 1];
    }

    public String GetFormattedStatText(int level)
    {
        if (IsMaxLevel(level))
        {
            return $"Current: +{GetCurrentEffectValue(level)} > MAX";
        }
        else if (level == 0)
        {
            return $"Current: +0 > +{GetNextEffectValue(level)}";
        }
        else
        {
            return $"Current: +{GetCurrentEffectValue(level)} > +{GetNextEffectValue(level)}";
        }
    }
}