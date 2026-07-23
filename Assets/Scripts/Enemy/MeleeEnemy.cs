using System.Collections;
using UnityEngine;

public class MeleeEnemy : BasicEnemy
{
    [Header("Melee Attack Settings")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float attackDamage = 5f;

    [Header("Movement Settings")]
    [SerializeField] private bool useFlankPathing = true;
    [SerializeField] private float flankOffsetMultiplier = 0.8f;
    [SerializeField] private bool usePredictiveChasing = true;
    [SerializeField] private float predictionLeadTime = 0.35f;

    [Header("Attack Movement Settings")]
    [Tooltip("Speed multiplier while attacking. 0 = complete stop, 0.7 = 70% speed during attack, 1.5 = aggressive forward lunge.")]
    [SerializeField] private float attackMoveSpeedMultiplier = 0.7f;
    [Tooltip("If true, drives straight toward player direction while attacking instead of side pathing.")]
    [SerializeField] private bool lungeTowardsPlayerOnAttack = true;

    [Tooltip("Seconds from attack start until the hit frame. Match this to when the weapon visually strikes.")]
    [SerializeField] private float attackWindup = 0.25f;

    [Tooltip("Total seconds the full attack animation plays. Set to the clip length.")]
    [SerializeField] private float attackDuration = 0.6f;

    [Header("Attack Indicator Settings")]
    [SerializeField] private GameObject attackIndicatorPrefab;

    private float attackCooldownTimer;
    private bool isAttacking;
    private GameObject activeIndicator;

    public bool IsAttacking => isAttacking;

    protected override void OnDisable()
    {
        base.OnDisable();
        if (activeIndicator != null)
            Destroy(activeIndicator);
    }

    protected override void Update()
    {
        if (isDead) return;

        if (attackCooldownTimer > 0f)
            attackCooldownTimer -= Time.deltaTime;

        base.Update();

        // Trigger attack when in range and cooldown is ready
        if (!isAttacking && attackCooldownTimer <= 0f && GetDistanceToPlayer() <= attackRange)
            StartCoroutine(PerformAttack());
    }

    protected override float GetAnimationSpeed()
    {
        return isAttacking ? 0f : base.GetAnimationSpeed();
    }

    protected override void WalkLogic()
    {
        if (isDead || playerTransform == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Maintain movement while attacking based on attackMoveSpeedMultiplier
        if (isAttacking)
        {
            if (attackMoveSpeedMultiplier <= 0f)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            Vector2 attackMoveDir;
            if (lungeTowardsPlayerOnAttack)
            {
                attackMoveDir = GetDirectionToPlayer();
            }
            else
            {
                Vector2 targetPos = usePredictiveChasing 
                    ? GetPredictedPlayerPosition(predictionLeadTime) 
                    : (Vector2)playerTransform.position;
                attackMoveDir = (targetPos - (Vector2)transform.position).normalized;
            }

            rb.linearVelocity = attackMoveDir * (speed * attackMoveSpeedMultiplier);
            return;
        }

        // Normal walk logic when not attacking
        float dist = GetDistanceToPlayer();
        if (dist > attackRange)
        {
            Vector2 targetPos = usePredictiveChasing 
                ? GetPredictedPlayerPosition(predictionLeadTime) 
                : (Vector2)playerTransform.position;

            Vector2 moveDirection;
            if (useFlankPathing)
            {
                float sideSign = (transform.position.x >= targetPos.x) ? 1f : -1f;
                Vector2 targetOffset = new Vector2(sideSign * attackRange * flankOffsetMultiplier, 0f);
                Vector2 finalTarget = targetPos + targetOffset;
                moveDirection = (finalTarget - (Vector2)transform.position).normalized;
            }
            else
            {
                moveDirection = (targetPos - (Vector2)transform.position).normalized;
            }

            rb.linearVelocity = moveDirection * speed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;

        // Instantiate indicator circle overlay centred on enemy during attack windup
        if (attackIndicatorPrefab != null)
        {
            activeIndicator = Instantiate(attackIndicatorPrefab, transform.position, Quaternion.identity, transform);
            activeIndicator.transform.localScale = Vector3.one * (attackRange * 2f);
        }

        TriggerAttackAnimation();

        // Wait for windup before dealing damage (WalkLogic continuously manages movement in Update)
        yield return new WaitForSeconds(attackWindup);

        // Remove indicator once windup ends
        if (activeIndicator != null)
        {
            Destroy(activeIndicator);
        }

        // 360-degree radial check: deal damage if player is within attackRange
        if (!isDead && GetDistanceToPlayer() <= attackRange)
        {
            DealDamageToPlayer(attackDamage);
        }

        // Wait out remainder of the attack animation
        float remaining = attackDuration - attackWindup;
        if (remaining > 0f)
            yield return new WaitForSeconds(remaining);

        attackCooldownTimer = attackCooldown;
        isAttacking = false;
    }

    public override void OnEnemyDeath(BasicEnemy enemy)
    {
        if (activeIndicator != null)
            Destroy(activeIndicator);

        base.OnEnemyDeath(enemy);
    }

    private void OnDrawGizmosSelected()
    {
        // Red sphere = attack trigger / damage radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}