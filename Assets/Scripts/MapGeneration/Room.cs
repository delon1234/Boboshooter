using System.Collections.Generic;
using UnityEngine;

// Data Structure for a Room with mostly read-only fields
public class Room
{
    public readonly int RoomNumber;
    public readonly Vector2 Location;
    public readonly int Distance;
    public bool IsNormal;
    public Dictionary<Vector2, Room> Neighbors = new Dictionary<Vector2, Room>();

    // Adjustable Fields (for post map generation)
    public Sprite Icon;
    public bool IsDeadend;

    // Runtime Fields
    public bool IsVisited = false;

    public Room(int roomNumber, Sprite icon, Vector2 location, int distance)
    {
        RoomNumber = roomNumber;
        Icon = icon;
        Location = location;
        Distance = distance;
    }

    public void SetIcon(Sprite icon)
    {
        Icon = icon;
    }

    public void MarkDeadend()
    {
        IsDeadend = true;
    }

    public void Connect(Vector2 direction, Room other)
    {
        Neighbors[direction] = other;
    }

    public bool HasNeighbor(Vector2 direction)
    {
        return Neighbors.ContainsKey(direction);
    }
}