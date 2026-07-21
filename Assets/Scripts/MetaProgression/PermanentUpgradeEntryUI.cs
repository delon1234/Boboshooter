using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PermanentUpgradeEntryUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text statsText;

    [SerializeField] private Button upgradeButton;
    [SerializeField] private TMP_Text upgradeButtonText;
    [SerializeField] private Button downgradeButton;
    [SerializeField] private TMP_Text downgradeButtonText;

    private PermanentUpgradeDefinition definition;

    // First population of information using the definition assigned to each prefab
    public void Initialize(PermanentUpgradeDefinition definition)
    {
        this.definition = definition;

        // static information
        icon.sprite = definition.Icon;
        nameText.text = definition.DisplayName;
        descriptionText.text = definition.DisplayDescription;

        Refresh();
    }

    // Dynamic Information
    // Updates Level, as well as Button Interactable
    private void Refresh()
    {
        int level = MetaData.GetUpgradeLevel(definition);
        levelText.text = $"{level} / {definition.MaxLevel}";

        statsText.text = definition.GetFormattedStatText(level);

        upgradeButtonText.text = $"Upgrade ({definition.GetUpgradeCostToNext(level)})";
        downgradeButtonText.text = $"Refund ({definition.GetRefundToPrevious(level)})";

        upgradeButton.interactable = !MetaData.IsMaxLevel(definition);
        downgradeButton.interactable = !MetaData.IsMinLevel(definition);
    }

    // "API" calls to MetaData.cs
    public void OnUpgradeClicked()
    {
        if (MetaData.Upgrade(definition))
        {
            SaveManager.Save();
            Refresh();
        }
    }

    public void OnDowngradeClicked()
    {
        if (MetaData.Downgrade(definition))
        {
            SaveManager.Save();
            Refresh();
        }
    }

    // Every Upgrade will listen to OnUpgradeChanged, need to ensure it is the right one first, then refresh
    private void OnUpgradeChanged(PermanentUpgradeDefinition definition)
    {
        if (this.definition.Type == definition.Type)
        {
            Refresh();
        }
    }

    private void OnEnable()
    {
        MetaData.OnPermanentUpgradesChanged += OnUpgradeChanged;
    }

    private void OnDisable()
    {
        MetaData.OnPermanentUpgradesChanged -= OnUpgradeChanged;
    }
}