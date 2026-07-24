using System.Collections.Generic;

// Resolves queries regarding Type -> Effect. This provides the link between Type -> Definition (containing information on EffectValues)
public static class MetaDataLookup
{
    private static Dictionary<PermanentUpgradeType, PermanentUpgradeDefinition> definitions = new();

    // Links Type -> Definition information
    // Currently registered by PermanentShopUI, acting as the Single Source of Truth for what Upgrades and Definition the game has
    public static void RegisterDefinitions(PermanentUpgradeDefinition[] upgradeDefinitions)
    {
        definitions.Clear();
        foreach (PermanentUpgradeDefinition definition in upgradeDefinitions)
        {
            definitions[definition.Type] = definition;
        }
    }


    public static float GetEffectValueByType(PermanentUpgradeType type)
    {
        // Query for Definition Information
        if (!definitions.TryGetValue(type, out PermanentUpgradeDefinition definition))
        {
            return 0;
        }
        // Query for Level Information
        int level = MetaData.GetUpgradeLevel(definition);
        return definition.GetCurrentEffectValue(level);
    }
}