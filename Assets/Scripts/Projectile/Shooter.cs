using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Pool;

public class Shooter : MonoBehaviour
{
    /* Responsible for weapon state, cooldowns, firing mode dispatch, and triggering BulletPattern.
     * Invariant: activeWeapon is never null. The player always holds exactly one weapon.
     * Weapon changes are always swaps — EquipWeapon replaces one weapon with another.
     */
    #region Inspector Variables
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform weaponTransform;
    [SerializeField] private WeaponData activeWeapon;
    #endregion

    #region References
    private PlayerInputHandler input;
    private AmmoComponent ammoComponent;
    private SpriteRenderer weaponSpriteRenderer;
    #endregion

    [field: SerializeField, HideInInspector] 
    private WeaponStats cachedStaticStats; // View changes by upgrades
    private readonly List<IDynamicModifier> dynamicModifiers = new List<IDynamicModifier>();

    #region State variables
    private float cooldownTimer = 0f; // Timer to control fireRate
    private bool isBursting = false;
    private int totalShotsFired = 0;
    private Coroutine burstCoroutine;
    #endregion

    #region Public API
    // PlayerUpgrades reads this to know BaseStats of weapons to apply upgrades
    public WeaponData ActiveWeapon => activeWeapon;
    // Fired with the OLD weapon info just as it is replaced — Pickup uses this to spawn it in the world
    public event Action<DroppedWeaponInfo> OnWeaponDropped;
    // Fired after the new weapon is fully set — PlayerUpgrades uses this to recalculate static stats
    public event Action<WeaponData> OnWeaponEquipped;
    #endregion

    private void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
        ammoComponent = GetComponent<AmmoComponent>();

        if (weaponTransform != null)
        {
            weaponSpriteRenderer = weaponTransform.GetComponent<SpriteRenderer>();
        }

        // Initialize directly — EquipWeapon is for runtime swaps only.
        // PlayerUpgrades hasn't subscribed yet (subscribes in Start) so we set the
        // baseline stats here; PlayerUpgrades will override them with modifiers in its Start().
        UpdateBaseStats(activeWeapon.baseStats);
        SubscribeToInput();
        ammoComponent?.SyncFromStats(cachedStaticStats);
        ApplyWeaponVisualOffsets();
    }

    private void ApplyWeaponVisualOffsets()
    {
        // Adjusts positions of ShootPoint and Weapon gameObjects via offsets in WeaponData to align to various weapon sprites
        // Ensures bullets are spawned at barrel and shoots accurately towards crosshair
        if (activeWeapon == null) return;

        if (shootPoint != null)
        {
            shootPoint.localPosition = activeWeapon.shootPointOffset;
        }

        if (weaponTransform != null)
        {
            weaponTransform.localPosition = activeWeapon.weaponSpriteOffset;

            if (weaponSpriteRenderer == null)
            {
                weaponSpriteRenderer = weaponTransform.GetComponent<SpriteRenderer>();
            }

            if (weaponSpriteRenderer != null)
            {
                weaponSpriteRenderer.sprite = activeWeapon.weaponSprite;
            }
        }
    }

    private void OnEnable() {
        if (input != null)
            input.OnFirePressed += TryFire;
    }

    private void OnDisable()
    {
        if (input != null)
            input.OnFirePressed -= TryFire;
    }

    private void SubscribeToInput() {
        if (input == null) return;
        input.OnFirePressed -= TryFire;
        // Auto is polled in Update(); Semi and Burst use press events
        if (activeWeapon.shootingMode != ShootingMode.Auto)
            input.OnFirePressed += TryFire;
    }

    /// <summary>
    /// Swaps the current weapon for a new one.
    /// Preserves old weapon ammo state when firing OnWeaponDropped, and restores target Magazine & Reserve ammo.
    /// </summary>
    public void EquipWeapon(WeaponData weapon, int targetMagazine = -1, int targetReserve = -1) {
        if (isBursting && burstCoroutine != null) {
            StopCoroutine(burstCoroutine);
            isBursting = false;
            burstCoroutine = null;
        }

        // Store old weapon & ammo state before swapping
        WeaponData oldWeapon = activeWeapon;
        int oldMag = ammoComponent != null ? ammoComponent.CurrentAmmo.CurrentMagazine : -1;
        int oldReserve = ammoComponent != null ? ammoComponent.CurrentAmmo.CurrentReserve : -1;

        activeWeapon = weapon;
        UpdateBaseStats(activeWeapon.baseStats); // baseline
        ApplyWeaponVisualOffsets();

        // Apply target ammo state onto AmmoComponent BEFORE notifying upgrades
        if (ammoComponent != null) {
            ammoComponent.SyncFromStats(cachedStaticStats, targetMagazine, targetReserve);
        }

        OnWeaponEquipped?.Invoke(activeWeapon);  // PlayerUpgrades recalculates static modifiers & updates caps
        SubscribeToInput();

        Debug.Log($"[Shooter] Swapped weapon to '{weapon.weaponName}'. Restoring Mag: {targetMagazine}, Reserve: {targetReserve} (Old weapon '{oldWeapon?.weaponName}' dropped with Mag: {oldMag}, Reserve: {oldReserve})");

        // Fire drop event with captured old weapon and ammo state
        if (oldWeapon != null) {
            OnWeaponDropped?.Invoke(new DroppedWeaponInfo(oldWeapon, oldMag, oldReserve));
        }
    }

    public void UpdateBaseStats(WeaponStats newBaseStats) {
        cachedStaticStats = newBaseStats;
    }

    public void AddDynamicModifier(IDynamicModifier modifier) {
        if (!dynamicModifiers.Contains(modifier))
            dynamicModifiers.Add(modifier);
    }

    public void RemoveDynamicModifier(IDynamicModifier modifier) {
        dynamicModifiers.Remove(modifier);
    }

    public void TryFire() {
        // 1. Check cooldown based on fireRate
        if (cooldownTimer > 0f) return;
        // 2. Check for active burst to not interrupt
        if (isBursting) return;
        // 3. Checks ammo for Player only
        if (ammoComponent != null) {
            if (ammoComponent.IsReloading) return; // Do nothing when reloading
            if (ammoComponent.IsMagazineEmpty) {
                ammoComponent.TriggerReload(); // Reload if out of bullets
                return;
            }
        }
        // 4. Reset cooldown timer for fireRate
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
        // Pure execution of shooting without checks. Called by TryFire() and burst coroutines
        ammoComponent?.ConsumeBullet();
        totalShotsFired++;
        // Build shot context for dynamic modifiers
        ShotContext ctx = new ShotContext {
            totalShotsFired = totalShotsFired,
            ownerHealthPercent = 1f // TODO: wire to HealthComponent when available
        };
        // Apply dynamic modifiers on top of the cached static stats
        WeaponStats finalStats = cachedStaticStats;
        foreach (var mod in dynamicModifiers)
            finalStats = mod.ModifyDynamicStats(finalStats, ctx);
        Debug.Log($"[Shot] dmg={finalStats.damage} fireRate={finalStats.fireRate} count={finalStats.projectileCount}");

        ObjectPool<Bullet> pool = PoolManager.Instance.GetPool<Bullet>(activeWeapon.bulletPrefab);
        activeWeapon.bulletPattern.Shoot(shootPoint, finalStats, pool);
    }

    private IEnumerator ExecuteBurst() {
        isBursting = true;
        int shotsRemaining = ammoComponent != null
        ? ammoComponent.GetShotsAvailable(activeWeapon.burstCount) // Gets available bullets (can be lesser than burstCount)
        : activeWeapon.burstCount; // enemies have no ammoComponent

        for (int i = 0; i < shotsRemaining; i++) {
            FireOnce();
            // Burst delay - delay between bullets in burst; no delay for last bullet
            if (i < shotsRemaining - 1)
                yield return new WaitForSeconds(activeWeapon.burstDelay);
        }
        if (ammoComponent != null && ammoComponent.IsMagazineEmpty)
            ammoComponent.TriggerReload();
        isBursting = false;
        burstCoroutine = null;
    }

    private void Update() {
        if (cooldownTimer > 0f) cooldownTimer -= Time.deltaTime;

        // Auto-firing: poll TryFire each frame while the trigger is held
        if (input != null && input.IsFiring && activeWeapon.shootingMode == ShootingMode.Auto)
            TryFire();
    }
}
