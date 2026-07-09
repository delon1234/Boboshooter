using UnityEngine;

// Attached to canvas in MainMenu Scene
public class MainMenuUI : MonoBehaviour
{
    public void StartGame()
    {   
        SceneLoader.StartNewRun();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}