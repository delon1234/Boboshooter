using UnityEngine;

// Attached to Doors in the Room Prefabs
// Responsible to tell RoomManager which direction the player wants to go
public class Door : MonoBehaviour
{
    [SerializeField] public Vector2 direction;
    [SerializeField] public Transform SpawnPoint;
    private RoomManager RoomManager;
    private Room Room;

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

}