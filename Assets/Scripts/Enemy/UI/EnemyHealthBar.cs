using UnityEngine;
using UnityEngine.UI;

// Attached to Prefab Enemy UI HealthBar
// Adjusts the Red Fill of the Health Bar visually
public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    public void SetHealth(HealthInfo healthInfo)
    {
        fillImage.fillAmount = healthInfo.CurrentHealth / healthInfo.MaxHealth;
    }
}
