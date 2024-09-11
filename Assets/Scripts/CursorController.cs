using UnityEngine;

public class CustomMouseCursor : MonoBehaviour
{
    [SerializeField] Texture2D mouseCursor;

    Vector2 hotSpot = new();
    CursorMode cursorMode = CursorMode.ForceSoftware;

    private void Start()
    {        
        Cursor.SetCursor(mouseCursor, hotSpot, cursorMode);
        
    }
}