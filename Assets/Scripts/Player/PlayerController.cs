using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private PlayerInputHandler _input;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _input = GetComponent<PlayerInputHandler>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Normalize the input vector to ensure consistent movement speed in all directions
        // Solves the issue of faster diagonal movement when both horizontal and vertical inputs are active
        _rb.linearVelocity = Vector2.ClampMagnitude(_input.MoveInput, 1f) * speed;
    }

}
