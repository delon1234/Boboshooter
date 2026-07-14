using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "SinglePattern", menuName = "Combat/Patterns/Single")]
public class SinglePattern : BulletPattern
{
    public override void Shoot(Transform shootPoint, WeaponStats weaponStats, ObjectPool<Bullet> pool)
    {
        Bullet bullet = pool.Get(); // Retrieves bullet from object pool
        bullet.transform.SetPositionAndRotation(shootPoint.position, shootPoint.rotation);
        bullet.Initialize(weaponStats, pool);
    }
}

