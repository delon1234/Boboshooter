using UnityEngine;

// Attached to canvas in MainMenu Scene
public class MainMenuUI : MonoBehaviour
{
    public void StartGame()
    {
        SceneLoader.LoadGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}