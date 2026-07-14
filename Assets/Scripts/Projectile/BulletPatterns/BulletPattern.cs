using UnityEngine;
using UnityEngine.Pool;
public abstract class BulletPattern : ScriptableObject
{
    /* BulletPattern is responsible for how bullets are spawned (geometry), but with no timing elements.
    Shoot() must complete shooting in 1 frame (no coroutines allowed in ScriptableObject)
    */
    public abstract void Shoot(Transform shootPoint, WeaponStats weaponStats, ObjectPool<Bullet> pool);
}
