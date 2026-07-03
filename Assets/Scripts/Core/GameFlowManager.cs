using UnityEngine;

// Attached to Core object in GameScene
// Highest level handling of scene switching
public class GameFlowManager : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private FloorManager floorManager;

    public void AdvanceFloor()
    {
        if (RunData.CurrentFloor >= RunData.FinalFloor)
        {
            RunData.Result = RunResult.Victory;
            SceneLoader.LoadRunOver();
        }
        else
        {
            RunData.AdvanceFloor();
            floorManager.GenerateNewFloor();
        }
    }

    private void HandlePlayerDeath()
    {
        RunData.Result = RunResult.Defeat;
        SceneLoader.LoadRunOver();
    }

    private void OnEnable()
    {
        playerHealth.OnDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        playerHealth.OnDeath -= HandlePlayerDeath;
    }
}