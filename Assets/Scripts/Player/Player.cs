using UnityEngine;

public class Player : MonoBehaviour
{
    /* Facade Pattern
     * It exposes references of Player components to the other scripts in a single, 
     * convenient location.
     */
    
    // Expose cached references to other scripts
    public PlayerHealth Health { get; private set; }
    public PlayerController Controller { get; private set; }
    public PlayerInputHandler Input { get; private set; }
    public PlayerAnimator Animator {  get; private set; }
    public PlayerShooter Shooter { get; private set; }
    public AimController AimController { get; private set; }

    // Cache singleton instance
    public static Player Instance { get; private set; }

    private void Awake()
    {
        // Caching Instance
        // Scripts should access singleton from Start() as Awake() execution order is arbitrary
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        // Cache the references once
        Health = GetComponent<PlayerHealth>();
        Controller = GetComponent<PlayerController>();
        Input = GetComponent<PlayerInputHandler>();
        Animator = GetComponentInChildren<PlayerAnimator>(); // PlayerAnimator in Sprite (child of Player)
        Shooter = GetComponent<PlayerShooter>();
        AimController = GetComponentInChildren<AimController>();
    }

    private void Start()
    {
        Health.OnDeath += HandleDeath;
    }

    private void OnDestroy()
    {
        if (Health != null)
        {
            Health.OnDeath -= HandleDeath;
        }
    }

    private void HandleDeath()
    {
        // 1. Disable inputs and controller movement
        if (Input != null) Input.enabled = false;
        if (Controller != null) Controller.enabled = false;

        // 2. Disable shooting and aiming
        if (Shooter != null) Shooter.enabled = false;
        if (AimController != null) AimController.enabled = false;

        // 3. Stop physics movement and make kinematic
        if (TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.linearVelocity = Vector2.zero; // Stops movement
            rb.bodyType = RigidbodyType2D.Kinematic; // Disables physics from affecting corpse
        }

        // 4. Disable collision so enemies and projectiles pass through the corpse
        if (TryGetComponent<Collider2D>(out var col))
        {
            col.enabled = false;
        }
    }

    // Commands to expose
    public void Heal(float amount)
    {
        Health.Heal(amount);
    }
    public void HealFully() // For Level Ascension
    {
        Health.HealFully();
    }
    public void UpgradeMaxHealth(float amount)
    {
        Health.UpgradeMaxHealth(amount);
    }

}
