using UnityEngine;

// Attached to every Enemy
// Responsible for the HealthBar UI updates, triggered from enemy damaged
public class EnemyHealthBarBinder : MonoBehaviour
{
    [SerializeField] private BasicEnemy enemy;
    [SerializeField] private EnemyHealthBar healthBar;

    private void OnEnable()
    {
        enemy.OnEnemyHealthChanged += HandleHealthChanged;
    }

    private void OnDisable()
    {
        enemy.OnEnemyHealthChanged -= HandleHealthChanged;
    }

    private void HandleHealthChanged(OnEnemyHealthChangedArgs args)
    {
        healthBar.SetHealth(args);
    }
}