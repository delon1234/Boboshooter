using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Fires a single bullet at a time, rotating the shoot point by <see cref="angleStep"/>
/// between shots to produce an outward spiral.
/// Each SpiralAttack asset owns its own <see cref="AttackPatternData"/>, so different
/// spiral attacks can use entirely different bullet types, speeds, and sizes.
/// </summary>
[CreateAssetMenu(fileName = "NewSpiralAttack", menuName = "Combat/Enemy Attacks/Spiral Attack")]
public class SpiralAttack : EnemyAttackBehaviour
{
    [Header("Bullet Configuration")]
    [Tooltip("Defines the bullet prefab, pattern geometry, and projectile stats for this spiral.")]
    [SerializeField] private AttackPatternData attackData;

    [Header("Spiral Settings")]
    [Tooltip("Degrees to rotate the shoot point between each shot.")]
    [SerializeField] private float angleStep = 10f;

    [Tooltip("Total number of shots fired across the full spiral.")]
    [SerializeField] private int totalShots = 36;

    [Tooltip("Seconds to wait between each individual shot.")]
    [SerializeField] private float timeBetweenShots = 0.08f;

    public override IEnumerator Execute(MonoBehaviour runner, Transform shootPoint)
    {
        WeaponStats stats = attackData.GetStats();
        ObjectPool<Bullet> pool = PoolManager.Instance.GetPool<Bullet>(attackData.bulletPrefab);

        float currentAngle = shootPoint.rotation.eulerAngles.z;

        for (int i = 0; i < totalShots; i++)
        {
            shootPoint.rotation = Quaternion.Euler(0f, 0f, currentAngle);
            attackData.bulletPattern.Shoot(shootPoint, stats, pool);

            currentAngle += angleStep;
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }
}
