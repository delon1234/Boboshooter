using System;
using UnityEngine;

// DataStruct class - used in LootTable.cs
// Each Pickup will have their Definition and the Weight
[Serializable]
public class LootWeightEntry : IWeighted
{
    [SerializeField] public PickupDefinition definition;
    [SerializeField] public int weight;

    // Interface forces a Weight {get;}
    // But I want to serialize it in Unity, therefore weight will be serialized
    // Weight will go through get to return weight
    public int Weight { get { return weight; } }
}
