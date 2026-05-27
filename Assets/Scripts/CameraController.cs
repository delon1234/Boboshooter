using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float fixedY = 0f;
    [SerializeField] private float fixedZ = -10f;

    private Transform playerTransform;

    [SerializeField] float lerpSpeed = 5f; // Smoothness of camera movement
    // Percentage of influence the cursor has on the camera's position (0 = only player, 1 = only cursor)
    [SerializeField] float cursorWeight = 0.3f; 

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    private void LateUpdate()
    {
        // LateUpdate() runs after all Update() calls
        // Ensures player position is updated before the camera moves, preventing jittery movement

        // Old code that simply followed the player with a fixed Z offset
        //if (player == null) return;
        //Vector3 desiredPosition = new Vector3(player.position.x, player.position.y, fixedZ);
        //cam.position = Vector3.Lerp(cam.position, desiredPosition, speed * Time.deltaTime);

        // Adjust the camera's position to be between the player's position and the mouse position to see more enemies

        // 1. Using Unity's new Input System to read the mouse position and convert it to world coordinates
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(screenPos);
        // Set Z to 0 as 2D game
        mouseWorld.z = 0f;

        // 2. Using lerp to get midpoint between player and mouse, with cursorWeight as "percentage".
        Vector3 midpoint = Vector3.Lerp(playerTransform.position, mouseWorld, cursorWeight);
        midpoint.z = transform.position.z;

        // 3. Adjust the camera's position to be between the player's position and the mouse position
        transform.position = Vector3.Lerp(transform.position, midpoint, lerpSpeed * Time.deltaTime);

    }
}
