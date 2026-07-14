using UnityEngine;
using UnityEngine.Pool;

public class Shooter : MonoBehaviour
{
    /* Responsible for tracking ammo, reloading of weapons
    */
    #region Inspector Variables
    [SerializeField] private Transform shootPoint;
    [SerializeField] private WeaponData activeWeapon;
    #endregion

    #region Private variables
    private PlayerInputHandler input;
    private WeaponStats cachedStaticStats;    
    #endregion

    #region State variables
    private float cooldownTimer = 0f; // Timer to control fireRate
    private bool isBursting;
    private int totalShotsFired;
    #endregion

    private void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
    }

    private void OnEnable() {
        if (input != null)
        {
            input.OnFirePressed += TryFire;
        }
    }

    private void OnDisable()
    {
        if (input != null)
        {
            input.OnFirePressed -= TryFire;
        }
    }

    public void TryFire() {
        // 1. Check cooldown based on fireRate
        if (cooldownTimer > 0f) return;
        // 2. Checks ammo
        // 3. Reset cooldown timer
        cooldownTimer = 1f / activeWeapon.baseStats.fireRate;
        // 4. Fire
        FireOnce();
    }

    public void FireOnce() {
        // Reduce ammo by 1
        // Get Final Stats
        WeaponStats finalStats = activeWeapon.baseStats;
        ObjectPool<Bullet> pool = PoolManager.Instance.GetPool<Bullet>(activeWeapon.bulletPrefab);
        activeWeapon.bulletPattern.Shoot(shootPoint, finalStats, pool);
    }

    private void Update() {
        if (cooldownTimer > 0f) cooldownTimer -= Time.deltaTime;
    }
}
