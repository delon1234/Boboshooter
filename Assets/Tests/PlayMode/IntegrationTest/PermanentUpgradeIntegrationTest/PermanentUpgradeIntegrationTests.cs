using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        // forcibly load the specified Integration Testing Scene
        yield return SceneManager.LoadSceneAsync("PermanentUpgradeStatsTest");

        // finds the references object in the Test Scene
        references = Object.FindFirstObjectByType<PermanentUpgradeIntegrationReferences>();
        Assert.IsNotNull(references, "Missing PermanentUpgradeIntegrationReferences");

        // handles registration (normally done by MetaDataLoader.cs during Runtime)
        registry = references.registry;
        MetaDataLookup.RegisterRegistry(registry);

        starterPistol = references.starterPistol;

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
        // wait one frame to allow Start and Awake codes to run properly, finishing Setup
        yield return null;
    }

    // clear any instanced state of player / if it has been modified by previous tests, since going to re instantiate another Player always anyway
    [UnityTearDown]
    public IEnumerator TearDown()
    {
        if(playerObject != null)
        {
            Object.Destroy(playerObject);
        }
        yield return null;
    }

    // Currently level 1 Damage, should expect a 5% damage increase (in Damage Definition) ontop of base pistol
    [Test]
    public void PermanentUpgrade_Damage_AffectsWeaponStats()
    {
        PermanentUpgradeDefinition DamageDefinition = registry.GetDefinition(PermanentUpgradeType.Damage);
        Assert.AreEqual(1, MetaData.GetUpgradeLevel(DamageDefinition)); // ensures it is really at level 1

        float damageMultiplier = 1f + DamageDefinition.GetCurrentEffectValue(1) / 100;
        Assert.AreEqual(damageMultiplier * starterPistol.baseStats.damage, shooter.CurrentStats.damage, 0.001f);
    }

    // Currently level 1 FireRate, should expect a 5% firerate increase (in FireRate Definition) ontop of base pistol
    [Test]
    public void PermanentUpgrade_FireRate_AffectsWeaponStats()
    {
        PermanentUpgradeDefinition FireRateDefinition = registry.GetDefinition(PermanentUpgradeType.FireRate);
        Assert.AreEqual(1, MetaData.GetUpgradeLevel(FireRateDefinition)); // ensures it is really at level 1

        float fireRateMultiplier = 1f + FireRateDefinition.GetCurrentEffectValue(1) / 100;
        Assert.AreEqual(fireRateMultiplier * starterPistol.baseStats.fireRate, shooter.CurrentStats.fireRate, 0.001f);
    }

    // Tests whether the MaxHealth actually did increase MaxHealth
    [Test]
    public void PermanentUpgrade_MaxHealth_AffectsPlayerHealth()
    {
        PermanentUpgradeDefinition healthDefinition = registry.GetDefinition(PermanentUpgradeType.MaxHealth);
        Assert.AreEqual(1, MetaData.GetUpgradeLevel(healthDefinition)); // ensures it is really at level 1

        float healthBonus = healthDefinition.GetCurrentEffectValue(1);
        float expectedMaxHealth = healthBonus + 5f; // assuming base player HP is 5

        Assert.AreEqual(expectedMaxHealth, playerHealth.MaxHealth, 0.001f);
    }

    // Tests whether the Movement Speed upgrade actually did increase Movement Speed
    [Test]
    public void PermanentUpgrade_MoveSpeed_AffectsPlayerMovement()
    {
        PermanentUpgradeDefinition moveDefinition = registry.GetDefinition(PermanentUpgradeType.MoveSpeed);
        Assert.AreEqual(1, MetaData.GetUpgradeLevel(moveDefinition)); // ensures it is really at level 1

        float bonus = moveDefinition.GetCurrentEffectValue(1);
        float expected = playerController.movementSpeed + bonus;

        Assert.AreEqual(expected, playerController.persistentMovementSpeed, 0.001f);
    }
}