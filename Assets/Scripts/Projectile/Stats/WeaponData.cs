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

    [Header("Non-upgradable Weapon Template Stats")]
    public ShootingMode shootingMode;
    public int burstCount;
    public float burstDelay;

    [Header("Stats and Pattern Config")]
    public WeaponStats baseStats;
    public BulletPattern bulletPattern;   // HOW: Behaviour of bullets (single, spread, radial)
}

