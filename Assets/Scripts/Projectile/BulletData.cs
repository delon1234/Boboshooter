using UnityEngine;

[CreateAssetMenu(fileName = "BulletData", menuName = "Scriptable Objects/BulletData")]
public class BulletData : ScriptableObject
{
    public float damage = 1f;
    public float speed = 10f;
    public float lifetime = 3f; // Lifetime of the bullet in seconds before it gets destroyed
    public Sprite sprite;
}
