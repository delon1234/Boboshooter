using UnityEngine;
using System.Collections;
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
    private AmmoComponent ammoComponent;
    #endregion

    #region State variables
    private float cooldownTimer = 0f; // Timer to control fireRate
    private bool isBursting = false;
    private int totalShotsFired = 0;
    private Coroutine burstCoroutine;
    #endregion

    private void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
        ammoComponent = GetComponent<AmmoComponent>();
        EquipWeapon(activeWeapon);
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

     private void SubscribeToInput() {
        input.OnFirePressed -= TryFire;
        // Auto is polled in Update(); Semi and Burst use press events
        if (activeWeapon.shootingMode != ShootingMode.Auto)
            input.OnFirePressed += TryFire;
    }

    public void EquipWeapon(WeaponData weapon) {
        if (isBursting && burstCoroutine != null) {
            StopCoroutine(burstCoroutine);
            isBursting = false;
            burstCoroutine = null;
        }

        activeWeapon = weapon;
        // PlayerUpgrades.RecalculateStaticStats() or some cached part;
        UpdateBaseStats(activeWeapon.baseStats);
        SubscribeToInput();
        if (ammoComponent != null) {
            ammoComponent.SyncFromStats(cachedStaticStats);
        }
    }

    public void UpdateBaseStats(WeaponStats newBaseStats) {
        // Base weapon stats + Static Modifiers
        cachedStaticStats = newBaseStats;
    }

    public void TryFire() {
        // 1. Check cooldown based on fireRate
        if (cooldownTimer > 0f) return;
        // 2. Check for active burst to not interrupt
        if (isBursting) return;
        // 3. Checks ammo
        if (ammoComponent != null) {
            if (ammoComponent.IsReloading) return; // Do nothing when reloading
            if (ammoComponent.IsMagazineEmpty) {
                ammoComponent.TriggerReload(); // Reload if out of bullets
                return;
            }
        }
        // 4. Reset cooldown timer
        cooldownTimer = 1f / cachedStaticStats.fireRate;

        switch (activeWeapon.shootingMode) {
            case ShootingMode.Auto:
            case ShootingMode.SemiAuto:
                FireOnce();
                break;
            case ShootingMode.Burst:
                burstCoroutine = StartCoroutine(ExecuteBurst());
                break;
        }
    }

    public void FireOnce() {
        // Pure execution of shooting (Checks done in TryFire). Called by TryFire() and burst coroutines
        ammoComponent?.ConsumeBullet();
        totalShotsFired++;
        // Get Final Stats
        WeaponStats finalStats = activeWeapon.baseStats;
        ObjectPool<Bullet> pool = PoolManager.Instance.GetPool<Bullet>(activeWeapon.bulletPrefab);
        activeWeapon.bulletPattern.Shoot(shootPoint, finalStats, pool);
    }

    private IEnumerator ExecuteBurst() {
        isBursting = true;
        int shotsRemaining = ammoComponent != null
        ? ammoComponent.GetShotsAvailable(activeWeapon.burstCount)
        : activeWeapon.burstCount; // enemies have no ammoComponent

        for (int i = 0; i < shotsRemaining; i++) {
            FireOnce();
            // Burst delay - delay between bullets in burst; no delay for last bullet
            if (i < shotsRemaining - 1)
                yield return new WaitForSeconds(activeWeapon.burstDelay);
        }
        if (ammoComponent != null && ammoComponent.IsMagazineEmpty)
            ammoComponent.TriggerReload(); // trigger reload AFTER burst completes
        isBursting = false;
        burstCoroutine = null;
    }

    private void Update() {
        if (cooldownTimer > 0f) cooldownTimer -= Time.deltaTime;

        // Auto-firing check: if the player is holding fire for an automatic weapon
        if (input != null && input.IsFiring && activeWeapon != null && activeWeapon.shootingMode == ShootingMode.Auto) {
            TryFire();
        }
    }
}
