using UnityEngine;

// DataStruct class - used in EnemySpawnTable.cs
// Each enemy will have their Prefab and the Weight
[System.Serializable]
public class EnemyWeightEntry
{
    public GameObject Prefab;
    public int Weight;
}
