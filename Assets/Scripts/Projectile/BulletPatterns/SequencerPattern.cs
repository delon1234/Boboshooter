using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "NewSequencerPattern", menuName = "Combat/Patterns/Sequencer")]
public class SequencerPattern : BulletPattern
{
    public BulletPattern[] patterns;
    private int currentIndex = 0;

    public override void Shoot(Transform shootPoint, WeaponStats weaponStats, ObjectPool<Bullet> pool)
    {
        // Select next pattern or choose randomly
        BulletPattern activePattern = patterns[currentIndex];
        activePattern.Shoot(shootPoint, weaponStats, pool);

        // Advance sequence
        currentIndex = (currentIndex + 1) % patterns.Length;
    }
}
