using System;
using System.Collections.Generic;

// "Packages" what things will be saved to the JSON by SaveSystem.cs
// Used by MetaData to store and load
// It needs to be [Serializable] and NOT readonly for JSONUtility to properly JSONify this structure
[Serializable]
public struct SaveData
{
    public int MetaCoins;
    public Dictionary<PermanentUpgradeType, int> PermanentUpgrades;

    public SaveData(int MetaCoins, Dictionary<PermanentUpgradeType, int> PermanentUpgrades)
    {
        this.MetaCoins = MetaCoins;
        this.PermanentUpgrades = PermanentUpgrades;
    }
}