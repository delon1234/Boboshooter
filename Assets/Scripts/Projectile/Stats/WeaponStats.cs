using UnityEngine;

[System.Serializable]
public struct WeaponStats
{
    /*
    The single source of truth for weapon parameters
    Holds generic upgradable core stats of weapons; excludes stats needed for upgrades like richochet
    */

    // Gun stats needed by Shooter
    [Tooltip("Bullets per second")]
    public float fireRate;
    [Header("Ammo Stats")]
    public int magazineSize;
    public float reloadTime;
    [Tooltip("Total Ammo Capacity; -1 for infinite ammo")]
    public int ammoReserveCapacity; // -1 = infinite ammo (sentinel value)

    // Visuals and effects stored in BulletPrefab
    [Header("Bullet Stats")]
    public float damage;
    public float projectileSpeed;
    public float projectileLifetime;
    public float projectileScale;

    [Tooltip("Additional Projectiles for BulletPatterns. Default is 1")]
    public int projectileCount; // Default is 1. Modifiers can increase this.
}

