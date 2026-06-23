using UnityEngine;

// Attached to every single Doors in the Room Prefabs
// Responsible to tell RoomManager which direction the player wants to go
// Deactivated Door = doesnt exist
// Locked Door = exists but does not work (enemies still in room etc)
public class Door : MonoBehaviour
{
    [SerializeField] public Vector2 direction;
    [SerializeField] public Transform SpawnPoint;
    [SerializeField] private DoorwayController doorwayController;

    private RoomManager RoomManager;
    private Room Room;
    private bool locked = false;

    public void Initialize(RoomManager RoomManager, Room Room)
    {
        this.RoomManager = RoomManager;
        this.Room = Room;

        Refresh(Room);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        if (this.locked)
        {
            return;
        }
        RoomManager.MoveTo(direction);
    }

    // Tells DoorwayController to update its activated status
    public void Refresh(Room room)
    {
        bool doorExist = room.HasNeighbor(direction);
        if (!doorExist)
        {
            doorwayController.SetState(DoorwayState.NoDoor);
            return;
        }
        doorwayController.SetState(DoorwayState.Unlocked);
    }

    // Door is active, asked to set to lock/unlocked, impacting trigger conditions
    // Tells doorwayController to handle visuals
    public void SetLocked(bool locked)
    {
        this.locked = locked;
        if (locked)
        {
            doorwayController.SetState(DoorwayState.Locked);
        } else
        {
            doorwayController.SetState(DoorwayState.Unlocked);
        }
    }
}