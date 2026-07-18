using System.Collections.Generic;
using UnityEngine;

// Data Structure for a Room with mostly read-only fields
public class Room
{
    // Assigned during Constructor
    public readonly int RoomNumber;
    public readonly Vector2 Location;
    public readonly int Distance;
    public Dictionary<Vector2, Room> Neighbors = new Dictionary<Vector2, Room>();
    public RoomType Type;

    // Adjustable Fields (for post map generation)
    public bool IsDeadend;
    public bool IsStartingRoom;

    // Runtime Fields
    public bool IsVisited = false;
    public bool HasSpawnedEnemies = false;
    public bool IsCleared = false;

    public Room(int roomNumber, Vector2 location, int distance)
    {
        RoomNumber = roomNumber;
        Location = location;
        Distance = distance;

        // Default Type is Normal until Post Generation processing turns it into a different room
        Type = RoomType.Normal;
        // Default is NOT starting room unless specified
        IsStartingRoom = false;
    }

    // Post Generation , might turn a normal room into other special rooms
    public void SetType(RoomType type)
    {
        Type = type;
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

    // Some room should not spawn enemies
    public void SetPeaceful()
    {
        this.HasSpawnedEnemies = true;
        this.IsCleared = true;
    }

    public void SetStartingRoom()
    {
        this.IsStartingRoom = true;
    }
}