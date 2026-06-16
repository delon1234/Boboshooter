using UnityEngine;

// Responsible for Organising Top Level Room Movement
public class RoomManager
{
    private MapGenerationState state;
    private RoomSpawner spawner;

    public Room CurrentRoom;
    public System.Action<Room, Vector2> OnRoomChanged;

    // Define states and spawner script
    public RoomManager(MapGenerationState state, RoomSpawner spawner)
    {
        this.state = state;
        this.spawner = spawner;
    }

    public void Initialize()
    {
        CurrentRoom = state.GetRoom(Vector2.zero);
        EnterRoom(CurrentRoom, Vector2.zero);
    }

    // Attempt to move in the specified direction
    public void MoveTo(Vector2 direction)
    {
        Vector2 target = CurrentRoom.Location + direction;
        Room NextRoom = state.GetRoom(target);
        if (NextRoom == null)
        {
            return;
        }
        EnterRoom(NextRoom, direction);
    }

    // Handle Entering Next Room
    private void EnterRoom(Room newRoom, Vector2 direction)
    {
        CurrentRoom = newRoom;
        CurrentRoom.IsVisited = true;
        GameObject GameRoom = spawner.SetActiveRoom(CurrentRoom.Location);
        PlayerPosition.Instance.TeleportToRoom(CurrentRoom, direction, GameRoom);
        OnRoomChanged?.Invoke(CurrentRoom, direction);
    }
}