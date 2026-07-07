using UnityEngine;

// Defines how the Prefab Pickup behaves by Definitions stored in Assets/Data/Pickup for Sprite and Amount
// Behaviour will be defined under Collect()
public class PickupBehaviour : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private PickupDefinition definition;

    public void Initialize(PickupDefinition definition)
    {
        this.definition = definition;
        spriteRenderer.sprite = definition.Sprite;
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

    // all choices for the Pickup behaviour
    private void Collect(GameObject player)
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

        Destroy(gameObject);
    }
}