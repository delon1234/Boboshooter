using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PermanentShopUI : MonoBehaviour
{
    [SerializeField] private Transform contentParent;
    [SerializeField] private PermanentUpgradeEntryUI entryPrefab;
    [SerializeField] private PermanentUpgradeDefinition[] definitions; // Helps to Order the definitions in the Shop
    [SerializeField] private TMP_Text metaCurrencyText;

    private void Start()
    {
        PopulateShop();
    }

    private void Awake()
    {
        RefreshCurrency();
    }
    
    public void ReturnToMainMenu()
    {
        SceneLoader.LoadMainMenu();
    }

    // Spawns an entry into the Scene using order of the definitions serialized
    // Each entry will have its own PermanentShopEntryUI, and be further edited within each entry
    private void PopulateShop()
    {
        foreach (PermanentUpgradeDefinition definition in definitions)
        {
            PermanentUpgradeEntryUI entry = Instantiate(entryPrefab, contentParent);
            entry.name = definition.DisplayName;
            entry.Initialize(definition);
        }
    }

    // No args, just refresh
    private void RefreshCurrency()
    {
        metaCurrencyText.text = MetaData.MetaCoins.ToString();
    }

    // Have args, refresh. This is future considerations to animate the amt being taken away instead of a static refresh
    private void UpdateCurrency(OnMetaCoinsChangedArgs args)
    {
        RefreshCurrency();
    }

    private void OnEnable()
    {
        MetaData.OnMetaCoinsChanged += UpdateCurrency;
    }

    private void OnDisable()
    {
        MetaData.OnMetaCoinsChanged -= UpdateCurrency;
    }
}