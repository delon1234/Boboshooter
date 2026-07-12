using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Attached to RoomPrefabs, collects all enemy spawnpoints
// Exposes GetRandomPoint given a GameRoom for enemy spawning
// Tracks the state of a room (enemies alive)
public class RoomRuntime : MonoBehaviour
{
    public Room room;

    // Serializes the parent, children will contain spawnzone(s)
    [SerializeField] public Transform EnemySpawnzoneParent;
    private List<BoxCollider2D> EnemySpawnZones;

    private Door[] doors;
    // Collects all Enemies that belongs to this room
    private HashSet<BasicEnemy> ActiveEnemies = new();

    // For boss rooms that will spawn a staircase to next level
    [SerializeField] private Tilemap staircaseTilemap;
    [SerializeField] private Tile staircaseTile;
    [SerializeField] private GameObject staircaseTriggerPrefab;

    // Need this for Staircase logic
    private GameFlowManager gameFlowManager;

    // Hold reference to Room data on spawn
    public void Initialize(Room room)
    {
        this.room = room;
    }

    // Need to have Door reference for controlling lock/unlock
    private void Awake()
    {
        doors = GetComponentsInChildren<Door>();

        // Collects all child BoxCollider spawnzones on wake
        EnemySpawnZones = new List<BoxCollider2D>();
        foreach (Transform child in EnemySpawnzoneParent)
        {
            if (child.TryGetComponent(out BoxCollider2D zone))
            {
                EnemySpawnZones.Add(zone);
            }
        }

        // "Hack" to find the GameFlowManager script reference
        gameFlowManager = FindFirstObjectByType<GameFlowManager>();
    }

    // Picks a random spawnzone from the GameRoom's list of spawnzones
    private BoxCollider2D GetRandomZone()
    {
        return EnemySpawnZones[Random.Range(0, EnemySpawnZones.Count - 1)];
    }

    // Takes a BoxCollider2D zone and retrieve a random point in its bounds
    public Vector2 GetRandomPoint()
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
        // Subscribe to EnemyDeath for each Enemy
        enemy.GetComponent<EnemyHealth>().OnEnemyDied += OnEnemyDeath;
    }

    public void OnEnemyDeath(BasicEnemy enemy)
    {
        enemy.Health.OnEnemyDied -= OnEnemyDeath;
        ActiveEnemies.Remove(enemy);
        if (ActiveEnemies.Count <= 0)
        {
            UnlockDoors();
        }
        if (ActiveEnemies.Count <= 0 && room.Type == RoomType.Boss)
        {
            SpawnStaircase(enemy.transform.position);
        }
    }

    // Tilemaps uses Vector3 and Vector3Int
    public void SpawnStaircase(Vector3 worldPosition)
    {
        Vector3Int cell = staircaseTilemap.WorldToCell(worldPosition);
        staircaseTilemap.SetTile(cell, staircaseTile);

        GameObject staircase = Instantiate(staircaseTriggerPrefab, staircaseTilemap.GetCellCenterWorld(cell), Quaternion.identity, transform);
        staircase.GetComponent<StaircaseTrigger>().Initialize(gameFlowManager);

    }
}
