using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Attached to RoomPrefabs, collects all enemy spawnpoints
// Exposes GetRandomPoint given a GameRoom for enemy spawning
// Tracks the state of a room (enemies alive)
public class RoomRuntime : MonoBehaviour
{
    public Room room;

    // Zone GameObjects will have BoxCollider2D
    [SerializeField] public GameObject[] SpawnzoneObjects;
    private Door[] doors;
    // Collects all Enemies that belongs to this room
    private HashSet<BasicEnemy> ActiveEnemies = new();

    // For boss rooms that will spawn a staircase to next level
    [SerializeField] private Tilemap staircaseTilemap;
    [SerializeField] private Tile staircaseTile;
    [SerializeField] private GameObject staircaseTriggerPrefab;

    // Hold reference to Room data on spawn
    public void Initialize(Room room)
    {
        this.room = room;
    }

    // Need to have Door reference for controlling lock/unlock
    private void Awake()
    {
        doors = GetComponentsInChildren<Door>();
    }

    private BoxCollider2D GetZoneCollider(GameObject zoneObject)
    {
        return zoneObject.GetComponent<BoxCollider2D>();
    }

    // Picks a random spawnzone from the GameRoom's list of spawnzones
    private BoxCollider2D GetRandomZone()
    {
        GameObject selectedSpawnzone = SpawnzoneObjects[Random.Range(0, SpawnzoneObjects.Length)];
        return GetZoneCollider(selectedSpawnzone);
    }

    // Takes a BoxCollider2D zone and retrieve a random point in its bounds
    public Vector2 GetRandomPoint(GameObject GameRoom)
    {
        BoxCollider2D selectedSpawnzone = GetRandomZone();
        Bounds b = selectedSpawnzone.bounds;
        return new Vector2(Random.Range(b.min.x, b.max.x), Random.Range(b.min.y, b.max.y));
    }

    public void LockDoors()
    {
        foreach (Door door in doors)
        {
            door.SetLocked(true);
        }
    }

    private void UnlockDoors()
    {
        foreach (Door door in doors)
        {
            door.SetLocked(false);
        }
    }

    public void OnEnemySpawned(BasicEnemy enemy)
    {
        ActiveEnemies.Add(enemy);
    }

    public void OnEnemyDeath(BasicEnemy enemy)
    {
        ActiveEnemies.Remove(enemy);
        if (ActiveEnemies.Count <= 0)
        {
            UnlockDoors();
        }
        if (ActiveEnemies.Count <= 0 && room.IsBossRoom)
        {
            SpawnStaircase(enemy.transform.position);
        }
    }

    // Tilemaps uses Vector3 and Vector3Int
    public void SpawnStaircase(Vector3 worldPosition)
    {
        Vector3Int cell = staircaseTilemap.WorldToCell(worldPosition);
        staircaseTilemap.SetTile(cell, staircaseTile);

        Instantiate(staircaseTriggerPrefab, staircaseTilemap.GetCellCenterWorld(cell), Quaternion.identity, transform);
    }

    // Subscribe to EnemyDeath
    private void OnEnable()
    {
        BasicEnemy.OnEnemyDied += OnEnemyDeath;
    }

    private void OnDisable()
    {
        BasicEnemy.OnEnemyDied -= OnEnemyDeath;
    }
}
