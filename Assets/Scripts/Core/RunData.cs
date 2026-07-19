// stores information of Current Run
using System;
using UnityEngine;

public static class RunData
{
    // Floor Data
    public static int CurrentFloor = 1;
    public const int FinalFloor = 5;
    public static RunResult Result;

    // "Dynamic" Run Data
    public static int Coins = 0;

    // "Statistics" Data
    public static int EnemiesKilled = 0;

    public static int EarnedCurrency = 0;

    // Events
    public static event Action<OnCoinsChangedArgs> OnCoinsChanged;

    public static void StartNewRun()
    {
        CurrentFloor = 1;
        Coins = 0;
        EnemiesKilled = 0;
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

    public static void IncrementEnemyKilled(int amt)
    {
        EnemiesKilled += amt;
    }

    // Determines the reward given using current statistics
    public static void UpdateEarnedCurrency()
    {
        int floorsCompleted = CurrentFloor - 1;
        if (Result == RunResult.Victory)
        {
            floorsCompleted += 1;
        }
        int reward = 0;

        reward += floorsCompleted * 50;
        reward += EnemiesKilled;
        EarnedCurrency = reward;
    }
}