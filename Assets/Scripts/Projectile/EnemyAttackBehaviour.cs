using System.Collections;
using UnityEngine;

/// <summary>
/// Abstract base for all timed enemy attack patterns.
/// Drives WHEN and HOW OFTEN BulletPattern.Shoot() is called over time.
/// Coroutines are started by EnemyAttackController — ScriptableObjects cannot
/// start their own coroutines, so <paramref name="runner"/> is provided as the host.
/// 
/// Each concrete subclass owns its own <see cref="AttackPatternData"/>, meaning
/// every sub-attack in a SequenceAttack can use different bullet types and stats.
/// </summary>
public abstract class EnemyAttackBehaviour : ScriptableObject
{
    /// <summary>
    /// Execute the attack. Must be run as a coroutine via EnemyAttackController.
    /// Stats and pool are resolved internally from each subclass's AttackPatternData.
    /// </summary>
    /// <param name="runner">The enemy MonoBehaviour hosting the coroutine.</param>
    /// <param name="shootPoint">Transform from which bullets are spawned.</param>
    public abstract IEnumerator Execute(MonoBehaviour runner, Transform shootPoint);
}
