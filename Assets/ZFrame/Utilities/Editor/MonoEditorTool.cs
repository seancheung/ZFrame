using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class MonoEditorTool
{
    public static MonoReflector.MonoMember DrawMethodListLayout(IEnumerable<MonoReflector.MonoMember> members,
        ref int index)
    {
        EditorGUI.BeginChangeCheck();
        index = EditorGUILayout.Popup(index, members.Select(m => m.ToString()).ToArray());
        return members.ElementAt(index);
    }

    public static MonoReflector.MonoMember DrawMethodList(Rect rect, IEnumerable<MonoReflector.MonoMember> members,
        ref int index)
    {
        EditorGUI.BeginChangeCheck();
        index = EditorGUI.Popup(rect, index, members.Select(m => m.ToString()).ToArray());
        return members.ElementAt(index);
    }
}