using UnityEngine;

// Defines how the Prefab Pickup Collision behaves by Definitions stored in Assets/Data/Pickup for Sprite and Amount
public class PickupBehaviour : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private PickupDefinition definition;

    // Assigns spriteRenderer the Component in itself
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

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
        PickupEffect.Apply(other.gameObject, definition);
        Destroy(gameObject);
    }
}