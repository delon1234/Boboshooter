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
        if (registry == null)
        {
            return 0;
        }

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

    // SOLELY FOR TEST CASES, dont want MetaData progression to affect test cases
    public static void Clear()
    {
        registry = null;
    }
}