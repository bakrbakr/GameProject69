using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridUtils
{
    public static TextMesh CreateWorldText(string text, Transform parent, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment)
    {
        GameObject gameObject = new GameObject("Grid Text", typeof(TextMesh));

        Transform transform = gameObject.transform;
        transform.SetParent(parent);
        transform.localPosition = localPosition;

        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.color = color;
        textMesh.fontSize = fontSize;

        return textMesh;
    }

    public static Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
