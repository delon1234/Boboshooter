using UnityEngine;

public class DebugUpgradeTester : MonoBehaviour
{
    public PlayerUpgrades playerUpgrades;
    public DamageUpgrade damageUpgrade;   // drag a ScriptableObject asset here

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
            playerUpgrades.AddStaticModifier(damageUpgrade);

        if (Input.GetKeyDown(KeyCode.Y))
            playerUpgrades.RemoveStaticModifier(damageUpgrade);
    }
}
