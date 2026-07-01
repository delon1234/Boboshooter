using System.Collections.Generic;
using UnityEngine;

// Responsible for Instantiating the room objects into the scene
public class RoomSpawner
{
    private MapGenerationState state;
    private RoomSpawnTable RoomSpawnTable;

    private Dictionary<Vector2, GameObject> spawnedRooms = new();

    public RoomSpawner(MapGenerationState state, RoomSpawnTable RoomSpawnTable)
    {
        this.state = state;
        this.RoomSpawnTable = RoomSpawnTable;
    }

    public void SpawnAllRooms(RoomManager RoomManager)
    {
        foreach (Room room in state.Rooms)
        {
            GameObject ChosenGameRoom = GetRoomPrefab(room); 
            GameObject GameRoom = Object.Instantiate(ChosenGameRoom, Vector2.zero, Quaternion.identity);
            RoomRuntime runtime = GameRoom.GetComponentInChildren<RoomRuntime>();
            runtime.Initialize(room);

            GameRoom.SetActive(false);
            GameRoom.name = $"Room {room.RoomNumber}";
            InitializeDoors(GameRoom, room, RoomManager);
            spawnedRooms[room.Location] = GameRoom;
        }
    }

    // Chooses the correct GameRoom using the Room's RoomType
    // Normal Rooms go through RNG
    // The rest (special) is currently fixed defined
    private GameObject GetRoomPrefab(Room room)
    {
        switch (room.Type)
        {
            case RoomType.Normal:
                return WeightedRandom.Pick(RoomSpawnTable.NormalRooms).Prefab;

            case RoomType.Boss:
                return RoomSpawnTable.BossRoom.Prefab;

            case RoomType.Shop:
                return RoomSpawnTable.ShopRoom.Prefab;

            case RoomType.Treasure:
                return RoomSpawnTable.TreasureRoom.Prefab;


            default:
                return RoomSpawnTable.NormalRooms[0].Prefab;
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