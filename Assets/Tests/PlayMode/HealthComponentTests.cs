using NUnit.Framework;
using UnityEngine;

public class HealthComponentTests
{
    private GameObject testGo;
    private HealthComponent healthComp;

    [SetUp]
    public void SetUp()
    {
        testGo = new GameObject("TestObject");
        healthComp = testGo.AddComponent<HealthComponent>();
        // Disable the auto-invulnerability feature from Start() during tests
        healthComp.testInvulnerable = false;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(testGo);
    }

    [Test]
    public void Initialization_SetsCurrentHealthToMaxHealth()
    {
        Assert.AreEqual(healthComp.MaxHealth, healthComp.CurrentHealth);
    }

    [Test]
    public void RestoreHealth_IncreasesCurrentHealth_UpToMaxHealth()
    {
        // 1. Apply some damage to lower the health below max
        var damageInfo = new DamageInfo(3f, null, Vector2.zero);
        healthComp.ApplyDamage(damageInfo);
        float healthAfterDamage = healthComp.CurrentHealth;

        // 2. Restore some health
        healthComp.RestoreHealth(2f);
        Assert.AreEqual(healthAfterDamage + 2f, healthComp.CurrentHealth);

        // 3. Restore more than the max health, it should clamp to MaxHealth
        healthComp.RestoreHealth(10f);
        Assert.AreEqual(healthComp.MaxHealth, healthComp.CurrentHealth);
    }

    [Test]
    public void RestoreHealth_WithNegativeOrZeroAmount_DoesNothing()
    {
        // 1. Lower health
        var damageInfo = new DamageInfo(2f, null, Vector2.zero);
        healthComp.ApplyDamage(damageInfo);
        float currentHealthBefore = healthComp.CurrentHealth;

        // 2. Try restoring 0 health
        healthComp.RestoreHealth(0f);
        Assert.AreEqual(currentHealthBefore, healthComp.CurrentHealth);

        // 3. Try restoring negative health
        healthComp.RestoreHealth(-1f);
        Assert.AreEqual(currentHealthBefore, healthComp.CurrentHealth);
    }

    [Test]
    public void IncreaseMaxHealth_WithoutProportionalHeal_IncreasesMaxHealthOnly()
    {
        float oldMax = healthComp.MaxHealth;
        
        // Lower health
        var damageInfo = new DamageInfo(2f, null, Vector2.zero);
        healthComp.ApplyDamage(damageInfo);
        float oldCurrent = healthComp.CurrentHealth;

        // Increase max health without proportional healing
        healthComp.IncreaseMaxHealth(5f, healProportionally: false);

        Assert.AreEqual(oldMax + 5f, healthComp.MaxHealth);
        Assert.AreEqual(oldCurrent, healthComp.CurrentHealth); // Current health remains unchanged
    }

    [Test]
    public void IncreaseMaxHealth_WithProportionalHeal_ScalesCurrentHealth()
    {
        float oldMax = healthComp.MaxHealth;
        
        // Lower health (e.g. from 5 to 3)
        var damageInfo = new DamageInfo(2f, null, Vector2.zero);
        healthComp.ApplyDamage(damageInfo);
        float oldCurrent = healthComp.CurrentHealth;

        // Expected proportional health after adding 5 max health (new max = 10): (3 / 5) * 10 = 6
        float expectedCurrent = (oldCurrent / oldMax) * (oldMax + 5f);

        // Increase max health with proportional healing
        healthComp.IncreaseMaxHealth(5f, healProportionally: true);

        Assert.AreEqual(oldMax + 5f, healthComp.MaxHealth);
        Assert.AreEqual(expectedCurrent, healthComp.CurrentHealth);
    }

    [Test]
    public void IncreaseMaxHealth_WithNegativeOrZeroAmount_DoesNothing()
    {
        float oldMax = healthComp.MaxHealth;
        float oldCurrent = healthComp.CurrentHealth;

        healthComp.IncreaseMaxHealth(0f, healProportionally: true);
        Assert.AreEqual(oldMax, healthComp.MaxHealth);
        Assert.AreEqual(oldCurrent, healthComp.CurrentHealth);

        healthComp.IncreaseMaxHealth(-5f, healProportionally: true);
        Assert.AreEqual(oldMax, healthComp.MaxHealth);
        Assert.AreEqual(oldCurrent, healthComp.CurrentHealth);
    }
}
