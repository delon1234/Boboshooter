using NUnit.Framework;
using UnityEngine;

// Focus on Testing RunData Logic
public class RunDataTests
{
    // Will Start each Run from a New Run logically
    [SetUp]
    public void SetUp()
    {
        RunData.StartNewRun();
    }

    // Nothing needs to be done after each run (as of now)
    // Because RunData is a static class. It constantly references the same Data
    [TearDown]
    public void TearDown()
    {
    }

    // Tests whether initial values for StartNewRun is correct
    [Test]
    public void StartNewRun_InitialState()
    {
        Assert.AreEqual(0, RunData.Coins);
        Assert.AreEqual(1, RunData.CurrentFloor);
    }

    // Tests whether StartNewRun correctly resets Floor and Coins to default values
    [Test]
    public void StartNewRun_ResetsState()
    {
        RunData.AddCoins(10);
        RunData.AdvanceFloor();
        RunData.StartNewRun();

        Assert.AreEqual(1, RunData.CurrentFloor);
        Assert.AreEqual(0, RunData.Coins);
    }

    // Tests whether AdvanceFloor correctly increments CurrentFloor by 1
    [Test]
    public void AdvanceFloor_IncrementsByOne()
    {
        RunData.AdvanceFloor();
        Assert.AreEqual(2, RunData.CurrentFloor);
    }

    // Tests whether AdvanceFloor does NOT reset Floor and Coins (only StartNewRun should do that)
    [Test]
    public void AdvanceFloor_DoesNotResetsState()
    {
        RunData.AddCoins(10);
        RunData.AdvanceFloor();

        Assert.AreEqual(2, RunData.CurrentFloor);
        Assert.AreEqual(10, RunData.Coins);
    }

    // Test whether Coins are deducted when insufficient
    [Test]
    public void SpendCoins_InsufficientCoins()
    {
        RunData.AddCoins(1);
        bool result = RunData.SpendCoins(2);

        Assert.IsFalse(result);
        Assert.AreEqual(1, RunData.Coins);
    }

    // Test whether Coins are deducted when sufficient
    [Test]
    public void SpendCoins_SufficientCoins()
    {
        RunData.AddCoins(2);
        bool result = RunData.SpendCoins(2);

        Assert.IsTrue(result);
        Assert.AreEqual(0, RunData.Coins);
    }
}