using System;
using System.Collections;
using UnityEngine;

public class AmmoComponent : MonoBehaviour
{
    /* Component to be attached to Player
    - Tracks magazine size, total ammo
    - Reload system
    */

    private int maxMagazineSize;
    private int maxReserveSize;
    private float reloadTime;

    private int currentMagazineSize;
    private int currentReserveSize;
    private bool isReloading;
    private Coroutine reloadCoroutine;

    public event Action<AmmoInfo> OnAmmoChanged;
    public event Action OnReloadStarted;
    public event Action OnReloadFinished;
    public event Action OnReloadCancelled; // future\
    // For implementation of starter weapon (Infinite reserve, limited clip + reload)
    public bool HasInfiniteReserves => maxReserveSize < 0;
    public bool IsReloading => isReloading;
    public bool IsMagazineEmpty => currentMagazineSize <= 0;
    public float ReloadTime => reloadTime;
    public AmmoInfo CurrentAmmo => new AmmoInfo(currentMagazineSize, maxMagazineSize, currentReserveSize, HasInfiniteReserves);

    private PlayerInputHandler input;

    private void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
    }

    private void OnEnable() {
        if (input != null)
        {
            input.OnReloadPressed += TriggerReload;
        }
    }

    private void OnDisable()
    {
        if (input != null)
        {
            input.OnReloadPressed -= TriggerReload;
        }
    }

    public void SyncFromStats(WeaponStats stats) {
        // Called when weapon is equipped or upgrades pickup that can affect ammo
        CancelReload();
        maxMagazineSize = stats.magazineSize;
        maxReserveSize = stats.ammoReserveCapacity;
        reloadTime = stats.reloadTime;
        currentMagazineSize = maxMagazineSize;
        currentReserveSize = maxReserveSize;
        OnAmmoChanged?.Invoke(CurrentAmmo);
    }

    // public bool CanFire() {
    //     return !isReloading && currentMagazineSize > 0;
    // }

    public int GetShotsAvailable(int requested) {
        // Refactored from Shooter as part of Burst to not violate TDA
        if (HasInfiniteReserves) return requested; // May remove as infinite reserves should still follow magazine limit
        return Mathf.Min(requested, currentMagazineSize);
    }

    public void ConsumeBullet() {
        currentMagazineSize--;
        AmmoInfo ammo = new AmmoInfo(currentMagazineSize, maxMagazineSize, currentReserveSize, HasInfiniteReserves);
        OnAmmoChanged?.Invoke(ammo);
        print(ammo);

        if (currentMagazineSize <= 0) {
            TriggerReload();
        }
    }

    public IEnumerator Reload() {
        isReloading = true;
        OnReloadStarted?.Invoke();
        yield return new WaitForSeconds(reloadTime);
        ReloadAmmo(); // Reload after delay
        isReloading = false;
        reloadCoroutine = null;
        OnReloadFinished?.Invoke();
    }

    public void TriggerReload() {
        // If reloading, exit early
        if (!isReloading && currentMagazineSize < maxMagazineSize) {
            if (!HasInfiniteReserves && currentReserveSize <= 0) {
                return; // Can't reload if we have no reserves left
            }
            reloadCoroutine = StartCoroutine(Reload());
        }
    }

    public void CancelReload() {
        if (isReloading && reloadCoroutine != null) {
            StopCoroutine(reloadCoroutine);
            isReloading = false;
            reloadCoroutine = null;
            OnReloadCancelled?.Invoke();
        }
    }

    public void AddReserveAmmo(int amount) {
        // Method is called by ammo pickup system to replenish reserves
        if (HasInfiniteReserves) return;
        currentReserveSize = Mathf.Min(currentReserveSize + amount, maxReserveSize);
        OnAmmoChanged?.Invoke(CurrentAmmo);
    }

    private void ReloadAmmo() {
        int needed = maxMagazineSize - currentMagazineSize;
        if (HasInfiniteReserves) {
            currentMagazineSize = maxMagazineSize;
            print($"Reload, set current magazine size to maxsize");
        }
        else {
            int toTransfer = Mathf.Min(needed, currentReserveSize); // Handles case when reserve < needed
            currentMagazineSize += toTransfer;
            currentReserveSize -= toTransfer;
            print($"Reloaded: {toTransfer}, Needed: {needed}");
        }
    }
}

public readonly struct AmmoInfo
{
    /* Ammo is a struct to pass ammo-related information to ammo UI 
     - Utilises Parameter Object Pattern to ensure scalability and maintainability as more properties can be added without changing method signatures
     - Readonly struct for immutability to prevent side effects when passing to other systems via events
     */

    /* Properties */
    // 1. Readonly properties for immutability
    public readonly int CurrentMagazine;
    public readonly int MaxMagazine;
    public readonly int CurrentReserve;   // add reserve for HUD
    public readonly bool IsInfiniteReserve; // lets HUD show "∞" symbol
    public AmmoInfo(int currentMag, int maxMag, int currentReserve, bool infiniteReserve = false)
    {
        CurrentMagazine    = currentMag;     
        MaxMagazine        = maxMag;
        CurrentReserve     = currentReserve;
        IsInfiniteReserve  = infiniteReserve;
    }
    public override string ToString() =>
        $"Ammo: {CurrentMagazine}/{MaxMagazine} | Reserve: {(IsInfiniteReserve ? "∞" : CurrentReserve.ToString())}";
}

