using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorChanger : MonoBehaviour
{
    [SerializeField] Texture2D customCursor;
    [SerializeField] Vector2 centerPos;
    [SerializeField] Color cursorColor;
    void Awake()
    {
        if (customCursor != null) SetCursorWithColor(customCursor, centerPos, cursorColor);
    }
    public void SetCursor(Texture2D cursor, Vector2 center, CursorMode mode = CursorMode.Auto)
        => Cursor.SetCursor(cursor, center, mode);

    public void SetCursorWithColor(Texture2D cursor, Vector2 center, Color color, CursorMode mode = CursorMode.Auto)
    {
        if (color == null)
        {
            SetCursor(cursor, center, mode);
            return;
        }

        var fixedCursor = new Texture2D(cursor.width, cursor.height);

        for (int y = 0; y < cursor.height; y++)
        {
            for (int x = 0; x < cursor.width; x++)
            {
                var origin = cursor.GetPixel(x, y);
                origin *= color;
                fixedCursor.SetPixel(x, y, origin);
            }
        }
        fixedCursor.Apply();
        SetCursor(fixedCursor, center, mode);
    }
}
