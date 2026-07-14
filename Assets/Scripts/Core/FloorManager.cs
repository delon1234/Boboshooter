using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Starting point from Unity Run
// Attached to the Canvas for Minimap
// Orchestrator of all Floor related Managers
public class FloorManager : MonoBehaviour
{
    // Serialized map settings and room icons.
    [Header("Minimap Parent")]
    [SerializeField] private Transform minimapObject;

    [Header("Minimap Settings")]
    public float Height; // = 500;
    public float Width; // = 500;
    public float Scale; // = 1;
    public float IconScale; // = 0.05f;
    public float Padding; // = 0.005f;

    [Header("Minimap Icon Settings")]
    [SerializeField] private RoomIconEntry[] RoomIcons;
    [SerializeField] private Sprite UnexploredRoom;
    [SerializeField] private Sprite ExploredRoom;
    [SerializeField] private Sprite CurrentRoom;

    [Header("Generator Settings")]
    public float RoomGenerationChance; // = 0.15f;
    public int MaxRoomAway; // = 8;
    public int MinRoomCount; // = 20;
    public int MaxRoomCount; // = 30;

    [Header("Room Spawning Weights")]
    [SerializeField] private RoomSpawnTable RoomSpawnTable;

    [Header("Enemy Spawning Weights")]
    [SerializeField] private EnemySpawnTable EnemySpawnTable;
    // Scripts
    private MapGenerationConfig config;
    private MapGenerationState state;
    private MapGenerator generator;
    private MinimapRenderer minimap;

    private RoomSpawner RoomSpawner;
    private RoomManager RoomManager;
    private PlayerRoomTeleport PlayerRoomTeleport;
    private EnemySpawner EnemySpawner;

    private void Awake()
    {
        // Collects Config Information from Serialized fields into a config instance
        config = new MapGenerationConfig(
            Height,
            Width,
            Scale,
            IconScale,
            Padding,
            RoomGenerationChance,
            MaxRoomAway,
            MinRoomCount,
            MaxRoomCount,
            RoomIcons,
            UnexploredRoom,
            ExploredRoom,
            CurrentRoom);

        // Contains current state of map mutated by Generation and used by Renderer
        state = new MapGenerationState();
        generator = new MapGenerator(config, state);
        minimap = new MinimapRenderer(minimapObject, config, state);
        RoomSpawner = new RoomSpawner(state, RoomSpawnTable);
        RoomManager = new RoomManager(state, RoomSpawner);
        EnemySpawner = new EnemySpawner(EnemySpawnTable);
    }

    private void Start()
    {
        // Help other MonoBehaviour scripts subscribes to RoomChange event
        RoomManager.OnRoomChanged += minimap.OnRoomChanged;
        RoomManager.OnRoomChanged += PlayerRoomTeleport.Instance.OnRoomChanged;
        RoomManager.OnRoomChanged += EnemySpawner.OnRoomChanged;

        GenerateFloor();
    }

    // Expose 1 function to clear and generate a new floor
    public void GenerateNewFloor()
    {
        ClearFloor();
        GenerateFloor();
        PlayerRoomTeleport.TeleportToSpawn();
    }

    // Assumes a clean slate, preps the scene for a new
    private void GenerateFloor()
    {
        generator.Generate();
        minimap.FreshRender(state.Rooms);
        RoomSpawner.SpawnAllRooms(RoomManager);

        // Logically Enters the Starting Room
        // Note this has to be done AFTER subscribers listen to OnRoomChanged Event
        RoomManager.Initialize();
    }

    // Cleans up Level state for a clean slate
    private void ClearFloor()
    {
        RoomSpawner.Reset();
        state.ResetState();
    }
}
