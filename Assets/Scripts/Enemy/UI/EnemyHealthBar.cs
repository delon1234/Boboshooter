using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Attached to Prefab Enemy UI HealthBar
// Adjusts the Red Fill of the Health Bar visually
public class EnemyHealthBar : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image glowImage; // Glow outline/border image showing invulnerability

    [Header("Color Settings")]
    [SerializeField] private Color normalFillColor = Color.red;
    [SerializeField] private Color invulnerableFillColor = new Color(0.6f, 0.85f, 1f); // Metallic silver-blue glow

    [Header("Glow Animation Settings")]
    [SerializeField] private float pulseSpeed = 5f;
    [SerializeField] private float minPulseAlpha = 0.3f;
    [SerializeField] private float maxPulseAlpha = 1f;

    private Coroutine glowPulseCoroutine;

    private void OnDisable()
    {
        // Clean up the coroutine when the health bar is disabled/object pooled
        if (glowPulseCoroutine != null)
        {
            StopCoroutine(glowPulseCoroutine);
            glowPulseCoroutine = null;
        }
    }

    public void SetHealth(HealthInfo healthInfo)
    {
        if (fillImage != null)
        {
            // Avoid division by zero
            fillImage.fillAmount = healthInfo.MaxHealth > 0 ? healthInfo.CurrentHealth / healthInfo.MaxHealth : 0f;
        }
        UpdateDisplay(healthInfo);
    }

    public void SetInvulnerable(bool isInvulnerable)
    {
        if (fillImage != null)
        {
            fillImage.color = isInvulnerable ? invulnerableFillColor : normalFillColor;
        }

        if (isInvulnerable)
        {
            if (glowImage != null)
            {
                glowImage.gameObject.SetActive(true);
                if (glowPulseCoroutine == null)
                {
                    glowPulseCoroutine = StartCoroutine(PulseGlow());
                }
            }
        }
        else
        {
            if (glowPulseCoroutine != null)
            {
                StopCoroutine(glowPulseCoroutine);
                glowPulseCoroutine = null;
            }
            if (glowImage != null)
            {
                glowImage.gameObject.SetActive(false);
            }
        }
    }

    private void UpdateDisplay(HealthInfo healthInfo)
    {
        if (healthText != null)
        {
            int current = Mathf.CeilToInt(healthInfo.CurrentHealth);
            int max = Mathf.CeilToInt(healthInfo.MaxHealth);
            healthText.text = $"{current} / {max}";
        }
    }

    private IEnumerator PulseGlow()
    {
        if (glowImage == null) yield break;

        Color color = glowImage.color;
        while (true)
        {
            float t = Mathf.PingPong(Time.time * pulseSpeed, 1f);
            color.a = Mathf.Lerp(minPulseAlpha, maxPulseAlpha, t);
            glowImage.color = color;
            yield return null;
        }
    }
}
