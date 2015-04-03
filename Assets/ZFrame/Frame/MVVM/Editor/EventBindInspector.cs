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

[CustomEditor(typeof (EventBind))]
public class EventBindInspector : Editor
{
    private ReorderableList _list;
    private static string format = "{0}/{1}";
    private static string eventFormat = "{0}/{1}({2})";
    private static float padding = 5f;

    private void OnEnable()
    {
        _list = new ReorderableList(serializedObject, serializedObject.FindProperty("bindingGroups"));
        float baseHeight = 2*EditorGUIUtility.singleLineHeight + 2*padding;

        _list.drawElementCallback += (rect, index, active, focused) =>
        {
            SerializedProperty group = _list.serializedProperty.GetArrayElementAtIndex(index);
            SerializedProperty component = group.FindPropertyRelative("component");


            _list.elementHeight = baseHeight;

            rect.y += padding;

            //event
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width/2, EditorGUIUtility.singleLineHeight),
                component, GUIContent.none);

            if (component.objectReferenceValue)
            {
                SerializedProperty action = group.FindPropertyRelative("action");
                SerializedProperty method = group.FindPropertyRelative("method");

                IEnumerable<MemberPath> eventPaths = GetEvents(component.objectReferenceValue.To<Component>().gameObject);
                string[] events = eventPaths.Select(p => p.ToString()).ToArray();
                int i = events.ToList().IndexOf(action.stringValue);
                i = Mathf.Clamp(i, 0, events.Length - 1);

                EditorGUI.BeginChangeCheck();
                i = EditorGUI.Popup(
                    new Rect(rect.x + rect.width/2, rect.y, rect.width/2, EditorGUIUtility.singleLineHeight), i, events);
                if (EditorGUI.EndChangeCheck())
                {
                    component.objectReferenceValue = eventPaths.ElementAt(i).component;
                }
                action.stringValue = events.At(i);

                //methods
                SerializedProperty comp = method.FindPropertyRelative("component");

                if (events.At(i) != null)
                {
                    EditorGUI.PropertyField(
                        new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, rect.width/2,
                            EditorGUIUtility.singleLineHeight),
                        comp, GUIContent.none);

                    if (comp.objectReferenceValue)
                    {
                        SerializedProperty meth = method.FindPropertyRelative("method");

                        IEnumerable<MemberPath> methPaths = GetMethods(
                            comp.objectReferenceValue.To<Component>().gameObject,
                            eventPaths.ElementAt(i).member.To<EventInfo>().EventHandlerType.Method("Invoke"));
                        string[] meths = methPaths.Select(p => p.ToString()).ToArray();
                        int j = meths.ToList().IndexOf(meth.stringValue);
                        j = Mathf.Clamp(j, 0, meths.Length - 1);

                        EditorGUI.BeginChangeCheck();
                        j = EditorGUI.Popup(
                            new Rect(rect.x + rect.width/2, rect.y + EditorGUIUtility.singleLineHeight, rect.width/2,
                                EditorGUIUtility.singleLineHeight), j, meths);
                        if (EditorGUI.EndChangeCheck())
                        {
                            comp.objectReferenceValue = methPaths.ElementAt(j).component;
                        }
                        meth.stringValue = meths.At(j);
                    }
                }
            }

            GUILayout.Space(padding);
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

    private IEnumerable<MemberPath> GetMethods(GameObject gameObject, MethodInfo method)
    {
        IEnumerable<Component> components = gameObject.GetComponents<Component>();

        foreach (Component component in components)
        {
            Type type = component.GetType();
            IList<MethodInfo> methods = type.Methods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public);
            foreach (
                MethodInfo methodInfo in
                    methods.Where(
                        m =>
                            !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_") && m.ReturnType == method.ReturnType &&
                            m.HasParameterSignature(method.GetParameters())))
                yield return new MemberPath(component, methodInfo);
        }

        foreach (
            MethodInfo methodInfo in
                typeof (GameObject).Methods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public)
                    .Where(
                        m =>
                            !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_") && m.ReturnType == method.ReturnType &&
                            m.HasParameterSignature(method.GetParameters())))
        {
            yield return new MemberPath(gameObject, methodInfo);
        }
    }

    private IEnumerable<MemberPath> GetEvents(GameObject gameObject)
    {
        IEnumerable<Component> components = gameObject.GetComponents<Component>();

        foreach (Component component in components)
        {
            Type type = component.GetType();
            EventInfo[] events = type.GetEvents();
            foreach (EventInfo eventInfo in events)
                yield return new MemberPath(component, eventInfo);
        }

        foreach (EventInfo eventInfo in typeof (GameObject).GetEvents(BindingFlags.Public))
        {
            yield return new MemberPath(gameObject, eventInfo);
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
            if (member is EventInfo)
                return string.Format(eventFormat, component.GetType().Name(), member.Name,
                    member.To<EventInfo>().EventHandlerType.Name());
            return string.Format(format, component.GetType().Name(), member.Name);
        }
    }
}