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

    public static void LoadGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public static void LoadGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public static void LoadWin()
    {
        SceneManager.LoadScene("GameOver");
    }
}