using UnityEngine;

// Pickups readily available for collect, spawned by enemies
// Defines how the Prefab Pickup Collision behaves by Definitions stored in Assets/Data/Pickup for Sprite and Amount
public class PickupBehaviour : BasePickupBehaviour
{
    public void Initialize(PickupDefinition definition)
    {
        InitializePickup(definition);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Player Entered
        if (!other.CompareTag("Player"))
        {
            return;
        }
        Collect(other.gameObject);
    }
}