using UnityEngine;

// Struct for spatial settings for BulletPatterns like Spiral, Radial etc
[System.Serializable]
public struct PatternSpatialSettings
{
    public float spreadAngle; // Total arc angle of spread
    public float angleStep; // Offset between adjacent bullets based on steps
    public float radiusOffset; // Distance from shootPoint where bullets are spawned
    public float speedMultiplier;
}

