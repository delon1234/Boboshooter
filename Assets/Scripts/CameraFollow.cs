using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float speed = 5f;
    public float fixedY = 0f;
    public float fixedZ = -10f;

    private Transform player;
    private Transform cam;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }
    
    private void LateUpdate()
    {
        if (player == null) return;
        Vector3 desiredPosition = new Vector3(player.position.x, player.position.y, fixedZ);
        cam.position = Vector3.Lerp(cam.position, desiredPosition, speed * Time.deltaTime);
    }
}
