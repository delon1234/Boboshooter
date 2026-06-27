using TMPro;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;

    private Player player;
    private PlayerInputHandler input;

    // 1. Cache animator parameter hashes for optimal performance (Prevents runtime string hashing)
    // 2. Prevent string references for type safety
    private static readonly int SpeedHash = Animator.StringToHash("speed");
    private static readonly int IsDashingHash = Animator.StringToHash("isDashing");
    private static readonly int DieTriggerHash = Animator.StringToHash("die");

    void Start()
    {
        SubscribeToEvents();
    }

    void OnEnable() { 
        SubscribeToEvents();
    }

    void OnDisable() {
        UnsubscribeToEvents();
    }
    // Update is called once per frame
    void Update()
    {
        // Set run state based on player's movement input
        float inputMagnitude = input.MoveInput.sqrMagnitude; // Use squared magnitude for single float
        animator.SetFloat(SpeedHash, inputMagnitude);
    }

    private void SubscribeToEvents()
    {
        if (Player.Instance == null) { return; }
        player = Player.Instance;
        input = player.Input;

        // Idempotent subscription to avoid multiple subscriptions
        player.Controller.OnDashStateChanged -= AnimateDash;
        player.Controller.OnDashStateChanged += AnimateDash;

        player.Health.OnDeath -= AnimateDeath;
        player.Health.OnDeath += AnimateDeath;
    }

    private void UnsubscribeToEvents() {
        player.Controller.OnDashStateChanged -= AnimateDash;
        player.Health.OnDeath -= AnimateDeath;
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
