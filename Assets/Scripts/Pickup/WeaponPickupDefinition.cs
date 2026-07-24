using UnityEngine;

// PickupDefinition subclass specifically for weapon drops
[CreateAssetMenu(fileName = "NewWeaponPickupDefinition", menuName = "Pickup Definition/Weapon Pickup")]
public class WeaponPickupDefinition : PickupDefinition
{
    public WeaponData weaponData;

    private void OnValidate()
    {
        Type = PickupType.Weapon;
        if (weaponData != null && Sprite == null)
        {
            Sprite = weaponData.weaponSprite;
        }
    }
}
