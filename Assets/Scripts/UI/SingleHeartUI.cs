using UnityEngine;
using UnityEngine.UI;
public class SingleHeartUI : MonoBehaviour
{
    // Reference to heart sprites for different health states
    [SerializeField] private Sprite fullHeart, halfHeart, emptyHeart;
    Image heartImage; // Reference to the Image component to change the sprite
    private void Awake()
    {
        heartImage = GetComponent<Image>();
    }

    public void SetState(HeartState state)
    {
        switch (state)
        {
            case HeartState.Full:
                heartImage.sprite = fullHeart;
                break;
            case HeartState.Half:
                heartImage.sprite = halfHeart;
                break;
            case HeartState.Empty:
                heartImage.sprite = emptyHeart;
                break;
        }
    }
}

public enum HeartState
{
    Empty,
    Half,
    Full
}
