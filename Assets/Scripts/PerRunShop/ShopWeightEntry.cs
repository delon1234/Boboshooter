using System;
using UnityEngine;

// DataStruct class - used in ShopTable.cs
// Each ShopItem will have their Definition and the Weight
[Serializable]
public class ShopWeightEntry : IWeighted
{
    [SerializeField] public PickupDefinition definition;
    [SerializeField] public int cost;
    [SerializeField] public int weight;

    // Interface forces a Weight {get;}
    // But I want to serialize it in Unity, therefore weight will be serialized
    // Weight will go through get to return weight
    public int Weight { get { return weight; } }
}