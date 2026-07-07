// stores information of Current Run
public static class RunData
{
    // Floor Data
    public static int CurrentFloor = 1;
    public static int FinalFloor = 5;
    public static RunResult Result;

    // "Dynamic" Run Data
    public static int Coins = 0;

    public static void StartNewRun()
    {
        CurrentFloor = 1;
        Coins = 0;
    }

    public static void AdvanceFloor()
    {
        CurrentFloor++;
    }

    public static void AddCoins(int amt)
    {
        Coins += amt;
    }
}