using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MoveAndFadeText))]
public class MoveAndFadeTextEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MoveAndFadeText script = (MoveAndFadeText)target;
        if (GUILayout.Button("Begin Animation"))
        {
            script.TriggerAnimation();
        }
    }
}
