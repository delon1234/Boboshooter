using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;

    private void Start()
    {
        UpdateCoins(new OnCoinsChangedArgs(0, RunData.Coins));
    }

    private void UpdateCoins(OnCoinsChangedArgs args)
    {
        coinText.text = args.Total.ToString();
    }

    // Subscribe to Coin Event
    private void OnEnable()
    {
        RunData.OnCoinsChanged += UpdateCoins;
    }

    private void OnDisable()
    {
        RunData.OnCoinsChanged -= UpdateCoins;
    }
}