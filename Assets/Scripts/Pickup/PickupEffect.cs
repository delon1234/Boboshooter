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
        }
    }
}