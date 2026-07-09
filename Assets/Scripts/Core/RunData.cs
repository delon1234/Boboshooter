// stores information of Current Run
using System;

public static class RunData
{
    // Floor Data
    public static int CurrentFloor = 1;
    public static int FinalFloor = 5;
    public static RunResult Result;

    // "Dynamic" Run Data
    public static int Coins = 0;

    // Events
    public static event Action<OnCoinsChangedArgs> OnCoinsChanged;

    public static void StartNewRun()
    {
        CurrentFloor = 1;
        Coins = 0;
        OnCoinsChanged?.Invoke(new OnCoinsChangedArgs(0, 0));
    }

    public static void AdvanceFloor()
    {
        CurrentFloor++;
    }

    public static void AddCoins(int amt)
    {
        Coins += amt;
        OnCoinsChanged?.Invoke(new OnCoinsChangedArgs(amt, Coins));
    }

    public static bool SpendCoins(int amt)
    {
        if (Coins < amt)
        {
            return false;
        }
        Coins -= amt;
        OnCoinsChanged?.Invoke(new OnCoinsChangedArgs(-amt, Coins));
        return true;
    }
}