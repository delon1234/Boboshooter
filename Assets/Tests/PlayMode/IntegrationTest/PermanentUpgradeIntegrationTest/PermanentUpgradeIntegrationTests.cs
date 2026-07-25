using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

// Contains the actual Test code, after taking references from PermanentUpgradeIntegrationReferences
public class PermanentUpgradeIntegrationTests
{
    // unpack serialised information
    private PermanentUpgradeIntegrationReferences references;
    private PermanentUpgradeRegistry registry;
    private WeaponData starterPistol;

    private GameObject playerObject;
    private PlayerHealth playerHealth;
    private PlayerController playerController;
    private Shooter shooter;

    // Different from Unit Tests, requires UnitySetUp to ensure Unity Lifecycle flow works properly
    // Awake and Start is important during instantiation of Player for example
    // Many components in Player depends on Awake and Start proper flow
    [UnitySetUp]
    public IEnumerator Setup()
    {
        // finds the references object in the Test Scene
        references = Object.FindFirstObjectByType<PermanentUpgradeIntegrationReferences>();
        Assert.IsNotNull(references, "Missing PermanentUpgradeIntegrationReferences");

        // handles registration (normally done by MetaDataLoader.cs during Runtime)
        MetaDataLookup.RegisterRegistry(registry);

        // gives Player default upgrade data, all level 1
        Dictionary<PermanentUpgradeType, int> upgrades = new()
        {
            {
                PermanentUpgradeType.MaxHealth,
                1
            },

            {
                PermanentUpgradeType.MoveSpeed,
                1
            },

            {
                PermanentUpgradeType.Damage,
                1
            },

            {
                PermanentUpgradeType.FireRate,
                1
            }
        };

        // mimics a load from SaveData
        MetaData.LoadFromSave(new SaveData(9876, upgrades));

        // creates the Player as is in the GameScene. then saves component references for easy testing
        playerObject = Object.Instantiate(references.playerPrefab);
        playerHealth = playerObject.GetComponent<PlayerHealth>();
        playerController = playerObject.GetComponent<PlayerController>();
        shooter = playerObject.GetComponent<Shooter>();
        registry = references.registry;
        starterPistol = references.starterPistol;
        // wait one frame to allow Start and Awake codes to run properly, finishing Setup
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        if(playerObject != null)
        {
            Object.Destroy(playerObject);
        }
        yield return null;
    }

    // Currently level 1 Damage, should expect a 5% damage increase (in Damage Definition) ontop of 15 base damage pistol
    [Test]
    public void PermanentUpgrade_Damage_AffectsWeaponStats()
    {
        PermanentUpgradeDefinition DamageDefinition = registry.GetDefinition(PermanentUpgradeType.Damage);
        Assert.AreEqual(1, MetaData.GetUpgradeLevel(DamageDefinition)); // ensures it is really at level 1

        float damageMultiplier = 1f + DamageDefinition.GetCurrentEffectValue(1);
        Assert.AreEqual(damageMultiplier * starterPistol.baseStats.damage, shooter.CurrentStats.damage, 0.001f);
    }
}