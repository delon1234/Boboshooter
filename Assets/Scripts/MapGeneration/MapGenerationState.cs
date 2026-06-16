using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerationState
{
    // Stores room state in a dictionary Vector2: Room
    private Dictionary<Vector2, Room> rooms = new Dictionary<Vector2, Room>();
    // Rooms as a Enumerable List, but not editable
    public IEnumerable<Room> Rooms => rooms.Values;
    // NumberOfRooms will Count the dictionary
    public int NumberOfRooms => rooms.Count;
    private int nextRoomNumber;

    // Debugger Function!
    public void ClearRoom()
    {
        rooms.Clear();
        nextRoomNumber = 0;
    }

    public Room CreateRoom(Sprite icon, Vector2 location, int distance)
    {
        Room room = new Room(nextRoomNumber, icon, location, distance);
        rooms[room.Location] = room;
        nextRoomNumber += 1;
        return room;
    }

    // public void AddRoom(Room room)
    // {
    //     rooms[room.Location] = room;
    // }

    public bool ContainsRoom(Vector2 location)
    {
        return rooms.ContainsKey(location);
    }

    public Room GetRoom(Vector2 location) {
        // Safely retrieve room using Dictionary
        rooms.TryGetValue(location, out Room room);
        return room;
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