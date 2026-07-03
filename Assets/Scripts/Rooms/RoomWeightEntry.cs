using System;
using UnityEngine;

// DataStruct class - used in RoomSpawnTable.cs
[Serializable]
public class RoomWeightEntry : IWeighted
{
    [SerializeField] public GameObject Prefab;
    [SerializeField] public int weight;

    // Interface forces a Weight {get;}
    // But I want to serialize it in Unity, therefore weight will be serialized
    // Weight will go through get to return weight
    public int Weight { get { return weight; } }

}