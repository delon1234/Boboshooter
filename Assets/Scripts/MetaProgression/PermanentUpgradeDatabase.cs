using UnityEngine;

// Currently, 2 sources need this definition list
// MetaDataLookup needs to register all definitions to understand link between Type -> Definition and Level
// PermanentShopUI needs to Register too to know what to prefab
// This will serve as a Single Source of Truth DB Link
[CreateAssetMenu(menuName = "Permanent Upgrade Database")]
public class PermanentUpgradeDatabase : ScriptableObject
{
    public PermanentUpgradeDefinition[] Definitions;
}