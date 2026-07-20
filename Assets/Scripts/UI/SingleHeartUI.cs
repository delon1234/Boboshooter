using UnityEngine;
using UnityEngine.UI;
public class SingleHeartUI : MonoBehaviour
{
    /* SingleHeartUI is attached on a HeartTemplate prefab, which has an Image component to represent a heart
    Responsible for changing sprite to match state
    */

    // Reference to heart sprites for different health states
    [SerializeField] private Sprite fullHeart, halfHeart, emptyHeart;
    Image heartImage; // Reference to the Image component to change the sprite

    // For toggling hearts to grey when invulnerable
    private static readonly Color NormalColor = Color.white;
    private static readonly Color GreyColor = new Color(0.45f, 0.45f, 0.45f, 1f);

    private void Awake()
    {
        heartImage = GetComponent<Image>();
    }

    public void SetState(HeartState state)
    {
        // Set Sprite to corresponding state
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

    public void SetGreyed(bool greyed)
    {
        heartImage.color = greyed ? GreyColor : NormalColor;
    }
}

public enum HeartState
{
    Empty,
    Half,
    Full
}
