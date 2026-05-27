using System;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    /*
     Handles Player Input via Unity's new Input System
    - Exposes public properties and events for other Player scripts to subscribe to. 
    - Decouples input handling from other scripts
     */
    public Vector2 MoveInput { get; private set; }
    public event Action OnFirePressed;

    private PlayerInputActions _input;

    private void Awake()
    {
        _input = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _input.Player.Enable();
        // Input.performed is an event that triggers when the specified action is performed (e.g., button press).
        // Create lambda to invoke the OnFirePressed event when the Fire action is performed. 
        _input.Player.Fire.performed += _ => OnFirePressed?.Invoke();
    }

    private void OnDisable() { _input.Player.Disable(); }

    private void Update()
    {
        // Reads input per frame to ensure responsive movement. This allows the player to change direction smoothly while moving.
        MoveInput = _input.Player.Move.ReadValue<Vector2>();
    }
}
