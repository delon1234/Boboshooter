using UnityEngine;

public class StaircaseTrigger : MonoBehaviour
{
    private GameFlowManager gameFlowManager;

    public void Initialize(GameFlowManager gameFlowManager)
    {
        this.gameFlowManager = gameFlowManager;
    }
    
    // Simply tells flow manager that player has reached staircase
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        gameFlowManager.AdvanceFloor();
    }
}