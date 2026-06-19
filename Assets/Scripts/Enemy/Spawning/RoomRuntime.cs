using UnityEngine;

// Attached to RoomPrefabs, collects all enemy spawnpoints
// Exposes GetRandomPoint given a GameRoom for enemy spawning
// Tracks the state of a room (enemies alive)
public class RoomRuntime : MonoBehaviour
{
    // Zone GameObjects will have BoxCollider2D
    [SerializeField] public GameObject[] SpawnzoneObjects;
    private Door[] doors;
    public int AliveEnemies = 0;

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

    public void OnEnemySpawned(int amt)
    {
        AliveEnemies += amt;
    }

    public void OnEnemyDeath(int amt)
    {
        AliveEnemies -= amt;
        if (AliveEnemies <= 0)
        {
            UnlockDoors();
        }
    }
}
