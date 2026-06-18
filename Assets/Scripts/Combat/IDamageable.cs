using UnityEngine;

public interface IDamageable
{
    /* IDamageable is an interface that defines the contract for any entity that can take damage.
     * Player, Enemy, and Destructible Objects like Boxes */
    void TakeDamage(DamageInfo damageInfo);
}
