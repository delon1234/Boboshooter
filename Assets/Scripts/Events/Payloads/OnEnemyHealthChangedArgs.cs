using UnityEngine;

// Information passed around when enemy takes damage
public class OnEnemyHealthChangedArgs
{
    public float amt;
    public float currentHealth;
    public float maxHealth;

    public OnEnemyHealthChangedArgs(float amt, float currentHealth, float maxHealth)
    {
        this.amt = amt;
        this.currentHealth = currentHealth;
        this.maxHealth = maxHealth;
    }
}
