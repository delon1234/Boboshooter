using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;
    private PlayerInputHandler _input;

    private void Awake()
    {
        _input = GetComponent<PlayerInputHandler>();
    }

    private void OnEnable() {
        _input.OnFirePressed += Shoot;
    }
    private void OnDisable()
    {
        _input.OnFirePressed -= Shoot;
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        Debug.Log("PlayerShooter: Shoot() called, bullet instantiated at " + shootPoint.position);
    }
}
