using System;
using UnityEngine;

public class PlayerRoomTeleport : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private RoomSpawner spawner;
    public static PlayerRoomTeleport Instance;

    // Need static Instance as I need to Serialize fields 
    private void Awake()
    {
        Instance = this;
    }

    public void TeleportToRoom(Room targetRoom, Vector2 fromDirection, GameObject GameRoom)
    {
        if (targetRoom == null || GameRoom == null)
        {
            return;
        }
        // Find all doors in the GameRoom, and determine the opposite side door
        // then retrieve the corresponding SpawnPoint position to teleport Player to
        Door[] doors = GameRoom.GetComponentsInChildren<Door>();
        Vector2 opposite = GetOpposite(fromDirection);
        Transform spawnPoint = null;
        foreach (Door d in doors)
        {
            if (d.direction == opposite)
            {
                spawnPoint = d.SpawnPoint;
                break;
            }
        }

        if (spawnPoint != null)
        {
            player.transform.position = spawnPoint.position;
        }
    }

    private Vector2 GetOpposite(Vector2 dir)
    {
        if (dir == Vector2.up) return Vector2.down;
        if (dir == Vector2.down) return Vector2.up;
        if (dir == Vector2.left) return Vector2.right;
        if (dir == Vector2.right) return Vector2.left;

        return Vector2.zero;
    }

    public void OnRoomChanged(OnRoomChangedArgs args)
    {
        TeleportToRoom(args.EnteredRoom, args.EnterDirection, args.GameRoom);
    }
}