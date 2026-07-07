using UnityEngine;

// will define Pickup Information via a ScriptableObject so that it can be configured through Unity Inspector under Assets/Data/Pickup
// this Pickup Info will be used by various system across, mainly Enemy Drops and Shop Items
[CreateAssetMenu(menuName = "Pickup Definition")]
public class PickupDefinition : ScriptableObject
{
    public PickupType Type;
    public Sprite Sprite;
    public int Amount = 1;
}