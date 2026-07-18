using TMPro;
using UnityEngine;

// Pickups initialized by Shop Room, will check whether player has enough currency first
public class ShopPickupBehaviour : BasePickupBehaviour
{
    [SerializeField] private TMP_Text priceText;
    private ShopWeightEntry shopWeightEntry;

    public void Initialize(ShopWeightEntry shopWeightEntry)
    {
        this.shopWeightEntry = shopWeightEntry;

        InitializePickup(shopWeightEntry.definition);
        priceText.text = shopWeightEntry.cost.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Player Entered
        if (!other.CompareTag("Player"))
        {
            return;
        }
        // Shop Item will check whether Player has enough Coins, if Yes, then spend and returned True
        if (!RunData.SpendCoins(shopWeightEntry.cost))
        {
            Debug.Log("Player not have enough coins");
            return;
        }
        Collect(other.gameObject);
    }
}