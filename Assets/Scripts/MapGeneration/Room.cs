using UnityEngine;

// Data Structure for a Room with mostly read-only fields
public class Room
{
    public readonly int RoomNumber;
    public readonly Vector2 Location;
    public readonly int Distance;

    // Adjustable Fields (for post map generation)
    public Sprite Icon;
    public bool IsDeadend;

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
}