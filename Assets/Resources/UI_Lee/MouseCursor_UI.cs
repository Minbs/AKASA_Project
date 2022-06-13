using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseCursor_UI : MonoBehaviour
{
    public bool hotSpotIscenter = false;
    public Texture2D CursorTexture;
    private Vector2 hotSpot;
    public Vector2 adjustHotSpot = Vector2.zero;


    public void Start()
    {
        StartCoroutine("MyCursor");
        Debug.Log("付快胶目辑 内风凭");
        Cursor.SetCursor(CursorTexture, hotSpot, CursorMode.Auto);
    }

    IEnumerator MyCursor()
    {
        yield return new WaitForEndOfFrame();

        if(hotSpotIscenter)
        {
            hotSpot.x = CursorTexture.width / 2;
            hotSpot.y = CursorTexture.height / 2;
        }
        else
        {
            hotSpot = adjustHotSpot;
        }
        Cursor.SetCursor(CursorTexture, hotSpot, CursorMode.Auto);
    }

}
