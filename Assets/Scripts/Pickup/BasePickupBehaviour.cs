using UnityEngine;

// Base class, will be inherited by PickupBehaviour (for enemy) and ShopPickupBehaviour (for shop)
// different OnTriggerEnter2D logic for both, this class serves as common logic for both
public abstract class BasePickupBehaviour : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    protected PickupDefinition definition;

    // Assigns spriteRenderer the Component in itself
    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void InitializePickup(PickupDefinition definition)
    {
        this.definition = definition;
        spriteRenderer.sprite = definition.Sprite;
    }

    protected void Collect(GameObject player)
    {
        PickupEffect.Apply(player, definition);
        Destroy(gameObject);
    }
}