using TMPro;
using UnityEngine;

// Attached to canvas in GameOver Scene
public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TMP_Text floorText;

    private void Start()
    {
        floorText.text = $"You have died on Floor {RunData.CurrentFloor}";
    }

    public void Replay()
    {
        SceneLoader.LoadGame();
    }

    public void MainMenu()
    {
        SceneLoader.LoadMainMenu();
    }
}