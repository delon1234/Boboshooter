using UnityEngine;

// Attached to every Enemy
// Responsible for the HealthBar UI updates, triggered from enemy damaged
public class EnemyHealthBarBinder : MonoBehaviour
{
    [SerializeField] private HealthComponent health;
    [SerializeField] private EnemyHealthBar healthBar;

    private void OnEnable()
    {
        health.OnHealthChange += HandleHealthChanged;
    }

    private void OnDisable()
    {
        health.OnHealthChange -= HandleHealthChanged;
    }

    private void HandleHealthChanged(HealthInfo healthInfo)
    {
        healthBar.SetHealth(healthInfo);
    }
}