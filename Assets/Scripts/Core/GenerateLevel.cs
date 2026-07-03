using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Starting point from Unity Run
// Attached to the Canvas for Minimap
// Handles all game scene logic
public class GenerateLevel : MonoBehaviour
{
    // Serialized map settings and room icons.
    public float Height = 500;
    public float Width = 500;
    public float Scale = 1f;
    public float IconScale = 0.05f;
    public float Padding = 0.005f;
    public float RoomGenerationChance = 0.15f;
    public int MaxRoomAway = 8;
    public int MinRoomCount = 20;
    public int MaxRoomCount = 30;

    public Sprite TreasureRoom;
    public Sprite BossRoom;
    public Sprite ShopRoom;
    public Sprite UnexploredRoom;
    public Sprite ExploredRoom;
    public Sprite CurrentRoom;

    [SerializeField] private Transform minimapObject;

    [SerializeField] private RoomSpawnTable RoomSpawnTable;

    // Scripts
    private MapGenerationConfig config;
    private MapGenerationState state;
    private MapGenerator generator;
    private MinimapRenderer minimap;

    private RoomSpawner RoomSpawner;
    private RoomManager RoomManager;
    private PlayerRoomTeleport PlayerRoomTeleport;
    private EnemySpawner EnemySpawner;

    [SerializeField] private EnemySpawnTable EnemySpawnTable;

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
            TreasureRoom,
            BossRoom,
            ShopRoom,
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

    // Simulate Room Rerender
    private void DebugFunction()
    {
        state.ClearRoom();
        generator.Generate();
        minimap.FreshRender(state.Rooms);
    }

    private void Start()
    {
        generator.Generate();
        minimap.FreshRender(state.Rooms);
        RoomSpawner.SpawnAllRooms(RoomManager);

        // Help other MonoBehaviour scripts subscribes to RoomChange event
        RoomManager.OnRoomChanged += minimap.OnRoomChanged;
        RoomManager.OnRoomChanged += PlayerRoomTeleport.Instance.OnRoomChanged;
        RoomManager.OnRoomChanged += EnemySpawner.OnRoomChanged;

        // Logically Enters the Starting Room
        // Note this has to be done AFTER subscribers listen to OnRoomChanged Event
        RoomManager.Initialize();
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
        {
            DebugFunction();
        }
    }
}
