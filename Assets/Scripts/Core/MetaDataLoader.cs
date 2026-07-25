using UnityEngine;

// The sole purpose of this class is to Register the permanent upgrades data to MetaDataLookup on Awake
// Will be used for Player Shooter features
// In the future, ideally a PlayerData class should be made and ensured that code workflow is clean
public class MetaDataLoader : MonoBehaviour
{
    [SerializeField] private PermanentUpgradeRegistry registry;

    private void Awake()
    {
        MetaDataLookup.RegisterRegistry(registry);
    }
}