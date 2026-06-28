using UnityEngine;

public class StaircaseTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        RunData.AdvanceFloor();
        SceneLoader.LoadGame();
    }
}