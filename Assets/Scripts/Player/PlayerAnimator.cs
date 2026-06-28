using TMPro;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;

    private PlayerController controller;
    private PlayerHealth health;
    private PlayerInputHandler input;

    // 1. Cache animator parameter hashes for optimal performance (Prevents runtime string hashing)
    // 2. Prevent string references for type safety
    private static readonly int SpeedHash = Animator.StringToHash("speed");
    private static readonly int IsDashingHash = Animator.StringToHash("isDashing");
    private static readonly int DieTriggerHash = Animator.StringToHash("die");

    private void Awake()
    {
        controller = GetComponentInParent<PlayerController>();
        health = GetComponentInParent<PlayerHealth>();
        input = GetComponentInParent<PlayerInputHandler>();
    }

    private void OnEnable()
    {
        if (controller != null) controller.OnDashStateChanged += AnimateDash;
        if (health != null) health.OnDeath += AnimateDeath;
    }

    private void OnDisable()
    {
        if (controller != null) controller.OnDashStateChanged -= AnimateDash;
        if (health != null) health.OnDeath -= AnimateDeath;
    }

    // Update is called once per frame
    void Update()
    {
        if (input != null)
        {
            // Set run state based on player's movement input
            float inputMagnitude = input.MoveInput.sqrMagnitude; // Use squared magnitude for single float
            animator.SetFloat(SpeedHash, inputMagnitude);
        }
    }

    private void AnimateDash(bool isDashing)
    {
        animator.SetBool(IsDashingHash, isDashing);
    }

    private void AnimateDeath()
    {
        animator.SetTrigger(DieTriggerHash); // Use Trigger (1 time event)
    }
}
