using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Fires a complete BulletPattern <see cref="waveCount"/> times,
/// with a configurable delay between each wave.
/// Each call to Shoot() is instantaneous (single-frame), so all bullets in
/// a wave still appear at once. The timing is owned entirely by this behaviour.
/// Each WaveAttack asset owns its own <see cref="AttackPatternData"/>, so different
/// wave attacks can use entirely different bullet types, speeds, and sizes.
/// </summary>
[CreateAssetMenu(fileName = "NewWaveAttack", menuName = "Combat/Enemy Attacks/Wave Attack")]
public class WaveAttack : EnemyAttackBehaviour
{
    [Header("Bullet Configuration")]
    [Tooltip("Defines the bullet prefab, pattern geometry, and projectile stats for this wave.")]
    [SerializeField] private AttackPatternData attackData;

    [Header("Wave Settings")]
    [Tooltip("Total number of waves to fire.")]
    [SerializeField] private int waveCount = 3;

    [Tooltip("Seconds to wait between each wave.")]
    [SerializeField] private float timeBetweenWaves = 0.5f;

    public override IEnumerator Execute(MonoBehaviour runner, Transform shootPoint)
    {
        WeaponStats stats = attackData.GetStats();
        ObjectPool<Bullet> pool = PoolManager.Instance.GetPool<Bullet>(attackData.bulletPrefab);

        for (int i = 0; i < waveCount; i++)
        {
            attackData.bulletPattern.Shoot(shootPoint, stats, pool);

            // No delay after the final wave — avoids redundant padding when chained in a SequenceAttack
            if (i < waveCount - 1)
                yield return new WaitForSeconds(timeBetweenWaves);
        }
    }
}
