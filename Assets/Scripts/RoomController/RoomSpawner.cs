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

    public void SpawnAllRooms()
    {
        foreach (Room room in state.Rooms)
        {
            GameObject obj = GameObject.Instantiate(roomPrefab, room.Location, Quaternion.identity);
            obj.name = $"Room {room.RoomNumber}";
            obj.SetActive(false);
            spawnedRooms[room.Location] = obj;
        }
    }

    public void SetActiveRoom(Vector2 location)
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
        }
    }
}