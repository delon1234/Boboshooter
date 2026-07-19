// "Packages" what things will be saved to the JSON by SaveSystem.cs
// Used by MetaData to store and load
public struct SaveData
{
    public readonly int MetaCurrency;

    public SaveData(int MetaCurrency)
    {
        this.MetaCurrency = MetaCurrency;
    }
}