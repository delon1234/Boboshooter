// Resolves queries regarding Type -> Effect. This provides the link between Type -> Definition (containing information on EffectValues)
public static class MetaDataLookup
{
    private static PermanentUpgradeRegistry registry;

    // Links Type -> Definition information
    // Currently registered by PermanentShopUI, acting as the Single Source of Truth for what Upgrades and Definition the game has
    public static void RegisterRegistry(PermanentUpgradeRegistry registry)
    {
        MetaDataLookup.registry = registry;
    }

    public static float GetEffectValueByType(PermanentUpgradeType type)
    {
        // Query for Definition Information
        PermanentUpgradeDefinition definition = registry.GetDefinition(type);
        if (!definition)
        {
            return 0;
        }
        // Query for Level Information
        int level = MetaData.GetUpgradeLevel(definition);
        return definition.GetCurrentEffectValue(level);
    }
}