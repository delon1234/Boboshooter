using System.Collections;
using UnityEngine;

public class MeleeEnemy : BasicEnemy
{
    [Header("Melee Attack Settings")]
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float attackDamage = 5f;
    [SerializeField] private float attackWindup = 0.25f;
    [SerializeField] private float attackDuration = 0.6f;

    [Header("Animation & Visuals")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool defaultFacingLeft = true;
    [SerializeField] private float deathDelay = 0.5f;

    private float attackCooldownTimer;
    private bool isAttacking;
    private bool isDead;
    private Collider2D enemyCollider;

    // Parameter hashes for high-performance Animator updates
    private static readonly int SpeedHash = Animator.StringToHash("speed");
    private static readonly int AttackHash = Animator.StringToHash("attack");
    private static readonly int HurtHash = Animator.StringToHash("hurt");
    private static readonly int DieHash = Animator.StringToHash("die");

    protected override void Awake()
    {
        base.Awake();
        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        enemyCollider = GetComponent<Collider2D>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (Health != null)
        {
            Health.OnTakingDamage += HandleHurt;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (Health != null)
        {
            Health.OnTakingDamage -= HandleHurt;
        }
    }

    private void Update()
    {
        if (isDead) return;

        if (attackCooldownTimer > 0f)
        {
            attackCooldownTimer -= Time.deltaTime;
        }

        // Face player direction
        if (playerTransform != null && spriteRenderer != null)
        {
            Vector2 dir = GetDirectionToPlayer();
            if (Mathf.Abs(dir.x) > 0.05f)
            {
                bool movingRight = dir.x > 0f;
                spriteRenderer.flipX = defaultFacingLeft ? movingRight : !movingRight;
            }
        }

        // Trigger Melee Attack when in range & cooldown ready
        if (!isAttacking && attackCooldownTimer <= 0f && GetDistanceToPlayer() <= attackRange)
        {
            StartCoroutine(PerformAttack());
        }

        // Update Animator speed
        if (animator != null)
        {
            float speedValue = isAttacking ? 0f : rb.linearVelocity.sqrMagnitude;
            animator.SetFloat(SpeedHash, speedValue);
        }
    }

    protected override void WalkLogic()
    {
        if (isDead || isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Walk to player if outside attack range
        float dist = GetDistanceToPlayer();
        if (dist > attackRange)
        {
            Vector2 direction = GetDirectionToPlayer();
            rb.linearVelocity = direction * speed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;
        rb.linearVelocity = Vector2.zero;

        if (animator != null)
        {
            animator.ResetTrigger(AttackHash);
            animator.SetTrigger(AttackHash);
        }

        // Wait for windup before dealing damage
        yield return new WaitForSeconds(attackWindup);

        // Check if player is still in range to hit
        if (!isDead && GetDistanceToPlayer() <= attackRange + 0.5f)
        {
            DealDamageToPlayer(attackDamage);
        }

        // Wait out remainder of attack animation duration
        float remainingTime = attackDuration - attackWindup;
        if (remainingTime > 0f)
        {
            yield return new WaitForSeconds(remainingTime);
        }

        attackCooldownTimer = attackCooldown;
        isAttacking = false;
    }

    private void HandleHurt(DamageInfo damageInfo)
    {
        if (isDead || animator == null) return;
        animator.ResetTrigger(HurtHash);
        animator.SetTrigger(HurtHash);
    }

    public override void OnEnemyDeath(BasicEnemy enemy)
    {
        if (isDead) return;
        isDead = true;

        // Stop movement and disable collisions during death sequence
        if (rb != null) rb.linearVelocity = Vector2.zero;
        if (enemyCollider != null) enemyCollider.enabled = false;

        if (animator != null)
        {
            animator.SetTrigger(DieHash);
        }

        StartCoroutine(HandleDeathRoutine());
    }

    private IEnumerator HandleDeathRoutine()
    {
        yield return new WaitForSeconds(deathDelay);
        DropLoot();
        Destroy(gameObject);
    }
}