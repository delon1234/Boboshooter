using UnityEngine;

// Attached to Core object in GameScene
// Highest level handling of scene switching
public class GameFlowManager : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private FloorManager floorManager;

    // After every run, coordinates reward calculation and saving
    private void EndRun(RunResult result)
    {
        RunData.Result = result;
        RunData.UpdateEarnedCurrency();
        MetaData.AddCurrency(RunData.EarnedCurrency);
        SaveManager.Save();
        SceneLoader.LoadRunOver();
    }

    public void AdvanceFloor()
    {
        if (RunData.CurrentFloor > RunData.FinalFloor)
        {
            EndRun(RunResult.Victory);
        }
        else
        {
            RunData.AdvanceFloor();
            floorManager.GenerateNewFloor();
        }
    }

    private void HandlePlayerDeath()
    {
        EndRun(RunResult.Defeat);
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