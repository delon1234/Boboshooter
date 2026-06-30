using System;
using UnityEngine;

// DataStruct class - used in EnemySpawnTable.cs
// Each enemy will have their Prefab and the Weight
[Serializable]
public class EnemyWeightEntry : IWeighted
{
    public GameObject Prefab;
    public int Weight { get; }
}
