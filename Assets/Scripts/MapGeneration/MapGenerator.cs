using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class MapGenerator
{
    private MapGenerationConfig config;
    private MapGenerationState state;

    public MapGenerator(MapGenerationConfig config, MapGenerationState state)
    {
        this.config = config;
        this.state = state;
    }

    // Start with Room 0,0
    // Then follow nested generation rules till completion
    // Then returns a fully generated map state
    public MapGenerationState Generate()
    {
        Room startingRoom = state.CreateRoom(config.CurrentRoomIcon, new Vector2(0, 0), 0);
        HandleNestedGeneration(startingRoom);

        // Always try generate till minimum
        while (state.NumberOfRooms < config.MinRoomCount)
        {
            List<Room> deadendRooms = state.DeadendRooms();
            Room selectedRoom = deadendRooms[Random.Range(0, deadendRooms.Count)];
            HandleNestedGeneration(selectedRoom);
        }

        // Sets a random furthest room into a boss room
        Room furthestRoom = state.GetFurthestRoom();
        if (furthestRoom != null)
        {
            furthestRoom.SetIcon(config.BossRoomIcon);
        }

        return state;
    }

    private void HandleNestedGeneration(Room room)
    {
        bool isDeadend = true;

        // // If room dont exist, add room
        // if (!state.ContainsRoom(room.Location))
        // {
        //     state.AddRoom(room);
        // }

        // Define and shuffle 4 Cardinal Directions for non bias generation
        List<(MapGenerationRules.Direction direction, Vector2 offset)> directions = new List<(MapGenerationRules.Direction direction, Vector2 offset)>
        {
            (MapGenerationRules.Direction.Left, Vector2.left),
            (MapGenerationRules.Direction.Right, Vector2.right),
            (MapGenerationRules.Direction.Up, Vector2.up),
            (MapGenerationRules.Direction.Down, Vector2.down),
        };

        for (int i = directions.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (directions[i], directions[j]) = (directions[j], directions[i]);
        }

        // Attempt each direction, if eligible for generation, create new room
        foreach (var (direction, offset) in directions)
        {
            if (Random.value >= config.RoomGenerationChance)
            {
                continue;
            }

            Vector2 position = room.Location + offset;

            if (MapGenerationRules.IsEligibleForGeneration(state, config, position, direction))
            {
                Room newRoom = state.CreateRoom(config.DefaultRoomIcon, position, room.Distance + 1);
                HandleNestedGeneration(newRoom);
                isDeadend = false;
            }
        }

        if (isDeadend)
        {
            room.MarkDeadend();
        }
    }
}