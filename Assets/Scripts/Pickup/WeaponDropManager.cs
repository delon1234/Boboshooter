using UnityEngine;

// Subscribes to Shooter.OnWeaponDropped and spawns a WeaponPickup object in the world carrying saved ammo
public class WeaponDropManager : MonoBehaviour
{
    [SerializeField] private WeaponPickupBehaviour weaponPickupPrefab;
    [SerializeField] private Vector3 spawnOffset = new Vector3(0.8f, 0f, 0f);

    private Shooter playerShooter;

    private void Start()
    {
        if (Player.Instance != null)
        {
            playerShooter = Player.Instance.Shooter;
        }
        else
        {
            playerShooter = FindFirstObjectByType<Shooter>();
        }

        if (playerShooter != null)
        {
            playerShooter.OnWeaponDropped += HandleWeaponDropped;
        }
    }

    private void OnDestroy()
    {
        if (playerShooter != null)
        {
            playerShooter.OnWeaponDropped -= HandleWeaponDropped;
        }
    }

    private void HandleWeaponDropped(DroppedWeaponInfo droppedInfo)
    {
        if (droppedInfo.Weapon == null || weaponPickupPrefab == null)
        {
            return;
        }

        Vector3 dropPosition = playerShooter.transform.position + spawnOffset;
        WeaponPickupBehaviour droppedPickup = Instantiate(weaponPickupPrefab, dropPosition, Quaternion.identity);
        droppedPickup.Initialize(droppedInfo.Weapon, droppedInfo.CurrentMagazine, droppedInfo.CurrentReserve);
    }
}
