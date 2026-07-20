using System;
using NUnit.Framework;

public class WeightedRandomTests
{
    private TestWeightedEntry[] TestWeightedTable;

    // Create a test entry for testing
    private class TestWeightedEntry : IWeighted
    {
        public String value;
        public int weight;

        // Interface forces a Weight {get;}
        // But I want to serialize it in Unity, therefore weight will be serialized
        // Weight will go through get to return weight
        public int Weight { get { return weight; } }

        public TestWeightedEntry(String val, int weight)
        {
            this.value = val;
            this.weight = weight;
        }
    }

    // Nothing needs to be done before each run because Tests will create a new list every run
    [SetUp]
    public void SetUp()
    {
    }

    // Nothing needs to be done after each run because Tests will create a new list every run
    [TearDown]
    public void TearDown()
    {
    }

    // If Table only has 1 item, it will pick that item
    [Test]
    public void WeightedRandom_OnlyOneEntry()
    {
        TestWeightedEntry entry = new TestWeightedEntry("test", 1);
        this.TestWeightedTable = new TestWeightedEntry[]
        {
            entry
        };
    
        for (int i = 0; i < 100; i++)
        {
            TestWeightedEntry result = WeightedRandom.Pick(TestWeightedTable);
            Assert.AreEqual(entry, result);
        }
    }

    // If Table has an item with 0 weight, it will never pick that item
    [Test]
    public void WeightedRandom_ZeroWeightNeverPicked()
    {
        TestWeightedEntry entry0 = new TestWeightedEntry("test0", 0);
        TestWeightedEntry entry1 = new TestWeightedEntry("test1", 1000);
        this.TestWeightedTable = new TestWeightedEntry[]
        {
            entry0,
            entry1
        };
    
        bool pickedEntry0 = false;
        for (int i = 0; i < 100; i++)
        {
            TestWeightedEntry result = WeightedRandom.Pick(TestWeightedTable);
            if (result == entry0)
            {
                pickedEntry0 = true;
            }
        }
        Assert.IsFalse(pickedEntry0);
    }
}