using UnityEngine;

public class RoomView : MonoBehaviour
{
    public Vector2 Location;

    public void Initialize(Room room)
    {
        Location = room.Location;
    }
}
