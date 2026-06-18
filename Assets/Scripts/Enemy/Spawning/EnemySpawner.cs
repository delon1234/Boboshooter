using UnityEngine;

// Responsible for logic of spawning enemies into the GameRoom
public class EnemySpawner
{   
    private EnemySpawnTable EnemySpawnTable;

    public EnemySpawner(EnemySpawnTable EnemySpawnTable)
    {
        this.EnemySpawnTable = EnemySpawnTable;
    }

    private void SpawnEnemies(Room room, GameObject GameRoom)
    {
        RoomRuntime roomRuntime = GameRoom.GetComponent<RoomRuntime>();
        Debug.Log($"spawn enemy in {room.RoomNumber}");
        int enemyCount = Random.Range(2, 5);

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject EnemyPrefab = GetRandomEnemy();
            Vector2 pos = roomRuntime.GetRandomPoint(GameRoom);
            Object.Instantiate(EnemyPrefab, pos, Quaternion.identity, GameRoom.transform);
        }
    }

    // Picks an enemy as defined in the weights
    private GameObject GetRandomEnemy()
    {
        // Sum all weights
        int totalWeight = 0;
        foreach (var enemy in EnemySpawnTable.Enemies)
        {
            totalWeight += Mathf.Max(1, enemy.Weight);
        }

        // Picks enemy based on rolled weight
        // Loops through table, subtracts weight from roll until it < 0
        int roll = Random.Range(0, totalWeight);
        foreach (var enemy in EnemySpawnTable.Enemies)
        {
            roll -= Mathf.Max(1, enemy.Weight);
            if (roll < 0)
            {
                return enemy.Prefab;
            }   
        }
        return EnemySpawnTable.Enemies[0].Prefab;
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
