using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCursor : MonoBehaviour
{
    Texture2D cursorImage;

    void Start()
    {
        cursorImage = Resources.Load<Texture2D>("Sprites/Cursor");
        Cursor.SetCursor(cursorImage, Vector2.zero, CursorMode.Auto);
    }
}
