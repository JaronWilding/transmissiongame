using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using WindowsA;

[CreateAssetMenu(fileName = "Test Window", menuName = "Test Window Jaron")]
public class ShapeData : ScriptableObject
{
    public TestWin _testWin;
    public Vector3 Position;
    public string ShapeName;
}
