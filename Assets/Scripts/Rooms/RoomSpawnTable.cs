using System;
using UnityEngine;

// Serialized in GenerateLevel.cs
// Will store the data table for Rooms
[Serializable]
public class RoomSpawnTable
{
    [Header("Fixed Rooms")]
    public RoomWeightEntry BossRoom;
    public RoomWeightEntry TreasureRoom;
    public RoomWeightEntry ShopRoom;

    [Header("Weighted Rooms")]
    public RoomWeightEntry[] NormalRooms;
}