using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimController : MonoBehaviour
{
    /*
    AimController is attached to RotatePoint, which rotates the object towards mouse
    RotatePoint resembles a player's hand position (positioned at shoulder level)
    */
    private Transform playerTransform;
    private SpriteRenderer weaponSprite;
    private float basePivotOffsetX;
    private Shooter shooter;

    [SerializeField] private GameObject weapon;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransform = transform.root; // RotatePoint is child of Player
        weaponSprite = weapon.GetComponent<SpriteRenderer>();
        shooter = playerTransform.GetComponent<Shooter>();

        // Read baseline hand offset directly from RotatePoint's transform X position set in Inspector
        basePivotOffsetX = Mathf.Abs(transform.localPosition.x);

        // Auto-detect playerSpriteRenderer if not set in Inspector
        if (playerSpriteRenderer == null && playerTransform != null)
        {
            playerSpriteRenderer = playerTransform.GetComponentInChildren<SpriteRenderer>();
        }
        
        // Auto-detect shootPoint if not assigned in Inspector
        if (shootPoint == null)
        {
            shootPoint = transform.Find("ShootPoint");
            if (shootPoint == null)
            {
                shootPoint = GetComponentInChildren<Transform>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        RotateTowardsMouse();
    }

    void RotateTowardsMouse()
    {
        // Standard 2D Top-Down Aiming used in most games:
        // Rotate the weapon pivot (transform) directly towards the mouse world position.
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(screenPos);

        // 1. Get Direction Vector from the weapon pivot to the mouse
        Vector2 direction = (Vector2)(mouseWorld - transform.position);
        
        // 2. Calculate angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // 3. Rotate weapon around Z axis
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // 4. Flip weapon sprite vertically when facing left so it isn't upside down
        bool isFacingLeft = angle > 90f || angle < -90f;
        weaponSprite.flipY = isFacingLeft;

        // 5. Adjust weapon sprite and shootPoint local Y positions when flipped to keep barrel aligned
        /// </summary>
        /// 1. When you rotate a gun 180° to aim left, the gun picture naturally turns upside-down.
        /// 2. To make the gun look right side up, we turned on flipY = true (Mirrored upside down)
        /// 3. Barrel becomes bottom half of image, while ShootPoint remains at same position
        /// - Adjust weapon's y offset to opposite direction via a negative
        if (shooter != null && shooter.ActiveWeapon != null)
        {
            Vector2 wOffset = shooter.ActiveWeapon.weaponSpriteOffset;
            weapon.transform.localPosition = isFacingLeft ? new Vector3(wOffset.x, -wOffset.y, 0f) : (Vector3)wOffset;
        }

        // 6. Flip player sprite horizontally when mouse is to the left of the player
        bool isMouseOnLeft = mouseWorld.x < playerTransform.position.x;
        if (playerSpriteRenderer != null)
        {
            playerSpriteRenderer.flipX = isMouseOnLeft;
        }

        // 7. Mirror RotatePoint's X position based on mouse side using Inspector baseline offset
        float handOffset = isMouseOnLeft ? -basePivotOffsetX : basePivotOffsetX;
        transform.localPosition = new Vector3(handOffset, transform.localPosition.y, transform.localPosition.z);
    }
}
