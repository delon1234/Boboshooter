using TMPro;
using UnityEngine;

// Attached to canvas in RunOver Scene
public class RunOverUI : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text floorText;

    private void Start()
    {
        switch (RunData.Result)
        {
            case RunResult.Victory:
                titleText.text = "Victory!";
                floorText.text = $"You have beat the Floor {RunData.CurrentFloor} boss and won the game!";
                break;

            case RunResult.Defeat:
                titleText.text = "Game Over";
                floorText.text = $"You reached Floor {RunData.CurrentFloor}.";
                break;
        }
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