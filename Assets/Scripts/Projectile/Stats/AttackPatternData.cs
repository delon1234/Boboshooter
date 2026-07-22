using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "NewEnemyAttack", menuName = "Combat/Enemy Attack Data")]
public class AttackPatternData : ScriptableObject
{
    // Contains necessary data for complex boss attack patterns which require finetuning of stats for balance
    [Header("Visuals & Audio")]
    public GameObject bulletPrefab;
    public AudioClip shootSound;

    [Header("Geometry & Pattern")]
    public BulletPattern bulletPattern;

    [Header("Balancing Stats")]
    public int projectileCount = 1;
    public float projectileSpeed = 10f;
    public float projectileLifetime = 3f;
    public float damage = 1f;
    public float projectileScale = 1f;

    // Helper to generate WeaponStats at runtime
    public WeaponStats GetStats()
    {
        return new WeaponStats
        {
            projectileCount = this.projectileCount,
            projectileSpeed = this.projectileSpeed,
            projectileLifetime = this.projectileLifetime,
            damage = this.damage,
            projectileScale = this.projectileScale
        };
    }
}
