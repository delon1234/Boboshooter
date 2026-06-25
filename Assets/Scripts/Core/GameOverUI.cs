using UnityEngine;

// Attached to canvas in GameOver Scene
public class GameOverUI : MonoBehaviour
{
    public void Replay()
    {
        SceneLoader.LoadGame();
    }

    public void MainMenu()
    {
        SceneLoader.LoadMainMenu();
    }
}