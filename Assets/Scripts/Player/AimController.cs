using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimController : MonoBehaviour
{

    private Transform playerTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
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
    }
}
