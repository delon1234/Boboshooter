using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerationState
{
    public List<Room> Rooms = new List<Room>();
    public int NumberOfRooms => Rooms.Count;
    private int nextRoomNumber;

    // Debugger Function!
    public void ClearRoom() {
        Rooms = new List<Room>();
    }

    public Room CreateRoom(Sprite icon, Vector2 location, int distance)
    {
        Room room = new Room(nextRoomNumber, icon, location, distance);
        nextRoomNumber += 1;
        return room;
    }

    public void AddRoom(Room room)
    {
        Rooms.Add(room);
    }

    public bool ContainsRoom(Vector2 location)
    {
        return Rooms.Exists(room => room.Location == location);
    }

    public List<Room> DeadendRooms()
    {
        return Rooms.Where(room => room.IsDeadend).ToList();
    }

    public Room GetFurthestRoom()
    {
        if (NumberOfRooms == 0)
        {
            return null;
        }

        int maxDistance = Rooms.Max(room => room.Distance);
        List<Room> furthestRooms = Rooms.Where(room => room.Distance == maxDistance).ToList();
        return furthestRooms[Random.Range(0, furthestRooms.Count)];
    }
}