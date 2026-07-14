using UnityEngine;

// Struct for spatial settings for BulletPatterns like Spiral, Radial etc
[System.Serializable]
public struct PatternSpatialSettings
{
    public float spreadAngle;
    public float angleStep;
    public float radiusOffset;
    public float speedMultiplier;
}

