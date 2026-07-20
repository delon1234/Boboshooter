using UnityEngine;

// will define Permanent Upgrade Information via a ScriptableObject so that it can be configured through Unity Inspector under Assets/Data/PermanentUpgrade
// this is essentially a struct containing relevant information around one upgrade, and PermanentUpgradeState will link upgrades to player achieved level
[CreateAssetMenu(menuName = "Permanent Upgrade")]
public class PermanentUpgradeDefinition : ScriptableObject
{
    public PermanentUpgradeType Type;
    public Sprite Icon;
    public string DisplayName;
    public string DisplayDescription;

    public int[] LevelCosts;

    public float BaseValue;
    public float ValuePerLevel;
}