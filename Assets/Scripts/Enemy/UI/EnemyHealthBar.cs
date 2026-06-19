using UnityEngine;
using UnityEngine.UI;

// Attached to Prefab Enemy UI HealthBar
// Adjusts the Red Fill of the Health Bar visually
public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    public void SetHealth(OnEnemyHealthChangedArgs args)
    {
        fillImage.fillAmount = args.currentHealth / args.maxHealth;
    }
}
