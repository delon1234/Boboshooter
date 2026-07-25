using UnityEngine;

// Helps Serializes all the needed references for a full System Test of Perm Upgrades <-> Player Upgrades
// Integration Tests aims to keep the code flow as true to the Player experience as possible, therefore using actual Game assets for testing
public class PermanentUpgradeIntegrationReferences : MonoBehaviour
{
    public GameObject playerPrefab;
    public PermanentUpgradeRegistry registry;
    public WeaponData starterPistol;
}