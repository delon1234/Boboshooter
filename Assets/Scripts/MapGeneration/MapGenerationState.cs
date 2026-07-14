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

    public Room CreateRoom(Vector2 location, int distance)
    {
        Room room = new Room(nextRoomNumber, location, distance);
        rooms[room.Location] = room;
        nextRoomNumber += 1;
        return room;
    }

    public void ResetState()
    {
        rooms.Clear();
        nextRoomNumber = 0;
    }

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

    // Maximum Distance + RoomType is Normal
    public Room GetFurthestAvailableRoom()
    {
        if (NumberOfRooms == 0)
        {
            return null;
        }
        int maxDistance = Rooms.Max(room => room.Distance);

        // Filters all rooms for Max Distance + RoomType is Normal
        List<Room> candidateRooms = Rooms.Where(room => room.Distance == maxDistance).Where(room => room.Type == RoomType.Normal).ToList();
        while (candidateRooms.Count == 0 && maxDistance > 0)
        {
            // Ideally, candidate will not be 0, but if it is, decrease Max by 1 and pick again.
            maxDistance -= 1;
            candidateRooms = Rooms.Where(room => room.Distance == maxDistance).Where(room => room.Type == RoomType.Normal).ToList();
        } 

        // Guard against Candidate 0. Should not happen
        Debug.Log("Room Generation Error, GetFurthestAvailableRoom has no Candidates");
        return candidateRooms.Count == 0 ? null : candidateRooms[Random.Range(0, candidateRooms.Count)];
    }
}