using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Combat/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("General Info")]
    public string weaponName;
    public Sprite weaponSprite;

    [Header("Visuals & Audio")]
    public GameObject muzzleFlashPrefab;
    public AudioClip shootSound;
    public GameObject bulletPrefab; // BulletPrefab - sprite, trail of bullet
    /// <summary>
    /// To align ShootPoint to weapon's barrel
    /// 1. Shift ShootPoint x, while keeping y=0
    /// 2. Since barrel is higher than trigger for guns (pivot for weaponSprite), lower Weapon y's offset to align ShootPoint
    /// Ensures RotatePoint's Y, ShootPoint Y and Barrel Centre are aligned on same axis
    /// </summary> 
    [Tooltip("Local position offset for the weapon sprite (use Y offset to align the barrel to Y=0).")]
    public Vector2 weaponSpriteOffset = Vector2.zero;
    [Tooltip("Local position offset for the muzzle/shoot point (Local Y should be 0).")]
    public Vector2 shootPointOffset = new Vector2(0.5f, 0f);

    [Header("Non-upgradable Weapon Template Stats")]
    public ShootingMode shootingMode;
    public int burstCount;
    public float burstDelay;

    [Header("Stats and Pattern Config")]
    public WeaponStats baseStats;
    public BulletPattern bulletPattern;   // HOW: Behaviour of bullets (single, spread, radial)
}

