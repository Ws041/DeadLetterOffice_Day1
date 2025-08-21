using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorHover : MonoBehaviour
{
    [SerializeField] Texture2D hoverCursor;
    [SerializeField] Texture2D defaultCursor;

    private void Start() => Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
    private void OnMouseEnter() => Cursor.SetCursor(hoverCursor, Vector2.zero, CursorMode.ForceSoftware);

    private void OnMouseExit() => Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
}
