using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimController : MonoBehaviour
{
    private Transform playerTransform;
    private SpriteRenderer weaponSprite;

    [SerializeField] private GameObject weapon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransform = transform.root; // RotatePoint is child of Player
        weaponSprite = weapon.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        RotateTowardsMouse();
    }

    void RotateTowardsMouse()
    {
        // Rotates ShootPoint to face the mouse cursor, allowing player to aim in any direction
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(screenPos);

        // 1. Get Direction Vector from player to mouse (Interested in direction only)
        Vector2 direction = (Vector2)(mouseWorld - playerTransform.position);
        // 2. Calculate angle from vector using atan2 and convert to degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // 3. Rotate around Z axis as 2D game (Rotates object in XY plane)
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // 4. Flip sprite vertically around y-axis
        // (Angle > 90 or < -90 means mouse is behind player, so flip sprite to face the other way)
        weaponSprite.flipY = angle > 90f || angle < -90f;
    }
}
