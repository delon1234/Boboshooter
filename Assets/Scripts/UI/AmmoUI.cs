using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Image ammoBarFill;

    [Header("Visual Settings")]
    [SerializeField] private Color normalBarColor = Color.white;
    [SerializeField] private Color reloadingBarColor = new Color(1f, 0.75f, 0f); // Amber / orange-yellow for reloading

    private AmmoComponent playerAmmo;
    private Coroutine reloadCoroutine;
    private bool isSubscribed;

    private void Start()
    {
        SubscribeToPlayerAmmo();
    }

    private void OnEnable()
    {
        SubscribeToPlayerAmmo();
    }

    private void OnDisable()
    {
        UnsubscribeFromPlayerAmmo();
    }

    private void SubscribeToPlayerAmmo()
    {
        if (isSubscribed) return;
        if (Player.Instance == null || Player.Instance.Ammo == null) return;

        playerAmmo = Player.Instance.Ammo;

        playerAmmo.OnAmmoChanged += HandleAmmoChanged;
        playerAmmo.OnReloadStarted += HandleReloadStarted;
        playerAmmo.OnReloadFinished += HandleReloadFinished;
        playerAmmo.OnReloadCancelled += HandleReloadCancelled;

        isSubscribed = true;

        // Initialize display immediately
        UpdateDisplay(playerAmmo.CurrentAmmo);
    }

    private void UnsubscribeFromPlayerAmmo()
    {
        if (!isSubscribed) return;

        if (playerAmmo != null)
        {
            playerAmmo.OnAmmoChanged -= HandleAmmoChanged;
            playerAmmo.OnReloadStarted -= HandleReloadStarted;
            playerAmmo.OnReloadFinished -= HandleReloadFinished;
            playerAmmo.OnReloadCancelled -= HandleReloadCancelled;
        }

        isSubscribed = false;
    }

    private void HandleAmmoChanged(AmmoInfo ammo)
    {
        // Only update automatically if we are not currently running a reload animation
        if (reloadCoroutine == null)
        {
            UpdateDisplay(ammo);
        }
    }

    private void HandleReloadStarted()
    {
        if (reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
        }
        reloadCoroutine = StartCoroutine(AnimateReload());
    }

    private void HandleReloadFinished()
    {
        StopReloadAnimation();
        UpdateDisplay(playerAmmo.CurrentAmmo);
    }

    private void HandleReloadCancelled()
    {
        StopReloadAnimation();
        UpdateDisplay(playerAmmo.CurrentAmmo);
    }

    private void StopReloadAnimation()
    {
        if (reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
            reloadCoroutine = null;
        }
    }

    private void UpdateDisplay(AmmoInfo ammo)
    {
        // Update Ammo Text (e.g., "30 / 90" or "30 / ∞")
        if (ammoText != null)
        {
            string reserveStr = ammo.IsInfiniteReserve ? "∞" : ammo.CurrentReserve.ToString();
            ammoText.text = $"{ammo.CurrentMagazine} / {reserveStr}";
        }

        // Update Ammo Bar Fill
        if (ammoBarFill != null)
        {
            ammoBarFill.color = normalBarColor;
            float fillRatio = ammo.MaxMagazine > 0 ? (float)ammo.CurrentMagazine / ammo.MaxMagazine : 0f;
            ammoBarFill.fillAmount = fillRatio;
        }
    }

    private IEnumerator AnimateReload()
    {
        float reloadDuration = playerAmmo.ReloadTime;
        float elapsed = 0f;

        if (ammoBarFill != null)
        {
            ammoBarFill.color = reloadingBarColor;
            ammoBarFill.fillAmount = 0f;
        }

        if (ammoText != null)
        {
            ammoText.text = "RELOADING";
        }

        while (elapsed < reloadDuration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / reloadDuration);

            if (ammoBarFill != null)
            {
                ammoBarFill.fillAmount = progress;
            }

            yield return null;
        }

        if (ammoBarFill != null)
        {
            ammoBarFill.fillAmount = 1f;
        }

        reloadCoroutine = null;
    }
}
