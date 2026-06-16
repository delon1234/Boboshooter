using UnityEngine;

// Responsible for Organising Top Level Room Movement
public class RoomManager
{
    private MapGenerationState state;
    private RoomSpawner spawner;
    private Room currentRoom;

    // Define states and spawner script
    public RoomManager(MapGenerationState state, RoomSpawner spawner)
    {
        this.state = state;
        this.spawner = spawner;
        currentRoom = state.GetRoom(Vector2.zero);
        Debug.Log("Start room exists: " + state.GetRoom(Vector2.zero));
        Debug.Log(state);
        spawner.SetActiveRoom(currentRoom.Location);
    }

    // Attempt to move in the specified direction
    public void MoveTo(Vector2 direction)
    {
        Vector2 target = currentRoom.Location + direction;
        Room next = state.GetRoom(target);
        if (next == null)
        {
            return;
        }
        currentRoom = next;
        spawner.SetActiveRoom(currentRoom.Location);
    }
}