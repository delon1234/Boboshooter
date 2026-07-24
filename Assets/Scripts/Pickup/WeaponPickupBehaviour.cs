using UnityEngine;

// Specialized pickup for weapons placed in scenes or dropped in the world
public class WeaponPickupBehaviour : BasePickupBehaviour
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private int currentMagazine = -1;
    [SerializeField] private int currentReserve = -1;
    [SerializeField] private float pickupCooldown = 0.8f;

    private float spawnTime;

    public WeaponData WeaponData => weaponData;
    public int CurrentMagazine => currentMagazine;
    public int CurrentReserve => currentReserve;

    protected override void Awake()
    {
        base.Awake();
        spawnTime = Time.time;
    }

    public void Initialize(WeaponData weapon, int currentMag = -1, int savedReserve = -1, PickupDefinition pickupDef = null)
    {
        weaponData = weapon;
        currentMagazine = currentMag;
        currentReserve = savedReserve;  // was: currentReserve = currentReserve (self-assignment bug!)
        spawnTime = Time.time;

        if (pickupDef != null)
        {
            InitializePickup(pickupDef);
        }
        else if (weaponData != null)
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }

            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = weaponData.weaponSprite;
            }
        }
    }

    private void TryCollect(Collider2D other)
    {
        // Ignore trigger while cooldown is active to prevent immediate re-pickup on drop
        if (Time.time < spawnTime + pickupCooldown)
        {
            return;
        }

        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (other.TryGetComponent<Shooter>(out var shooter) && weaponData != null)
        {
            shooter.EquipWeapon(weaponData, currentMagazine, currentReserve);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryCollect(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryCollect(other);
    }
}
