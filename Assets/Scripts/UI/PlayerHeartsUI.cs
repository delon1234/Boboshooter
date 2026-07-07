using UnityEngine;
using System.Collections.Generic;

public class PlayerHeartsUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject heartPrefab; // Prefab for the heart UI element
    private IHealth playerHealth; // Reference to the PlayerHealth component
    List<SingleHeartUI> hearts = new List<SingleHeartUI>(); // List to hold references to the heart UI elements
    private void Start()
    {
        SubscribeToPlayerHealth(); // Player.Instance is initialized as Start() executes after all Awake()
    }

    private void OnEnable()
    {
        SubscribeToPlayerHealth();
    }

    private void OnDisable()
    {
        UnsubscribeFromPlayerHealth();
    }

    private void SubscribeToPlayerHealth()
    {
        // If Player.Instance is not yet initialized PlayerHeartsUI.OnEnable() executes before Player.Awake(), exit to prevent nullreference
        if (Player.Instance == null) return;
        playerHealth = Player.Instance.Health;
        // Idempotent subscription prevents duplicate subscription when OnEnable() -> Start().
        playerHealth.OnHealthChange -= UpdateHearts;
        playerHealth.OnHealthChange += UpdateHearts;
        UpdateHearts(new HealthInfo(playerHealth.CurrentHealth, playerHealth.MaxHealth));   
    }

    private void UnsubscribeFromPlayerHealth()
    {
        playerHealth.OnHealthChange -= UpdateHearts;
    }

    private void UpdateHearts(HealthInfo healthInfo)
    {
        float currentHealth = healthInfo.CurrentHealth;
        float maxHealth = healthInfo.MaxHealth;
        int maxHealthInt = Mathf.CeilToInt(maxHealth); // Round up maxHealth to the ceiling integer for heart count
        // 1. Add/Remove SingleHeartUI instances to match maxHealthInt 
        AdjustMaxHearts(maxHealthInt);
        // 2. Update each SingleHeartUI sprite to reflect the current health state (Full/Half/Empty)
        UpdateHeartSprites(currentHealth);
    }

    private void CreateHeart(int numOfHearts)
    {
        for (int i = 0; i < numOfHearts; i++)
        {
            // Instantiate a new heart prefab and add it to the hearts list
            GameObject newHeart = Instantiate(heartPrefab, transform);
            newHeart.transform.SetParent(transform); // Set the parent of the new heart to PlayerHeartsUI
            SingleHeartUI singleHeartUI = newHeart.GetComponent<SingleHeartUI>();
            hearts.Add(singleHeartUI);
        }
    }

    private void AdjustMaxHearts(int maxHeartsInt)
    {
        // Future extension for maxHealth is increased/decreased (e.g. upgrades like health pickup)
        // Expensive as create/destroy GameObject; minimize by avoiding to clear all hearts whenever OnHealthChange is called
        if (hearts.Count == maxHeartsInt) { return; }
        // If list is too small (1. list is empty, 2. maxHearts is increased), create new hearts to match maxHearts
        if (hearts.Count < maxHeartsInt)
        {
            CreateHeart(maxHeartsInt - hearts.Count);
        }
        // If maxHearts is decreased (list is too large)
        for (int i = hearts.Count - 1; i >= maxHeartsInt; i--)
        {
            Destroy(hearts[i].gameObject); // Destroy the excess heart GameObject
            hearts.RemoveAt(i); // Remove the reference from the list
        }
    }

    private void UpdateHeartSprites(float currentHealth)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (currentHealth >= i + 1)
            {
                hearts[i].SetState(HeartState.Full);
            }
            else if (currentHealth >= i + 0.5f)
            {
                hearts[i].SetState(HeartState.Half);
            }
            else
            {
                hearts[i].SetState(HeartState.Empty);
            }
        }
    }
}
