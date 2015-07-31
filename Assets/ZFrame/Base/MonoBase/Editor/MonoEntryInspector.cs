using System;
using System.Collections.Generic;
using Fasterflect;
using UnityEditor;
using UnityEngine;
using ZFrame.Base.MonoBase;

[CustomEditor(typeof (MonoEntry))]
public class MonoEntryInspector : Editor
{
    public override void OnInspectorGUI()
    {
        MonoEntry entry = serializedObject.targetObject as MonoEntry;
        Dictionary<string, List<Action>> invokers = entry.GetFieldValue("invokers") as Dictionary<string, List<Action>>;

        if (invokers.Count == 0)
            return;

        GUILayout.BeginVertical();
        foreach (KeyValuePair<string, List<Action>> invoker in invokers)
        {
            GUILayout.Label(invoker.Key, EditorStyles.boldLabel);
            GUILayout.BeginVertical(GUI.skin.box);
            {
                foreach (Action action in invoker.Value)
                {
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);
                    {
                        GUILayout.Label(action.Target.ToString(), EditorStyles.miniLabel);
                        GUILayout.Label(action.Method.Name, EditorStyles.miniLabel);
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndVertical();
    }
}