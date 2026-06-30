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
            EnemyWeightEntry ChosenEnemyEntry = WeightedRandom.Pick(SpawnList);
            GameObject EnemyPrefab = ChosenEnemyEntry.Prefab;
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

    // Triggered by RoomManager
    // If new room, spawns enemies
    public void OnRoomChanged(OnRoomChangedArgs args)
    {
        Room room = args.EnteredRoom;
        if (room.HasSpawnedEnemies)
        {
            return;
        }

        switch (room.Type)
        {
            case RoomType.Boss:
                SpawnEnemies(room, args.GameRoom, EnemySpawnTable.BossEnemies, 1, 1);
                break;
            case RoomType.Normal:
                SpawnEnemies(room, args.GameRoom, EnemySpawnTable.BasicEnemies, 2, 5);
                break;
        }
        room.HasSpawnedEnemies = true;
    }
}
