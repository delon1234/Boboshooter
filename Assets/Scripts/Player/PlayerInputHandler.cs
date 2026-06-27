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
    // Events for other scripts to subscribe to for input actions
    public event Action OnFirePressed;
    public event Action DashPressed;

    private PlayerInputActions _input;

    private void Awake()
    {
        _input = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _input.Player.Enable();
        // Input.performed is an event that triggers when the specified action is performed (e.g., button press).
        // Invoke event when the button is pressed via lambda. 
        _input.Player.Fire.performed += _ => OnFirePressed?.Invoke();
        _input.Player.Dash.performed += _ => DashPressed?.Invoke();
    }

    private void OnDisable() { 
        _input.Player.Disable(); 
        _input.Player.Fire.performed -= _ => OnFirePressed?.Invoke();
        _input.Player.Dash.performed -= _ => DashPressed?.Invoke();
    }

    private void Update()
    {
        // Reads input per frame to ensure responsive movement. This allows the player to change direction smoothly while moving.
        MoveInput = _input.Player.Move.ReadValue<Vector2>();
    }
}
