using UnityEngine;

[System.Serializable]
public struct WeaponStats
{
    /*
    The single source of truth for weapon parameters
    Holds generic upgradable core stats of weapons; excludes stats needed for upgrades like richochet
    */

    // Gun stats needed by Shooter
    public float fireRate;
    // Ammo Stats
    public int magazineSize;
    public float reloadTime;
    public int ammoReserveCapacity;

    // Visuals and effects stored in BulletPrefab
    // Bullet Stats
    public float damage;
    public float projectileSpeed;
    public float projectileLifetime;
    public float projectileScale;

    // Generic count used by BulletPatterns
    public int projectileCount; // Default is 1. Modifiers can increase this.
}

