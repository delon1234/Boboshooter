using UnityEngine;

// Attached to canvas in MainMenu Scene
public class MainMenuUI : MonoBehaviour
{
    public void StartGame()
    {   
        RunData.StartNewRun();
        SceneLoader.LoadGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}