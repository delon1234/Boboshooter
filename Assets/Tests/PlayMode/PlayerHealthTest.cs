using NUnit.Framework;
using UnityEngine;

public class PlayerHealthTests
{
    private GameObject testGo;
    private PlayerHealth playerHealth;
    private HealthComponent healthComp;

    [SetUp]
    public void SetUp()
    {
        testGo = new GameObject("PlayerObject");
        playerHealth = testGo.AddComponent<PlayerHealth>();
        healthComp = testGo.AddComponent<HealthComponent>();

        // Inject healthComponent using C# reflection since it is a private serialized field
        var field = typeof(PlayerHealth).GetField("healthComponent", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field.SetValue(playerHealth, healthComp);

        // Turn off invulnerability on start so it doesn't block taking damage during tests
        healthComp.testInvulnerable = false;
        playerHealth.onHitInvulnDuration = 0f; // Prevent taking damage from causing invulnerability in tests
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(testGo);
    }

    [Test]
    public void Heal_RestoresCurrentHealth()
    {
        // 1. Take damage to lower health
        playerHealth.TakeDamage(new DamageInfo(3f, null, Vector2.zero));
        float healthAfterDamage = playerHealth.CurrentHealth;

        // 2. Heal the player
        playerHealth.Heal(2f);
        Assert.AreEqual(healthAfterDamage + 2f, playerHealth.CurrentHealth);
    }

    [Test]
    public void HealFully_RestoresHealthToMax()
    {
        // 1. Take damage
        playerHealth.TakeDamage(new DamageInfo(4f, null, Vector2.zero));
        Assert.Less(playerHealth.CurrentHealth, playerHealth.MaxHealth);

        // 2. Heal fully
        playerHealth.HealFully();
        Assert.AreEqual(playerHealth.MaxHealth, playerHealth.CurrentHealth);
    }

    [Test]
    public void UpgradeMaxHealth_IncreasesMaxHealthAndHealsByUpgradeAmount()
    {
        // 1. Take damage (initial max is 5, take 3 damage -> current is 2)
        playerHealth.TakeDamage(new DamageInfo(3f, null, Vector2.zero));
        float oldMax = playerHealth.MaxHealth;
        float oldCurrent = playerHealth.CurrentHealth;

        // 2. Upgrade max health by 2 (should add 2 to max health and heal 2)
        playerHealth.UpgradeMaxHealth(2f);

        Assert.AreEqual(oldMax + 2f, playerHealth.MaxHealth);
        Assert.AreEqual(oldCurrent + 2f, playerHealth.CurrentHealth);
    }
}
