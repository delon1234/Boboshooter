using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "RadialPattern", menuName = "Combat/Patterns/Radial")]
public class RadialPattern : BulletPattern
{
    public PatternSpatialSettings spatialSettings;

    public override void Shoot(Transform shootPoint, WeaponStats weaponStats, ObjectPool<Bullet> pool)
    {
        int count = weaponStats.projectileCount;
        GameObject owner = shootPoint.GetComponentInParent<BasicEnemy>()?.gameObject ?? shootPoint.GetComponentInParent<Player>()?.gameObject ?? shootPoint.root.gameObject;

        if (count <= 0) return;

        // Fallback for a single bullet to avoid division or redundant loops
        if (count == 1)
        {
            Bullet bullet = pool.Get();
            Vector3 spawnPos = shootPoint.position + shootPoint.right * spatialSettings.radiusOffset;
            bullet.transform.SetPositionAndRotation(spawnPos, shootPoint.rotation);
            bullet.Initialize(weaponStats, pool, owner);
            return;
        }

        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float currentAngle = i * angleStep;
            Quaternion bulletRotation = Quaternion.Euler(0, 0, currentAngle);
            Quaternion targetRotation = shootPoint.rotation * bulletRotation;

            // Calculate the outer direction relative to the shoot point's rotation
            Vector3 spawnDirection = targetRotation * Vector3.right;
            Vector3 spawnPosition = shootPoint.position + (spawnDirection * spatialSettings.radiusOffset);

            Bullet bullet = pool.Get();
            bullet.transform.SetPositionAndRotation(spawnPosition, targetRotation);
            bullet.Initialize(weaponStats, pool, owner);
        }
    }
}
