using UnityEngine;

namespace UI
{
    public class HardwareAnimatedCursor : MonoBehaviour
    {
        // Uses Unity Native Hardware Cursor to set custom cursor
        [Header("Cursor Animation Frames")]
        [SerializeField] private Texture2D[] cursorFrames; // Set texture type to cursor

        [Tooltip("Frames per second.")]
        [SerializeField] private float frameRate = 12f;

        [Tooltip("Offset for active clicking point is default top left (0,0). For centre; width/2, height/2")]
        [SerializeField] private Vector2 hotSpot = new Vector2(32f, 32f);

        [SerializeField] private CursorMode cursorMode = CursorMode.Auto;

        private int currentFrameIndex = 0;
        private float timer = 0f;

        private void Update()
        {
            if (cursorFrames == null || cursorFrames.Length == 0) return;

            timer += Time.deltaTime;
            if (timer >= 1f / frameRate)
            {
                timer -= 1f / frameRate;
                currentFrameIndex = (currentFrameIndex + 1) % cursorFrames.Length;
                Cursor.SetCursor(cursorFrames[currentFrameIndex], hotSpot, cursorMode);
            }
        }

        private void OnDisable()
        {
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
        }

        private void OnDestroy()
        {
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
        }
    }
}
