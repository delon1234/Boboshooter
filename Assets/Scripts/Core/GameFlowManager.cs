using UnityEngine;

// Attached to GameFlowManager object in GameScene
// Highest level handling of scene switching
public class GameFlowManager : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;

    private void HandlePlayerDeath()
    {
        SceneLoader.LoadGameOver();
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