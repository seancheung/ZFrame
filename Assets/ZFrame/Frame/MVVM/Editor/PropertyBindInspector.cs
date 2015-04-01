using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fasterflect;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using ZFrame.Utilities;
using Object = UnityEngine.Object;

[CustomEditor(typeof (PropertyBind))]
public class PropertyBindInspector : Editor
{
    private ReorderableList _list;
    private static string format = "{0}/{1}({2})";
    private static float padding = 5f;

    private void OnEnable()
    {
        Color color = GUI.backgroundColor;
        _list = new ReorderableList(serializedObject, serializedObject.FindProperty("bindingGroups"));
        _list.elementHeight = 4*EditorGUIUtility.singleLineHeight + 2*padding;

        _list.drawElementCallback += (rect, index, active, focused) =>
        {
            SerializedProperty group = _list.serializedProperty.GetArrayElementAtIndex(index);
            if (Application.isPlaying)
            {
                SerializedProperty canUpdate = group.FindPropertyRelative("canUpdate");
                if (!canUpdate.boolValue)
                    GUI.backgroundColor = Color.red;
            }
            SerializedProperty source = group.FindPropertyRelative("source");
            SerializedProperty sourceProp = group.FindPropertyRelative("sourceProp");
            SerializedProperty target = group.FindPropertyRelative("target");
            SerializedProperty targetProp = group.FindPropertyRelative("targetProp");
            SerializedProperty bindDir = group.FindPropertyRelative("bindingDirection");
            SerializedProperty format = group.FindPropertyRelative("format");

            rect.y += padding;

            //source
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width/2, EditorGUIUtility.singleLineHeight),
                source, GUIContent.none);

            if (source.objectReferenceValue)
            {
                IEnumerable<MemberPath> sourcePaths = GetProps(source.objectReferenceValue.To<Component>().gameObject);

                string[] props = sourcePaths.Select(p => p.ToString()).ToArray();

                int i = props.ToList().IndexOf(sourceProp.stringValue);
                i = Mathf.Clamp(i, 0, props.Length - 1);

                EditorGUI.BeginChangeCheck();
                i = EditorGUI.Popup(
                    new Rect(rect.x + rect.width/2, rect.y, rect.width/2, EditorGUIUtility.singleLineHeight), i, props);
                if (EditorGUI.EndChangeCheck())
                {
                    source.objectReferenceValue = sourcePaths.ElementAt(i).component;
                }
                sourceProp.stringValue = props.At(i);
            }

            //direction
            if (source.objectReferenceValue && target.objectReferenceValue)
            {
                EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, rect.width,
                        EditorGUIUtility.singleLineHeight), bindDir);
                EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y + 2*EditorGUIUtility.singleLineHeight, rect.width,
                        EditorGUIUtility.singleLineHeight), format);
            }

            //target
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y + 3*EditorGUIUtility.singleLineHeight, rect.width/2,
                    EditorGUIUtility.singleLineHeight),
                target, GUIContent.none);

            if (target.objectReferenceValue)
            {
                IEnumerable<MemberPath> targetPaths = GetProps(target.objectReferenceValue.To<Component>().gameObject);
                string[] props = targetPaths.Select(p => p.ToString()).ToArray();

                int i = props.ToList().IndexOf(targetProp.stringValue);
                i = Mathf.Clamp(i, 0, props.Length - 1);

                EditorGUI.BeginChangeCheck();
                i = EditorGUI.Popup(
                    new Rect(rect.x + rect.width/2, rect.y + 3*EditorGUIUtility.singleLineHeight, rect.width/2,
                        EditorGUIUtility.singleLineHeight), i, props);
                if (EditorGUI.EndChangeCheck())
                {
                    target.objectReferenceValue = targetPaths.ElementAt(i).component;
                }
                targetProp.stringValue = props.At(i);
            }

            GUILayout.Space(padding);

            GUI.backgroundColor = color;
        };
    }

    public override void OnInspectorGUI()
    {
        if (Application.isPlaying)
            GUI.enabled = false;

        serializedObject.Update();
        _list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();

        GUI.enabled = true;
    }

    private IEnumerable<MemberPath> GetProps(GameObject gameObject)
    {
        IEnumerable<Component> components = gameObject.GetComponents<Component>();

        foreach (Component component in components)
        {
            Type type = component.GetType();
            IList<MemberInfo> props = type.FieldsAndProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (MemberInfo memberInfo in props)
                yield return new MemberPath(component, memberInfo);
        }

        foreach (MemberInfo memberInfo in typeof(GameObject).FieldsAndProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            yield return new MemberPath(gameObject, memberInfo);
        }
    }

   

    private class MemberPath
    {
        public Object component;
        public MemberInfo member;

        public MemberPath(Object component, MemberInfo member)
        {
            this.component = component;
            this.member = member;
        }

        public override string ToString()
        {
            return string.Format(format, component.GetType().Name(), member.Name, member.Type().Name());
        }
    }
}