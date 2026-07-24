using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Composes an ordered list of <see cref="EnemyAttackBehaviour"/> assets and
/// executes them sequentially. Use this as a boss-phase tool: chain spirals,
/// waves, and pauses into a single callable attack routine.
/// 
/// Each sub-attack fully completes before the next begins.
/// Because each sub-attack owns its own AttackPatternData, different entries
/// in the sequence can fire different bullet types, speeds, and sizes.
/// </summary>
[CreateAssetMenu(fileName = "NewSequenceAttack", menuName = "Combat/Enemy Attacks/Sequence Attack")]
public class SequenceAttack : EnemyAttackBehaviour
{
    [Tooltip("Ordered list of EnemyAttackBehaviours to execute one after another.")]
    [SerializeField] private List<EnemyAttackBehaviour> sequence;

    public override IEnumerator Execute(MonoBehaviour runner, Transform shootPoint)
    {
        foreach (EnemyAttackBehaviour attack in sequence)
        {
            if (attack == null) continue;

            // yield return StartCoroutine(...) on the runner fully awaits each sub-attack
            // before proceeding to the next — ensuring strict sequential execution.
            yield return runner.StartCoroutine(attack.Execute(runner, shootPoint));
        }
    }
}
