using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private PlayerInputActions _input;
    private Rigidbody2D _rb;
    private Vector2 _movementInput;

    private void Awake()
    {
        _input = new PlayerInputActions();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody2D>();
        _input.Player.Enable();
    }

    private void OnDisable()
    {
        _rb = null;
        _input.Player.Disable();
    }

    private void Update()
    {
        // Reads input per frame to ensure responsive movement. This allows the player to change direction smoothly while moving.
        _movementInput = _input.Player.Move.ReadValue<Vector2>();
    }


    private void FixedUpdate()
    {
        // Normalize the input vector to ensure consistent movement speed in all directions
        // Solves the issue of faster diagonal movement when both horizontal and vertical inputs are active
        Vector2 movement = Vector2.ClampMagnitude(_movementInput, 1f) * speed;
        _rb.linearVelocity = movement;
    }

}
