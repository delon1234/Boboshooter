using UnityEngine;

// Responsible for logic of spawning enemies into the GameRoom
public class EnemySpawner
{   
    private void SpawnEnemies(Room room, GameObject GameRoom)
    {
        Debug.Log($"spawn enemy in {room.RoomNumber}");
    }

    public void OnRoomChanged(OnRoomChangedArgs args)
    {
        Room room = args.EnteredRoom;
        if (room.HasSpawnedEnemies)
        {
            return;
        }

        SpawnEnemies(room, args.GameRoom);
        room.HasSpawnedEnemies = true;
    }
}
