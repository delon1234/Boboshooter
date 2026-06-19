using System;
using UnityEngine;

// Serialized in GenerateLevel.cs
// Will store the data table for enemies
[Serializable]
public class EnemySpawnTable
{
    public EnemyWeightEntry[] Enemies;
}