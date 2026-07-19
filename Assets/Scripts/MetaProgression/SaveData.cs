using System;

// "Packages" what things will be saved to the JSON by SaveSystem.cs
// Used by MetaData to store and load
// It needs to be [Serializable] and NOT readonly for JSONUtility to properly JSONify this structure
[Serializable]
public struct SaveData
{
    public int MetaCurrency;

    public SaveData(int MetaCurrency)
    {
        this.MetaCurrency = MetaCurrency;
    }
}