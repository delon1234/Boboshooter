using System.Collections.Generic;
using UnityEngine;

// Responsible for Instantiating the room objects into the scene
public class RoomSpawner
{
    private MapGenerationState state;
    private RoomSpawnTable RoomSpawnTable;

    // GameObject parenting all GameRooms, easy for resets
    private Transform FloorRoot;
    // Dictionary Lookup for GameRooms
    private Dictionary<Vector2, GameObject> spawnedRoomObjects = new();

    public RoomSpawner(MapGenerationState state, RoomSpawnTable RoomSpawnTable)
    {
        this.state = state;
        this.RoomSpawnTable = RoomSpawnTable;
    }

    public void SpawnAllRooms(RoomManager RoomManager)
    {
        FloorRoot = new GameObject("FloorRoot").transform;

        foreach (Room room in state.Rooms)
        {
            GameObject ChosenGameRoom = GetRoomPrefab(room); 
            GameObject GameRoom = Object.Instantiate(ChosenGameRoom, Vector2.zero, Quaternion.identity);
            IRoomRuntime runtime = GameRoom.GetComponentInChildren<IRoomRuntime>();
            runtime.Initialize(room);

            GameRoom.SetActive(false);
            GameRoom.name = $"Room {room.RoomNumber}";
            GameRoom.transform.SetParent(FloorRoot);
            InitializeDoors(GameRoom, room, RoomManager);
            spawnedRoomObjects[room.Location] = GameRoom;
        }
    }

    // Chooses the correct GameRoom using the Room's RoomType
    // Normal Rooms go through RNG
    // The rest (special) is currently fixed defined
    private GameObject GetRoomPrefab(Room room)
    {
        // Starting room must always return the Start room (some rooms middle is wall, not suitable to start in)
        if (room.IsStartingRoom)
        {
            return RoomSpawnTable.StartingRoom.Prefab;
        }

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
        foreach (var keyValue in spawnedRoomObjects)
        {
            keyValue.Value.SetActive(false);
        }

        // Set requested room as Active
        if (spawnedRoomObjects.TryGetValue(location, out GameObject room))
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

    // Resets everything, GameRoom parent and dictionary
    public void Reset()
    {
        if (FloorRoot != null)
        {
            Object.Destroy(FloorRoot.gameObject);
        }
        spawnedRoomObjects.Clear();
    }
}