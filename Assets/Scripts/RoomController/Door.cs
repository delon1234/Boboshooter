using UnityEngine;

// Attached to Doors in the Room Prefabs
// Responsible to tell RoomManager which direction the player wants to go
public class Door : MonoBehaviour
{
    public Vector2 direction;
    private RoomManager manager;

    public void Initialize(RoomManager manager)
    {
        this.manager = manager;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        manager.MoveTo(direction);
    }
}