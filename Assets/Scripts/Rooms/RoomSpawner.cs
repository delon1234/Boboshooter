using System.Collections.Generic;
using UnityEngine;

// Responsible for Instantiating the room objects into the scene
public class RoomSpawner
{
    private MapGenerationState state;
    private GameObject roomPrefab;

    private Dictionary<Vector2, GameObject> spawnedRooms = new();

    public RoomSpawner(MapGenerationState state, GameObject prefab)
    {
        this.state = state;
        this.roomPrefab = prefab;
    }

    public void SpawnAllRooms(RoomManager RoomManager)
    {
        foreach (Room room in state.Rooms)
        {
            GameObject GameRoom = GameObject.Instantiate(roomPrefab, room.Location, Quaternion.identity);
            GameRoom.SetActive(false);
            GameRoom.name = $"Room {room.RoomNumber}";
            InitializeDoors(GameRoom, room, RoomManager);
            spawnedRooms[room.Location] = GameRoom;
        }
    }

    public GameObject SetActiveRoom(Vector2 location)
    {
        // Make all room Inactive
        foreach (var keyValue in spawnedRooms)
        {
            keyValue.Value.SetActive(false);
        }

        // Set requested room as Active
        if (spawnedRooms.TryGetValue(location, out GameObject room))
        {
            room.SetActive(true);
            return room;
        }
        return null;
    }

    // Activates all the Door Scripts
    private void InitializeDoors(GameObject GameRoom, Room room, RoomManager RoomManager)
    {
        Door[] doors = GameRoom.GetComponentsInChildren<Door>();
        foreach (Door door in doors)
        {
            door.Initialize(RoomManager, room);
        }
    }
}