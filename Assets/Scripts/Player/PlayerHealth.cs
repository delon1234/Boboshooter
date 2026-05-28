using UnityEngine;

// Parent Class for all Enemies
public class PlayerHealth : MonoBehaviour
{
    [Header("Stats")]
    private float maxHealth = 100;
    private float currentHealth;


    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amt)
    {
        currentHealth -= amt;
        if (currentHealth <= 0f)
        {
            // You Lose
        }
        Debug.Log(currentHealth);
    }
}