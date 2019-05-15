using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TestScript))]
public class TestScriptEditor : Editor
{
    // Variables
    TestScript targetScript;

    GUIStyle headerStyle = new GUIStyle();
    GUIStyle footerStyle = new GUIStyle();

    // Main Methods
    private void OnEnable()
    {
        targetScript = (TestScript)target;
        GenerateStyles();
    }

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();
        EditorGUILayout.LabelField("HEADER", headerStyle, GUILayout.Height(50));
        targetScript.speedValue = EditorGUILayout.Slider("Speed Value: ", targetScript.speedValue, 0.0f, 10.0f);

        EditorGUILayout.LabelField("Transmission", footerStyle, GUILayout.Height(25));

    }

    //Utility Methods

    void GenerateStyles()
    {
        Texture2D bg = (Texture2D)Resources.Load("header_bg_001");
        Font fnt = (Font)Resources.Load("BAHNSCHRIFT");

        headerStyle.normal.background = bg;
        headerStyle.font = fnt;
        headerStyle.fontSize = 32;
        headerStyle.normal.textColor = Color.white;
        headerStyle.alignment = TextAnchor.MiddleCenter;

        footerStyle.normal.background = bg;
        footerStyle.font = fnt;
        footerStyle.fontSize = 12;
        footerStyle.normal.textColor = Color.white;
        footerStyle.alignment = TextAnchor.LowerRight;
        footerStyle.padding = new RectOffset(0, 5, 0, 2);

    }
}
