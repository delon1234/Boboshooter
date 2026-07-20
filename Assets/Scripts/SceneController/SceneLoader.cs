using UnityEngine;
using UnityEngine.SceneManagement;

// Logic for switching between scenes
// Easy to add/replace scene names in one place
public static class SceneLoader
{
    public static void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public static void StartNewRun()
    {
        RunData.StartNewRun();
        SceneManager.LoadScene("GameScene");
    }

    public static void LoadRunOver()
    {
        SceneManager.LoadScene("RunOver");
    }
}