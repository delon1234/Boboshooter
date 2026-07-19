// Stores Persistent Data that will be used for each run
// And Defines how each of these stats will impact each run
public static class MetaData
{
    public static int MetaCurrency = 0;

    // Takes a SaveData struct and initialises values
    public static void LoadFromSave(SaveData loadedSave)
    {
        MetaCurrency = loadedSave.MetaCurrency;
    }

    // Packages current data into SaveData and returns it for saving
    public static SaveData GetSaveData()
    {
        return new SaveData(MetaCurrency);
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
}