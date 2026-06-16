using System;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private RoomSpawner spawner;
    public static PlayerPosition Instance;

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
}