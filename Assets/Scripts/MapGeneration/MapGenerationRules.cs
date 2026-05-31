using System;
using UnityEngine;

// Custom algorithm for how the rooms should generate
public static class MapGenerationRules
{
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    public static bool IsEligibleForGeneration(MapGenerationState state, MapGenerationConfig config, Vector2 position, Direction direction)
    {
        // If room exist already, not eligible
        if (state.ContainsRoom(position))
        {
            return false;
        }

        // Max Distance from Starting Room
        if (Math.Abs(position.x) > config.MaxRoomAway || Math.Abs(position.y) > config.MaxRoomAway)
        {
            return false;
        }

        // Max Rooms
        if (state.NumberOfRooms > config.MaxRoomCount)
        {
            return false;
        }

        // Check Eligibility of candidate direction (must not have touching branches)
        switch (direction)
        {
            case Direction.Left:
                return !state.ContainsRoom(position + Vector2.left)
                    && !state.ContainsRoom(position + Vector2.up)
                    && !state.ContainsRoom(position + Vector2.down);
            case Direction.Right:
                return !state.ContainsRoom(position + Vector2.right)
                    && !state.ContainsRoom(position + Vector2.up)
                    && !state.ContainsRoom(position + Vector2.down);
            case Direction.Up:
                return !state.ContainsRoom(position + Vector2.left)
                    && !state.ContainsRoom(position + Vector2.right)
                    && !state.ContainsRoom(position + Vector2.up);
            case Direction.Down:
                return !state.ContainsRoom(position + Vector2.left)
                    && !state.ContainsRoom(position + Vector2.right)
                    && !state.ContainsRoom(position + Vector2.down);
        }
        return false;
    }
}