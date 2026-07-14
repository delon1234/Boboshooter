using UnityEngine;

// Data Struct class to link a RoomType to a specific Icon
// Serialized in FloorManager.cs, used in MinimapRenderer.cs
[System.Serializable]
public struct RoomIconEntry
{
    public RoomType Type;
    public Sprite Icon;
}