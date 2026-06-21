using UnityEngine;

// Attached to the direction itself
// Controls the Door Visuals and Active/Inactive
// Teleport Trigger Control by setting parent to inactive
// If a branch direction has no path, it should be set inactive - all visuals will also be hidden
public class DoorwayController : MonoBehaviour
{
    [SerializeField] private GameObject carpet;
    [SerializeField] private GameObject doorOpen;
    [SerializeField] private GameObject doorClosed;
    [SerializeField] private GameObject wallOpen;

    // Doorway has 3 states
    public void SetState(DoorwayState state)
    {
        switch (state)
        {
            // Turn Parent Inactive, Disables Visual and Trigger
            case DoorwayState.NoDoor:
                gameObject.SetActive(false);
                break;
            // All Visuals are set to Locked Door
            case DoorwayState.Locked:
                VisualActive(state);
                break;
            // All Visuals are set to Unlocked Door
            case DoorwayState.Unlocked:
                VisualActive(state);
                break;
        }
    }

    // Set all visual elements to locked/unlocked
    private void VisualActive(DoorwayState state)
    {
        switch (state)
        {
            case DoorwayState.Locked:
                wallOpen.SetActive(false);
                doorOpen.SetActive(false);
                doorClosed.SetActive(true);                
                break;
            case DoorwayState.Unlocked:
                wallOpen.SetActive(true);
                doorOpen.SetActive(true);
                doorClosed.SetActive(false);                
                break;
        }

    }
}