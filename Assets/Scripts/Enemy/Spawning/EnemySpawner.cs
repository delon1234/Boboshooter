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
        RoomRuntime roomRuntime = GameRoom.GetComponentInChildren<RoomRuntime>();
        int enemyCount = Random.Range(minCount, maxCount);

        for (int i = 0; i < enemyCount; i++)
        {
            EnemyWeightEntry ChosenEnemyEntry = WeightedRandom.Pick(SpawnList);
            GameObject EnemyPrefab = ChosenEnemyEntry.Prefab;
            Vector2 pos = roomRuntime.GetRandomPoint();
            GameObject EnemyInstance = Object.Instantiate(EnemyPrefab, pos, Quaternion.identity, GameRoom.transform);
            BasicEnemy BasicEnemy = EnemyInstance.GetComponent<BasicEnemy>();

            // Pass Enemy into RoomRuntime to help with Door Logic            
            roomRuntime.OnEnemySpawned(BasicEnemy);

            // Pass GameRoom into Enemy to help with Parenting LootDrops under Room
            BasicEnemy.Initialize(roomRuntime);
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
                // Normal Enemy count scaling by floor
                int minCount = 2 + RunData.CurrentFloor;
                int maxCount = 5+ RunData.CurrentFloor;

                SpawnEnemies(room, args.GameRoom, EnemySpawnTable.BasicEnemies, minCount, maxCount);
                break;
        }
        room.HasSpawnedEnemies = true;
    }
}
