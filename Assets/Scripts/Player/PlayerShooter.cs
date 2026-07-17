using UnityEngine;

// public class PlayerShooter : MonoBehaviour
// {
//     [SerializeField] private GameObject bulletPrefab;
//     [SerializeField] private Transform shootPoint;
//     private PlayerInputHandler input;

//     private void Awake()
//     {
//         input = GetComponent<PlayerInputHandler>();
//     }

//     private void OnEnable() {
//         if (input != null)
//         {
//             input.OnFirePressed += Shoot;
//         }
//     }

//     private void OnDisable()
//     {
//         if (input != null)
//         {
//             input.OnFirePressed -= Shoot;
//         }
//     }

//     private void Shoot()
//     {
//         Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
//         Debug.Log("PlayerShooter: Shoot() called, bullet instantiated at " + shootPoint.position);
//     }
// }
