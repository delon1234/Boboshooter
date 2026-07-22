using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "SpreadPattern", menuName = "Combat/Patterns/Spread")]
public class SpreadPattern : BulletPattern
{
    public PatternSpatialSettings spatialSettings;
    
    public override void Shoot(Transform shootPoint, WeaponStats weaponStats, ObjectPool<Bullet> pool)
    {
        /* Arc approach (Fixed Boundary) for spread*/
        int count = weaponStats.projectileCount;
        bool isEnemyBullet = shootPoint.GetComponentInParent<Player>() == null;
        
        // Fallback for single bullet to avoid division by zero
        if (count <= 1)
        {
            Bullet bullet = pool.Get();
            bullet.transform.SetPositionAndRotation(shootPoint.position, shootPoint.rotation);
            bullet.Initialize(weaponStats, pool, isEnemyBullet);
            return;
        }

        float angleStep = spatialSettings.spreadAngle / (count - 1);
        float startingOffset = -spatialSettings.spreadAngle / 2f; // Left Boundary; Shootpoint is centered at 0 deg

        for (int i = 0; i < count; i++)
        {
            float relativeOffset = startingOffset + (i * angleStep);
            // Use Quarternion over Euler Angles
            Quaternion bulletRotation = Quaternion.Euler(0, 0, relativeOffset);
            Quaternion targetRotation = shootPoint.rotation * bulletRotation;

            Bullet bullet = pool.Get();
            bullet.transform.SetPositionAndRotation(shootPoint.position, targetRotation);
            bullet.Initialize(weaponStats, pool, isEnemyBullet);
        }
    }
}

