#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

public class ColorSetter : EditorWindow
{
    private Color _color;

    [MenuItem("Rooster/Color Setter")]
    static void Init()
    {
        ColorSetter colorSetterWindow = (ColorSetter) GetWindow(typeof(ColorSetter));
        colorSetterWindow.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Change Color for Selected Objects", EditorStyles.boldLabel);

        _color = EditorGUILayout.ColorField("Color", _color);

        if (GUILayout.Button("Change Color"))
        {
            ChangeColor();
        }
    }

    void ChangeColor()
    {
        foreach (GameObject item in Selection.gameObjects)
        {
            Renderer _renderer = item.GetComponent<Renderer>();
            if (_renderer!=null)
            {
                _renderer.sharedMaterial.color = _color;
            }
        }
    }
}
#endif