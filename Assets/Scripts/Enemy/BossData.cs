using UnityEngine;

/// <summary>
/// Data-driven configuration for a boss variant.
/// Create different boss types by making new BossData assets in the Unity Editor
/// and assigning them to the BossEnemy prefab variant via the Inspector.
/// </summary>
[CreateAssetMenu(fileName = "NewBossData", menuName = "Enemies/Boss Data")]
public class BossData : ScriptableObject
{
    [Header("Movement")]
    [Tooltip("Distance at which the boss stops chasing and enters the attack state.")]
    public float attackRange = 6f;

    [Tooltip("Stopping tolerance — boss halts when within this distance of the player.")]
    public float movementTolerance = 0.3f;

    [Header("Attack Timing")]
    [Tooltip("Seconds to wait between the end of one attack and the start of the next chase.")]
    public float attackCooldown = 2f;

    [Header("Phase 1 (Full health → phase threshold)")]
    public EnemyAttackBehaviour phase1Attack;

    [Header("Phase 2 (Below phase threshold)")]
    [Tooltip("0–1 fraction of MaxHealth. Boss transitions to Phase 2 when CurrentHealth falls below this.")]
    [Range(0f, 1f)]
    public float phase2HealthThreshold = 0.5f;
    public EnemyAttackBehaviour phase2Attack;
}
