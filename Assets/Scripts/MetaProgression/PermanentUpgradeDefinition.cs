using System;
using UnityEngine;

// will define Permanent Upgrade Information via a ScriptableObject so that it can be configured through Unity Inspector under Assets/Data/PermanentUpgrade
// this is essentially a struct containing relevant information around one upgrade, and PermanentUpgradeState will link upgrades to player achieved level
[CreateAssetMenu(menuName = "Permanent Upgrade")]
public class PermanentUpgradeDefinition : ScriptableObject
{
    // Fine tuning UI to decide whether to show % value or not
    public enum UpgradeValueFormat
    {
        Flat,
        Percentage
    }

    public PermanentUpgradeType Type;
    public Sprite Icon;
    public string DisplayName;
    public string DisplayDescription;

    public int[] LevelCosts;
    public int MaxLevel => LevelCosts.Length; // Derive MaxLevel from LevelCosts length

    public float[] LevelEffectValues; // Fine-tuned level effects

    [SerializeField] private UpgradeValueFormat valueFormat;


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

    // Returns a readable UI for a value based on its valueFormat
    public string GetFormattedEffect(float value)
    {
        switch (valueFormat)
        {
            case UpgradeValueFormat.Percentage:
                return $"+{value}%";

            case UpgradeValueFormat.Flat:
            default:
                return $"+{value}";
        }
    }

    public String GetFormattedStatText(int level)
    {
        String CurrentEffectString = GetFormattedEffect( GetCurrentEffectValue(level) );
        
        if (IsMaxLevel(level))
        {
            return $"Current: {CurrentEffectString} > MAX";
        }
        else
        {
            String NextEffectString = GetFormattedEffect( GetNextEffectValue(level) );
            return $"Current: {CurrentEffectString} > {NextEffectString}";
        }
    }
}