using UnityEngine;

public class Player : MonoBehaviour
{
    /* Facade Pattern
     * It exposes references of Player components to the other scripts in a single, 
     * convenient location.
     */
    
    // Expose cached references to other scripts
    public PlayerHealth Health { get; private set; }
    public PlayerController Controller { get; private set; }
    public PlayerInputHandler Input { get; private set; }

    // Cache singleton instance
    public static Player Instance { get; private set; }

    private void Awake()
    {
        // Caching Instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        // Cache the references once
        Health = GetComponent<PlayerHealth>();
        Controller = GetComponent<PlayerController>();
        Input = GetComponent<PlayerInputHandler>();
    }
}
