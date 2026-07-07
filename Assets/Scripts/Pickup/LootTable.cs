using System;
using UnityEngine;

// Serialized in BasicEnemy.cs
// Will store the loot table for each enemy
[Serializable]
public class LootTable
{
    public LootWeightEntry[] LootDropTable;
}