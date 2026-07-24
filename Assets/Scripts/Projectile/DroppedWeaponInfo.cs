using System;

// Container struct to hold weapon and ammo state for weapon drop events
[Serializable]
public readonly struct DroppedWeaponInfo
{
    public readonly WeaponData Weapon;
    public readonly int CurrentMagazine;
    public readonly int CurrentReserve;

    public DroppedWeaponInfo(WeaponData weapon, int currentMagazine, int currentReserve)
    {
        Weapon = weapon;
        CurrentMagazine = currentMagazine;
        CurrentReserve = currentReserve;
    }
}
