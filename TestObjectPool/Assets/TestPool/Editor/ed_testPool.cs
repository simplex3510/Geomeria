using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(testPool))]
public class ed_testPool : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Label("< OP >");
        if (GUILayout.Button("SpawnEnemyType1"))
        {
            (target as testPool).SpawnEnemyType1();
        }
        if (GUILayout.Button("SpawnEnemyType2"))
        {
            (target as testPool).SpawnEnemyType2();
        }
    }
    
}
