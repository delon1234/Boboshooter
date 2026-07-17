using UnityEngine;

// Attached to every Enemy
// Responsible for the HealthBar UI updates, triggered from enemy damaged
public class EnemyHealthBarBinder : MonoBehaviour
{
    private IHealth health;
    private IInvulnerable invulnerable;
    [SerializeField] private EnemyHealthBar healthBar;

    private void Awake()
    {
        health = GetComponent<IHealth>();
        invulnerable = GetComponent<IInvulnerable>();
    }

    private void OnEnable()
    {
        if (health != null)
        {
            health.OnHealthChange += HandleHealthChanged;
            HandleHealthChanged(new HealthInfo(health.CurrentHealth, health.MaxHealth));
        }

        if (invulnerable != null)
        {
            invulnerable.OnInvulnerabilityChanged += HandleInvulnerabilityChanged;
            HandleInvulnerabilityChanged(invulnerable.IsInvulnerable);
        }
        else
        {
            if (healthBar != null)
            {
                healthBar.SetInvulnerable(false);
            }
        }
    }

    private void OnDisable()
    {
        if (health != null)
        {
            health.OnHealthChange -= HandleHealthChanged;
        }

        if (invulnerable != null)
        {
            invulnerable.OnInvulnerabilityChanged -= HandleInvulnerabilityChanged;
        }
    }

    private void HandleHealthChanged(HealthInfo healthInfo)
    {
        if (healthBar != null)
        {
            healthBar.SetHealth(healthInfo);
        }
    }

    private void HandleInvulnerabilityChanged(bool isInvulnerable)
    {
        if (healthBar != null)
        {
            healthBar.SetInvulnerable(isInvulnerable);
        }
    }
}