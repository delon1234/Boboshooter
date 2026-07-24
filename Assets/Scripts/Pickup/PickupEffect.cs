using UnityEngine;

// Helper class to define what each Type of Pickup will do
// Call with Player and PickupDefinition information, then it will apply a function according to what Definition was picked up
// Will be used by Enemy Pickups and Shop Pickups
public static class PickupEffect
{
    public static void Apply(GameObject player, PickupDefinition definition)
    {
        switch (definition.Type)
        {
            case PickupType.Coin:
                RunData.AddCoins(definition.Amount);
                break;

            case PickupType.Heal:
                player.GetComponent<PlayerHealth>().Heal(definition.Amount);
                break;

            case PickupType.MaxHealth:
                player.GetComponent<PlayerHealth>().UpgradeMaxHealth(definition.Amount);
                break;

            case PickupType.Magazine:
                if (player.TryGetComponent<AmmoComponent>(out var ammo))
                {
                    ammo.RestoreMagazine(definition.Amount); // Restores 1 magazine size by default
                }
                break;
            case PickupType.AmmoCrate:
                if (player.TryGetComponent<AmmoComponent>(out var ammoComp))
                {
                    ammoComp.ReplenishMaxReserve(); // Fully replenishes reserves
                }
                break;
            case PickupType.Weapon:
                if (definition is WeaponPickupDefinition weaponDef && weaponDef.weaponData != null)
                {
                    if (player.TryGetComponent<Shooter>(out var shooter))
                    {
                        shooter.EquipWeapon(weaponDef.weaponData);
                    }
                }
                break;
        }
    }
}