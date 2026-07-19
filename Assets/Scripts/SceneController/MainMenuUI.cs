using UnityEngine;

// Attached to canvas in MainMenu Scene
public class MainMenuUI : MonoBehaviour
{
    // NOTE that Loading of Data only happens when you enter the MainMenuUI
    private void Awake()
    {
        SaveManager.Load();
    }

    public void StartGame()
    {   
        SceneLoader.StartNewRun();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}