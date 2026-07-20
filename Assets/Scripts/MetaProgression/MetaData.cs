// Stores Persistent Data that will be used for each run
// And Defines how each of these stats will impact each run
using System.Collections.Generic;

public static class MetaData
{
    public static int MetaCurrency = 0;
    private static Dictionary<PermanentUpgradeType, int> UpgradeLevels = new();

    // Takes a SaveData struct and initialises values
    public static void LoadFromSave(SaveData loadedSave)
    {
        MetaCurrency = loadedSave.MetaCurrency;
        UpgradeLevels = loadedSave.PermanentUpgrades;
    }

    // Packages current data into SaveData and returns it for saving
    public static SaveData GetSaveData()
    {
        return new SaveData(MetaCurrency, UpgradeLevels);
    }

    public static void AddCurrency(int amt)
    {
        MetaCurrency += amt;
    }

    public static bool SpendCurrency(int amt)
    {
        if (MetaCurrency < amt)
        {
            return false;
        }
        MetaCurrency -= amt;
        return true;
    }

    public static int GetUpgradeLevel(PermanentUpgradeType type)
    {
        return UpgradeLevels.GetValueOrDefault(type, 0);
    }

    public static void SetUpgradeLevel(PermanentUpgradeType type, int level)
    {
        UpgradeLevels[type] = level;
    }
}