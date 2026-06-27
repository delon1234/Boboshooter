using UnityEngine;

// Responsible for logic of spawning enemies into the GameRoom
public class EnemySpawner
{   
    private EnemySpawnTable EnemySpawnTable;

    public EnemySpawner(EnemySpawnTable EnemySpawnTable)
    {
        this.EnemySpawnTable = EnemySpawnTable;
    }

    private void SpawnEnemies(Room room, GameObject GameRoom, EnemyWeightEntry[] SpawnList, int minCount, int maxCount)
    {
        RoomRuntime roomRuntime = GameRoom.GetComponent<RoomRuntime>();
        int enemyCount = Random.Range(minCount, maxCount);

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject EnemyPrefab = GetRandomEnemy(SpawnList);
            Vector2 pos = roomRuntime.GetRandomPoint(GameRoom);
            GameObject EnemyInstance = Object.Instantiate(EnemyPrefab, pos, Quaternion.identity);
            BasicEnemy BasicEnemy = EnemyInstance.GetComponent<BasicEnemy>();
            roomRuntime.OnEnemySpawned(BasicEnemy);
        }
        if (enemyCount > 0)
        {
            roomRuntime.LockDoors();
        }
    }

    // Picks an enemy as defined in the weights
    private GameObject GetRandomEnemy(EnemyWeightEntry[] SpawnList)
    {
        // Sum all weights
        int totalWeight = 0;
        foreach (var enemy in SpawnList)
        {
            totalWeight += Mathf.Max(1, enemy.Weight);
        }

        // Picks enemy based on rolled weight
        // Loops through table, subtracts weight from roll until it < 0
        int roll = Random.Range(0, totalWeight);
        foreach (var enemy in SpawnList)
        {
            roll -= Mathf.Max(1, enemy.Weight);
            if (roll < 0)
            {
                return enemy.Prefab;
            }   
        }
        return SpawnList[0].Prefab;
    }

    // Triggered by RoomManager
    // If new room, spawns enemies
    public void OnRoomChanged(OnRoomChangedArgs args)
    {
        Room room = args.EnteredRoom;
        if (room.HasSpawnedEnemies)
        {
            return;
        }

        if (room.IsBossRoom)
        {
            SpawnEnemies(room, args.GameRoom, EnemySpawnTable.BossEnemies, 1, 1);
        } else
        {
            SpawnEnemies(room, args.GameRoom, EnemySpawnTable.BasicEnemies, 2, 5);
        }

        room.HasSpawnedEnemies = true;
    }
}
