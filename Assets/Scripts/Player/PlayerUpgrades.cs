using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    /* Holds the player's upgrade inventory.
     * On upgrade collection or weapon swap:
     *   1. RecalculateStaticStats() folds all IStaticModifiers over the active weapon's base stats.
     *   2. Pushes the result to Shooter (cachedStaticStats) and AmmoComponent (capacity values).
     *
     * Static modifiers are recalculated once per upgrade/weapon-swap event (event-driven push).
     * Dynamic modifiers are evaluated per-shot inside Shooter.FireOnce() at runtime.
     */

    [Header("Modifier Lists")]
    public List<IStaticModifier> staticModifiers = new List<IStaticModifier>();
    public List<IDynamicModifier> dynamicModifiers = new List<IDynamicModifier>();

    // Cached references — populated in Awake via Player facade
    private Shooter shooter;
    private AmmoComponent ammoComponent;

    private void Awake()
    {
        // Use the Player singleton facade to avoid fragile GetComponent chains
        shooter = Player.Instance != null
            ? Player.Instance.Shooter
            : GetComponent<Shooter>();

        ammoComponent = Player.Instance != null
            ? Player.Instance.Ammo
            : GetComponent<AmmoComponent>();
    }

    private void Start()
    {
        // Subscribe in Start (not Awake) — Player.Instance and Shooter are guaranteed
        // to have finished their own Awake() by this point.
        if (shooter != null)
            shooter.OnWeaponEquipped += HandleWeaponEquipped;

        // Initial push: Awake's EquipWeapon fired before we subscribed, so we apply
        // modifiers now to cover the starting weapon. Treat this as a weapon swap so
        // AmmoComponent initialises current ammo to full capacity.
        RecalculateStaticStats(isWeaponSwap: true);
    }

    private void OnDestroy()
    {
        if (shooter != null)
            shooter.OnWeaponEquipped -= HandleWeaponEquipped;
    }

    private void HandleWeaponEquipped(WeaponData _)
    {
        // Weapon changed — recompute modifiers on top of the new weapon's base stats.
        // isWeaponSwap = true so AmmoComponent does a full reset (SyncFromStats).
        RecalculateStaticStats(isWeaponSwap: true);
    }

    /// <summary>
    /// Folds all static modifiers over the active weapon's base stats and pushes
    /// the resulting WeaponStats to Shooter and AmmoComponent.
    /// </summary>
    /// <param name="isWeaponSwap">
    /// True when triggered by a weapon equip — AmmoComponent resets current ammo to full capacity.
    /// False (default) when triggered by an upgrade — only the caps and reload time are updated,
    /// preserving the player's current ammo counts.
    /// </param>
    public void RecalculateStaticStats(bool isWeaponSwap = false)
    {
        if (shooter == null || shooter.ActiveWeapon == null)
        {
            Debug.LogWarning("[PlayerUpgrades] RecalculateStaticStats called but Shooter or ActiveWeapon is null.");
            return;
        }

        // Start from the weapon's unmodified base stats
        WeaponStats stats = shooter.ActiveWeapon.baseStats;

        // Fold each static modifier in order
        foreach (IStaticModifier mod in staticModifiers)
            stats = mod.ModifyStaticStats(stats);

        // Push the computed stats downstream
        shooter.UpdateBaseStats(stats);

        // Weapon swap: full reset (current ammo → max capacity)
        // Upgrade applied: cap-only update (current ammo preserved)
        if (isWeaponSwap)
            ammoComponent?.SyncFromStats(stats);
        else
            ammoComponent?.UpdateCaps(stats);
    }

    // ── Static modifier management ────────────────────────────────────────────

    public void AddStaticModifier(IStaticModifier modifier)
    {
        staticModifiers.Add(modifier);
        RecalculateStaticStats();
    }

    public void RemoveStaticModifier(IStaticModifier modifier)
    {
        if (staticModifiers.Remove(modifier))
            RecalculateStaticStats();
    }

    // ── Dynamic modifier management ───────────────────────────────────────────
    // PlayerUpgrades acts as the single registration point; Shooter stores the
    // live list it evaluates per-shot.

    public void AddDynamicModifier(IDynamicModifier modifier)
    {
        dynamicModifiers.Add(modifier);
        shooter?.AddDynamicModifier(modifier);
    }

    public void RemoveDynamicModifier(IDynamicModifier modifier)
    {
        dynamicModifiers.Remove(modifier);
        shooter?.RemoveDynamicModifier(modifier);
    }
}