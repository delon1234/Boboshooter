using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Starting point from Unity Run
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
    public Sprite DefaultRoom;
    public Sprite CurrentRoom;

    private MapGenerationConfig config;
    private MapGenerationState state;
    private MapGenerator generator;
    private MinimapRenderer renderer;

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
            DefaultRoom,
            CurrentRoom);

        // Contains current state of map mutated by Generation and used by Renderer
        state = new MapGenerationState();
        renderer = new MinimapRenderer(transform, config);
        generator = new MapGenerator(config, state);
    }

    // Simulate Room Rerender
    private void DebugFunction()
    {
        state.ClearRoom();
        generator.Generate();
        renderer.FreshRender(state.Rooms);
    }

    private void Start()
    {
        generator.Generate();
        renderer.FreshRender(state.Rooms);
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
        {
            DebugFunction();
        }
    }
}
