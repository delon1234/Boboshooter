// Stores Persistent Data that will be used for each run
// And Defines how each of these stats will impact each run
using System;
using System.Collections.Generic;
using UnityEngine;

public static class MetaData
{
    public static int MetaCoins = 0;
    private static Dictionary<PermanentUpgradeType, int> UpgradeLevels = new();

    // Events, mainly for UI
    public static event Action<OnMetaCoinsChangedArgs> OnMetaCoinsChanged;
    public static event Action<PermanentUpgradeDefinition> OnPermanentUpgradesChanged;


    // Takes a SaveData struct and initialises values
    public static void LoadFromSave(SaveData loadedSave)
    {
        MetaCoins = loadedSave.MetaCoins;
        UpgradeLevels = loadedSave.PermanentUpgrades;
    }

    // Packages current data into SaveData and returns it for saving
    public static SaveData GetSaveData()
    {
        return new SaveData(MetaCoins, UpgradeLevels);
    }

    public static void AddCurrency(int amt)
    {
        MetaCoins += amt;
        OnMetaCoinsChanged?.Invoke(new OnMetaCoinsChangedArgs(amt, MetaCoins));
    }

    public static bool SpendCurrency(int amt)
    {
        if (MetaCoins < amt)
        {
            return false;
        }
        MetaCoins -= amt;
        OnMetaCoinsChanged?.Invoke(new OnMetaCoinsChangedArgs(amt, MetaCoins));
        return true;
    }

    // Simple Helper, Dictionary reader
    // Helps to return sane values
    public static int GetUpgradeLevel(PermanentUpgradeDefinition definition)
    {
        int currentLevel = UpgradeLevels.GetValueOrDefault(definition.Type, 0);
        return Mathf.Clamp(currentLevel, 0, definition.MaxLevel);
    }

    public static bool IsMaxLevel(PermanentUpgradeDefinition definition)
    {
        return GetUpgradeLevel(definition) >= definition.MaxLevel;
    }

    public static bool IsMinLevel(PermanentUpgradeDefinition definition)
    {
        return GetUpgradeLevel(definition) <= 0;
    }

    public static bool Upgrade(PermanentUpgradeDefinition definition)
    {
        // Level Checks
        int currentLevel = GetUpgradeLevel(definition);
        if (currentLevel >= definition.MaxLevel)
        {
            return false;
        }

        // Cost Checks
        int cost = definition.LevelCosts[currentLevel];
        if (!SpendCurrency(cost))
        {
            return false;
        }

        // Runtime Saves
        UpgradeLevels[definition.Type] = currentLevel + 1;
        OnPermanentUpgradesChanged?.Invoke(definition);
        return true;
    }

    public static bool Downgrade(PermanentUpgradeDefinition definition)
    {
        // Level Checks
        int currentLevel = GetUpgradeLevel(definition);
        if (currentLevel <= 0)
        {
            return false;
        }

        // Cost Checks
        int refund = definition.LevelCosts[currentLevel - 1];
        AddCurrency(refund);

        // Runtime Saves
        UpgradeLevels[definition.Type] = currentLevel - 1;
        OnPermanentUpgradesChanged?.Invoke(definition);
        return true;
    }
}