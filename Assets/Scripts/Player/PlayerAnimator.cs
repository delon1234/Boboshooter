using System.Collections;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Invulnerability Flicker")]
    [SerializeField] private float flickerOnAlpha = 1f;
    [SerializeField] private float flickerOffAlpha = 0.25f;
    [SerializeField] private float flickerInterval = 0.05f;

    private PlayerController controller;
    private PlayerHealth health;
    private PlayerInputHandler input;

    private Coroutine flickerCoroutine;

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
        if (health != null)
        {
            health.OnDeath += AnimateDeath;
            health.OnInvulnerabilityChanged += OnInvulnerabilityChanged;
        }
    }

    private void OnDisable()
    {
        if (controller != null) controller.OnDashStateChanged -= AnimateDash;
        if (health != null)
        {
            health.OnDeath -= AnimateDeath;
            health.OnInvulnerabilityChanged -= OnInvulnerabilityChanged;
        }
    }

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

    private void OnInvulnerabilityChanged(bool isInvulnerable)
    {
        // health.OnInvulnerabilityChanged calls OnInvulnerabilityChanged here when invulnerability starts and stops 
        // which toggles flickering of sprite
        if (isInvulnerable)
        {
            flickerCoroutine = StartCoroutine(FlickerSprite());
        }
        else
        {
            if (flickerCoroutine != null) StopCoroutine(flickerCoroutine);
            // Restore full opacity when invulnerability ends
            if (spriteRenderer != null)
                spriteRenderer.color = new Color(1f, 1f, 1f, flickerOnAlpha);
        }
    }

    private IEnumerator FlickerSprite()
    {
        while (true)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, flickerOffAlpha);
            yield return new WaitForSeconds(flickerInterval);
            spriteRenderer.color = new Color(1f, 1f, 1f, flickerOnAlpha);
            yield return new WaitForSeconds(flickerInterval);
        }
    }
}
