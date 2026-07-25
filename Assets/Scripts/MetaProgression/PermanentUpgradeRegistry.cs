using System.Collections.Generic;
using UnityEngine;

// Currently, 2 sources need this definition list
// MetaDataLookup needs to register all definitions to understand link between Type -> Definition and Level
// PermanentShopUI needs to Register too to know what to prefab
// This will serve as a Single Source of Truth Link
[CreateAssetMenu(menuName = "Permanent Upgrade Registry")]
public class PermanentUpgradeRegistry : ScriptableObject
{
    // Serialised
    public PermanentUpgradeDefinition[] Definitions;

    private Dictionary<PermanentUpgradeType, PermanentUpgradeDefinition> lookup;

    // PURELY FOR UNIT TEST, cannot serialise during Unit Test, therefore will use this to initialise Definitions
    public void SetDefinitions(PermanentUpgradeDefinition[] definitions)
    {
        this.Definitions = definitions;
        BuildLookup();
    }

    // sets up a Dictionary Lookup of Type -> Definition
    private void BuildLookup()
    {
        lookup = new Dictionary<PermanentUpgradeType, PermanentUpgradeDefinition>();
        foreach(var definition in Definitions)
        {
            lookup[definition.Type] = definition;
        }
    }

    // easily retrieve Defition given Type from this registry object
    public PermanentUpgradeDefinition GetDefinition(PermanentUpgradeType type)
    {
        if(lookup.TryGetValue(type, out var definition))
        {
            return definition;
        }
        Debug.LogError($"Missing Permanent Upgrade Definition: {type}");
        return null;
    }

    private void OnEnable()
    {
        // this happens inside Test Cases, where Registry is created in memory without definitions
        if (Definitions == null || Definitions.Length == 0)
        {
            return;
        }
        BuildLookup();
    }
}