using UnityEngine;

/// <summary>
/// A data-driven boss enemy driven by an FSM with two attack phases.
/// 
/// Variants are created by assigning different <see cref="BossData"/> assets —
/// no subclassing required.
/// 
/// State flow:
///   Chase ──(in range)──► Attack ──(attack done)──► Cooldown ──(timer done)──► Chase
///                              └──(HP &lt; threshold)──► Phase 2 transition
/// </summary>
public class BossEnemy : BasicEnemy
{
    // ── Inspector ────────────────────────────────────────────────────────────────
    [SerializeField] private BossData data;

    // ── Private references ────────────────────────────────────────────────────────
    private EnemyAttackController attackController;

    // ── FSM state ─────────────────────────────────────────────────────────────────
    private enum BossState { Chase, Attack, Cooldown }
    private BossState currentState = BossState.Chase;

    private float cooldownTimer;
    private float periodicInvulnerabilityTimer;
    private bool isPhaseTwo = false;

    // ── Unity lifecycle ───────────────────────────────────────────────────────────

    protected override void Awake()
    {
        base.Awake();
        attackController = GetComponent<EnemyAttackController>();
    }

    protected override void Start()
    {
        base.Start();
        if (data != null)
        {
            periodicInvulnerabilityTimer = data.periodicInvulnerabilityInterval;
        }
    }

    protected override void Update()
    {
        if (isDead) return;
        base.Update();

        CheckPhaseTransition();
        TickPeriodicInvulnerability();
        TickFSM();
    }

    // NOTE: No FixedUpdate here — BasicEnemy.FixedUpdate() calls WalkLogic(),
    // and virtual dispatch already routes it to BossEnemy.WalkLogic() below.

    // ── FSM ───────────────────────────────────────────────────────────────────────

    private void TickFSM()
    {
        switch (currentState)
        {
            case BossState.Chase:
                if (GetDistanceToPlayer() <= data.attackRange)
                    EnterAttack();
                break;

            case BossState.Attack:
                // EnemyAttackController.IsAttacking goes false when the coroutine finishes
                if (!attackController.IsAttacking)
                    EnterCooldown();
                break;

            case BossState.Cooldown:
                cooldownTimer -= Time.deltaTime;
                if (cooldownTimer <= 0f)
                    currentState = BossState.Chase;
                break;
        }
    }

    private void CheckPhaseTransition()
    {
        if (isPhaseTwo || data.phase2Attack == null) return;

        float healthPercent = Health.CurrentHealth / Health.MaxHealth;
        if (healthPercent < data.phase2HealthThreshold)
        {
            isPhaseTwo = true;

            if (data.invulnerableOnPhaseTransition && data.phaseTransitionInvulnerabilityDuration > 0f)
            {
                GainInvulnerability(data.phaseTransitionInvulnerabilityDuration);
            }

            // Interrupt the current attack immediately and start phase 2
            attackController.StopAttack();
            EnterAttack();
        }
    }

    private void TickPeriodicInvulnerability()
    {
        if (data == null || !data.periodicInvulnerabilityEnabled || data.periodicInvulnerabilityInterval <= 0f) return;

        periodicInvulnerabilityTimer -= Time.deltaTime;
        if (periodicInvulnerabilityTimer <= 0f)
        {
            periodicInvulnerabilityTimer = data.periodicInvulnerabilityInterval;
            GainInvulnerability(data.periodicInvulnerabilityDuration);
        }
    }

    /// <summary>
    /// Grants invulnerability to the boss for the specified duration in seconds.
    /// </summary>
    public void GainInvulnerability(float duration)
    {
        if (Health != null)
        {
            Health.GainInvulnerability(duration);
        }
    }

    private void EnterAttack()
    {
        currentState = BossState.Attack;
        EnemyAttackBehaviour chosen = isPhaseTwo ? data.phase2Attack : data.phase1Attack;
        attackController.ExecuteAttack(chosen);
    }

    private void EnterCooldown()
    {
        currentState = BossState.Cooldown;
        cooldownTimer = data.attackCooldown;
    }

    // ── Movement ──────────────────────────────────────────────────────────────────

    /// <summary>
    /// Stand still while attacking; chase the player otherwise.
    /// </summary>
    protected override void WalkLogic()
    {
        if (currentState == BossState.Attack)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float dist = GetDistanceToPlayer();
        if (dist > data.movementTolerance)
        {
            rb.linearVelocity = GetDirectionToPlayer() * speed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}