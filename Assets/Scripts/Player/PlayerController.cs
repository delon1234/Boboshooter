using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 5f;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float buffer = 0.1f;
    [SerializeField] private float dashCooldown = 1f;

    public event Action<bool> OnDashStateChanged; // Event to notify when the player starts or stops dashing

    private bool _isDashing = false;
    private bool _canDash = true;

    private PlayerHealth _health;
    private PlayerInputHandler _input;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _input = GetComponent<PlayerInputHandler>();
        _rb = GetComponent<Rigidbody2D>();
        _health = GetComponent<PlayerHealth>();
    }

    private void FixedUpdate()
    {
        if (_isDashing) { return; } // Disable movement input from affecting player during dash
        // Normalize the input vector to ensure consistent movement speed in all directions
        // Solves the issue of faster diagonal movement when both horizontal and vertical inputs are active
        _rb.linearVelocity = Vector2.ClampMagnitude(_input.MoveInput, 1f) * movementSpeed;
    }

    private void OnEnable()
    {
        _input.DashPressed += HandleDash;
    }

    private void OnDisable()
    {
        if (_input != null)
        {
            _input.DashPressed -= HandleDash;
        }
    }

    private IEnumerator Dash()
    {
        _canDash = false;
        _isDashing = true;
        OnDashStateChanged?.Invoke(_isDashing);
        _health.GainInvulnerability(dashDuration + buffer);
        _rb.linearVelocity = _input.MoveInput.normalized * dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        _isDashing = false;
        OnDashStateChanged?.Invoke(_isDashing);
        yield return new WaitForSeconds(dashCooldown);
        _canDash = true;
    }

    private void HandleDash()
    {
        if (_canDash)
        {
            StartCoroutine(Dash());
        }
    }
}
