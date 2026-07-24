using UnityEngine;

public class RangedEnemy : BasicEnemy
{
    [Header("Shooter / Attack Settings")]
    [Tooltip("Transform from which bullets are spawned and aimed at the player.")]
    [SerializeField] private Transform shootPoint;

    [Tooltip("Maximum distance from player at which the enemy will attempt to fire.")]
    [SerializeField] private float attackRange = 8f;

    [Header("Ranged Spacing Settings")]
    [Tooltip("Target distance to maintain from the player.")]
    [SerializeField] private float shootingDistance = 5f;

    [Tooltip("Buffer zone around shooting distance to prevent direction jittering.")]
    [SerializeField] private float tolerance = 0.5f;

    [Tooltip("Speed multiplier when backpedaling so aggressive players can catch up.")]
    // Enemy retreat at a slower speed to allow players to catch up
    [SerializeField] private float backpedalSpeedMultiplier = 0.7f;

    [Header("Strafe (Circle Orbit) Settings")]
    [Tooltip("Weight of lateral movement relative to linear retreat/approach. Higher value = wider orbit.")]
    [SerializeField] private float strafeWeight = 0.4f;

    [Tooltip("Frequency in seconds to swap strafe direction (clockwise vs counter-clockwise). Set to 0 to disable.")]
    [SerializeField] private float strafeSwapInterval = 3f;

    private float strafeDirection = 1f; // +1 = Clockwise, -1 = Counter-Clockwise
    private float strafeTimer;
    private Shooter shooter;

    protected override void Awake()
    {
        base.Awake();
        shooter = GetComponent<Shooter>();

        if (shootPoint == null)
        {
            Transform foundPoint = transform.Find("ShootPoint");
            shootPoint = foundPoint != null ? foundPoint : transform;
        }
    }

    protected override void Start()
    {
        base.Start();
        // Randomize initial strafe direction (clockwise vs counter-clockwise)
        strafeDirection = Random.value > 0.5f ? 1f : -1f;
        strafeTimer = strafeSwapInterval;
    }

    protected override void Update()
    {
        base.Update();

        if (isDead) return;

        // Periodically swap strafe direction for dynamic movement
        if (strafeSwapInterval > 0f)
        {
            strafeTimer -= Time.deltaTime;
            if (strafeTimer <= 0f)
            {
                strafeDirection *= -1f;
                strafeTimer = strafeSwapInterval;
            }
        }

        HandleShooting();
    }

    private void HandleShooting()
    {
        if (playerTransform == null || shooter == null) return;

        // Orient shootPoint towards player position so bullets fly towards the player
        Vector2 dir = GetDirectionToPlayer();
        if (dir != Vector2.zero && shootPoint != null)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            shootPoint.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        // Trigger shooting if within range (Shooter component handles fireRate cooldown internally)
        if (GetDistanceToPlayer() <= attackRange)
        {
            shooter.TryFire();
        }
    }

    // Range Movement Logic
    protected override void WalkLogic()
    {
        if (playerTransform == null) return;

        float distance = GetDistanceToPlayer();
        Vector2 toPlayer = GetDirectionToPlayer();
        
        // Perpendicular vector for sideways (lateral) movement
        Vector2 strafePerp = new Vector2(-toPlayer.y, toPlayer.x) * strafeDirection;

        Vector2 moveVector;

        if (distance < shootingDistance - tolerance)
        {
            // TOO CLOSE: Backpedal away while angling laterally (circle-strafe retreat)
            Vector2 retreatDir = (-toPlayer + strafePerp * strafeWeight).normalized;
            moveVector = retreatDir * (speed * backpedalSpeedMultiplier);
        }
        else if (distance > shootingDistance + tolerance)
        {
            // TOO FAR: Advance towards player with a slight lateral angle
            Vector2 advanceDir = (toPlayer + strafePerp * (strafeWeight * 0.5f)).normalized;
            moveVector = advanceDir * speed;
        }
        else
        {
            // SWEET SPOT (within tolerance window): Orbit around player at reduced speed
            moveVector = strafePerp * (speed * 0.5f * strafeWeight);
        }

        rb.linearVelocity = moveVector;
    }
}