using System.Collections;
using UnityEngine;

/// <summary>
/// Attached to enemy GameObjects. Starts and stops <see cref="EnemyAttackBehaviour"/>
/// coroutines on behalf of the enemy AI state machine.
/// 
/// The enemy AI drives this controller — it calls <see cref="ExecuteAttack"/> on
/// phase transitions and <see cref="StopAttack"/> when leaving an attack state.
/// 
/// Does NOT require a Shooter component — each EnemyAttackBehaviour resolves its
/// own bullet prefab, stats, and pool via its embedded AttackPatternData.
/// Shooter can still exist on the same GameObject for simple enemies that call
/// TryFire() directly from their AI logic (e.g. RangedEnemy).
/// </summary>
public class EnemyAttackController : MonoBehaviour
{
    [Header("Shoot Point")]
    [Tooltip("Transform from which bullets are spawned. Should sit at the enemy's barrel or centre.")]
    [SerializeField] private Transform shootPoint;

    private EnemyAttackBehaviour currentAttack;
    private Coroutine activeCoroutine;

    private void OnDisable()
    {
        // Prevent coroutine leaks when the enemy is deactivated mid-attack
        StopAttack();
    }

    /// <summary>
    /// Interrupts any running attack and starts the supplied <paramref name="attack"/>.
    /// Each EnemyAttackBehaviour resolves its own stats and pool internally.
    /// </summary>
    /// <param name="attack">The EnemyAttackBehaviour asset to execute.</param>
    public void ExecuteAttack(EnemyAttackBehaviour attack)
    {
        if (attack == null)
        {
            Debug.LogWarning($"[EnemyAttackController] ExecuteAttack called with a null attack on {gameObject.name}.", this);
            return;
        }

        StopAttack(); // Cancel any in-progress attack first

        currentAttack = attack;
        activeCoroutine = StartCoroutine(ExecuteAttackRoutine(attack));
    }

    private IEnumerator ExecuteAttackRoutine(EnemyAttackBehaviour attack)
    {
        yield return StartCoroutine(attack.Execute(this, shootPoint));
        activeCoroutine = null;
        currentAttack = null;
    }

    /// <summary>
    /// Stops the currently running attack coroutine, if any.
    /// Safe to call when no attack is running.
    /// </summary>
    public void StopAttack()
    {
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
            activeCoroutine = null;
        }
        currentAttack = null;
    }

    // Public readonly state

    /// <summary>Returns true while an attack coroutine is active.</summary>
    public bool IsAttacking => activeCoroutine != null;

    /// <summary>The EnemyAttackBehaviour currently executing, or null if idle.</summary>
    public EnemyAttackBehaviour CurrentAttack => currentAttack;
}
