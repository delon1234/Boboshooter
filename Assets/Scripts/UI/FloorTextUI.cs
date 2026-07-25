// Provide visual cue to the Player on which Floor they are currently on
using TMPro;
using UnityEngine;

public class FloorUI : MonoBehaviour
{
    [SerializeField] TMP_Text floorText;

    private void Refresh(OnFloorChangedArgs args)
    {
        floorText.text = $"Floor {args.Current} / {args.Final}";
    }

    // currently "hacky" way to initialise the initial value UI
    private void Start()
    {
        Refresh(new OnFloorChangedArgs(RunData.CurrentFloor, RunData.FinalFloor));
    }

    private void OnEnable()
    {
        RunData.OnFloorChanged += Refresh;
    }

    private void OnDisable()
    {
        RunData.OnFloorChanged -= Refresh;
    }
}