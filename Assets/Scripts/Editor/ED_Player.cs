using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player))]
public class ED_Player : Editor
{
    Player m_Player;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        m_Player = target as Player;
        GUILayout.Label("< OP >");
        if (GUILayout.Button("Equip Any Perk"))
        {
            // m_Player;
        }
    }
}
