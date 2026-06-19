using UnityEngine;

// Attached to every single Doors in the Room Prefabs
// Responsible to tell RoomManager which direction the player wants to go
// Deactivated Door = doesnt exist
// Locked Door = exists but does not work (enemies still in room etc)
public class Door : MonoBehaviour
{
    [SerializeField] public Vector2 direction;
    [SerializeField] public Transform SpawnPoint;
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

    private void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    // Deactivates itself if room does not have neighbour in that direction
    public void Refresh(Room room)
    {
        SetActive(room.HasNeighbor(direction));
    }

    public void SetLocked(bool locked)
    {
        this.locked = locked;
    }
}