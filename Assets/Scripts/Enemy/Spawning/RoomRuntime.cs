using UnityEngine;

// Attached to RoomPrefabs, collects all enemy spawnpoints
public class RoomRuntime : MonoBehaviour
{
    // Zone GameObjects will have BoxCollider2D
    [SerializeField] public GameObject[] SpawnzoneObjects;

    public BoxCollider2D GetZoneCollider(GameObject zoneObject)
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

}
