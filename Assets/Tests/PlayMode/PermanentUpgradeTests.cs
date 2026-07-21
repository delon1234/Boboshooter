using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class PermanentUpgradeTests
{
    // Create a new definition for the purpose of Logic Testing
    private PermanentUpgradeDefinition testDefinition;
    private PermanentUpgradeType testType = PermanentUpgradeType.Damage;
    private int InitialCurrency = 50;

    private int LevelOneCost = 10;
    private int LevelTwoCost = 20;
    private int LevelThreeCost = 30;

    private float LevelOneEffect = 6;
    private float LevelTwoEffect = 12;
    private float LevelThreeEffect = 15;

    // Initializes a TestDefinition and Loads a specified SaveData
    [SetUp]
    public void SetUp()
    {
        // Create fake ScriptableObject and "Initialize" it with Logic Values, more targetted towards EffectValues
        testDefinition = ScriptableObject.CreateInstance<PermanentUpgradeDefinition>();
        testDefinition.Type = testType;
        testDefinition.LevelCosts = new int[]
        {
            LevelOneCost,
            LevelTwoCost,
            LevelThreeCost
        };

        testDefinition.LevelEffectValues = new float[]
        {
            LevelOneEffect,
            LevelTwoEffect,
            LevelThreeEffect
        };

        // Initializes a specific Save value
        MetaData.LoadFromSave(new SaveData(InitialCurrency, new Dictionary<PermanentUpgradeType, int>()));

        // For effect values, need to register definitions first
        MetaDataLookup.RegisterDefinitions(new PermanentUpgradeDefinition[]{testDefinition});
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(testDefinition);
    }

    // No further manipulation after Setup, new definitions should start at level 0
    [Test]
    public void NewDefinition_StartsAtLevelZero()
    {
        int level = MetaData.GetUpgradeLevel(testDefinition);
        Assert.AreEqual(0, level);
    }

    // Upgrade properly deducts the right amount of currency
    [Test]
    public void Upgrade_DeductsCurrency()
    {
        MetaData.Upgrade(testDefinition);
        Assert.AreEqual(InitialCurrency - LevelOneCost, MetaData.MetaCoins);
    }

    // Upgrade properly increases level by 1
    [Test]
    public void Upgrade_IncreasesLevel()
    {
        bool result = MetaData.Upgrade(testDefinition);
        Assert.IsTrue(result);

        Assert.AreEqual(1, MetaData.GetUpgradeLevel(testDefinition));
    }

    // Upgrades cannot exceed max level
    [Test]
    public void Upgrade_CannotExceedMaxLevel()
    {
        // Awards enough Currency
        MetaData.LoadFromSave(new SaveData(InitialCurrency*100, new Dictionary<PermanentUpgradeType, int>()));

        MetaData.Upgrade(testDefinition); // lvl 1
        MetaData.Upgrade(testDefinition); // lvl 2
        bool result = MetaData.Upgrade(testDefinition); // lvl 3
        Assert.IsTrue(result);

        result = MetaData.Upgrade(testDefinition); // lvl 4, but max is lvl 3
        Assert.IsFalse(result);

        Assert.AreEqual(3, MetaData.GetUpgradeLevel(testDefinition));
    }

    // If not enough currency, cannot upgrade and does not deduct currency
    [Test]
    public void Upgrade_WithInsufficientCurrencyFails()
    {
        MetaData.Upgrade(testDefinition); // lvl 1
        bool result = MetaData.Upgrade(testDefinition); // lvl 2
        Assert.IsTrue(result);
        Assert.AreEqual(InitialCurrency - LevelOneCost - LevelTwoCost, MetaData.MetaCoins);

        // Should fail
        result = MetaData.Upgrade(testDefinition); // lvl 3
        Assert.IsFalse(result);
        Assert.AreEqual(InitialCurrency - LevelOneCost - LevelTwoCost, MetaData.MetaCoins);
        Assert.AreEqual(2, MetaData.GetUpgradeLevel(testDefinition));
    }

    // Properly refunds currency
    [Test]
    public void Downgrade_RefundsCurrency()
    {
        MetaData.Upgrade(testDefinition); // lvl 1
        Assert.AreEqual(InitialCurrency - LevelOneCost, MetaData.MetaCoins);
        Assert.AreEqual(1, MetaData.GetUpgradeLevel(testDefinition));

        bool result = MetaData.Downgrade(testDefinition); // lvl 1 -> lvl 0
        Assert.IsTrue(result);
        Assert.AreEqual(InitialCurrency, MetaData.MetaCoins);
        Assert.AreEqual(0, MetaData.GetUpgradeLevel(testDefinition));
    }

    // Attempting to downgrade at level 0 should not do anything
    [Test]
    public void Downgrade_AtLevelZeroFails()
    {
        bool result = MetaData.Downgrade(testDefinition);
        Assert.IsFalse(result);
        Assert.AreEqual(InitialCurrency, MetaData.MetaCoins);
    }

    // Lookup values via MetaDataLookup should return the correct values
    [Test]
    public void Lookup_LevelZeroEffectValueEqualZero()
    {
        float result = MetaDataLookup.GetEffectValueByType(testType);
        Assert.AreEqual(0, result);
    }

    // Lookup values via MetaDataLookup should return the correct values
    [Test]
    public void Lookup_EffectValues()
    {
        // Awards enough Currency
        MetaData.LoadFromSave(new SaveData(InitialCurrency*100, new Dictionary<PermanentUpgradeType, int>()));

        MetaData.Upgrade(testDefinition); // lvl 1
        float result = MetaDataLookup.GetEffectValueByType(testType);
        Assert.AreEqual(LevelOneEffect, result);

        MetaData.Upgrade(testDefinition); // lvl 2
        result = MetaDataLookup.GetEffectValueByType(testType);
        Assert.AreEqual(LevelTwoEffect, result);

        MetaData.Upgrade(testDefinition); // lvl 3
        result = MetaDataLookup.GetEffectValueByType(testType);
        Assert.AreEqual(LevelThreeEffect, result);
    }
}