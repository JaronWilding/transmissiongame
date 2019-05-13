using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DesignerWindow : EditorWindow
{
    [MenuItem("Window/Designer")]
    static void OpenWindow()
    {
        DesignerWindow window = (DesignerWindow)GetWindow(typeof(DesignerWindow));
        window.minSize = new Vector2(200f, 200f);
        window.Show();
    }
}
