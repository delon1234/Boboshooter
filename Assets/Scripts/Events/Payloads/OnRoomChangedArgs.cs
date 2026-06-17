using UnityEngine;

// Information passed around during a Room Change Event
public class OnRoomChangedArgs
{
   public Room EnteredRoom;
   public GameObject GameRoom;
   public Vector2 EnterDirection;

    public OnRoomChangedArgs(Room EnteredRoom, GameObject GameRoom, Vector2 EnterDirection)
    {
        this.EnteredRoom = EnteredRoom;
        this.GameRoom = GameRoom;
        this.EnterDirection = EnterDirection;
    }
}
