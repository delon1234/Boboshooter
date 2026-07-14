using System.Collections.Generic;
using UnityEngine;

// Attached to ShopRooms
// Helps initialize ShopPickups into the GameScene via Serialized Positions
public class ShopRuntime : MonoBehaviour
{
    [SerializeField] private ShopTable shopTable;
    [SerializeField] private GameObject shopPickupPrefab;
    [SerializeField] private Transform pickupSpawnpointParent;
    private List<Transform> pickupSpawnpoints;

    private void Awake()
    {
        foreach (Transform child in pickupSpawnpointParent)
        {
            pickupSpawnpoints.Add(child);
        }            
    }

    private void Start()
    {
        foreach (Transform point in pickupSpawnpoints)
        {
            ShopWeightEntry selectedEntry = WeightedRandom.Pick(shopTable.ShopWeightTable);
            GameObject ShopPickupClone = Instantiate(shopPickupPrefab, point.position, Quaternion.identity, transform);
            ShopPickupClone.GetComponent<ShopPickupBehaviour>().Initialize(selectedEntry);
        }
    }
}