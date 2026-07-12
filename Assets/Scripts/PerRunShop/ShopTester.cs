using UnityEngine;

public class ShopTester : MonoBehaviour
{
    [SerializeField] private ShopPickupBehaviour shopPickup;
    [SerializeField] private ShopTable shopTable;

    private void Start()
    {
        ShopWeightEntry entry = WeightedRandom.Pick(shopTable.ShopWeightTable);
        shopPickup.Initialize(entry);
    }
}