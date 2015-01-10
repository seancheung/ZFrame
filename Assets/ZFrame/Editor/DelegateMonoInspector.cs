using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof (DelegateMono))]
public class DelegateMonoInspector : Editor
{
	public override void OnInspectorGUI()
	{
		foreach (KeyValuePair<string, List<Delegate>> del in ((DelegateMono) serializedObject.targetObject).delegates)
		{
			EditorGUILayout.LabelField(del.Key);
			foreach (Delegate method in del.Value)
			{
				EditorGUILayout.SelectableLabel(method.Target + "->" + method.Method);
			}
		}
	}

	private static IEnumerable<MonoMethod> GetMethods(GameObject target)
	{
		MonoBehaviour[] comps = target.GetComponents<MonoBehaviour>();

		for (int i = 0, imax = comps.Length; i < imax; ++i)
		{
			MonoBehaviour mb = comps[i];
			if (mb == null) continue;

			MethodInfo[] methods = mb.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public);

			foreach (MethodInfo mi in methods)
			{
				if (mi.ReturnType == typeof (void))
				{
					string name = mi.Name;
					if (name == "Invoke") continue;
					if (name == "InvokeRepeating") continue;
					if (name == "CancelInvoke") continue;
					if (name == "StopCoroutine") continue;
					if (name == "StopAllCoroutines") continue;
					if (name == "BroadcastMessage") continue;
					if (name.StartsWith("SendMessage")) continue;
					if (name.StartsWith("set_")) continue;
					if (name.StartsWith("add_")) continue;
					if (name.StartsWith("remove_")) continue;

					yield return new MonoMethod(mb, mi.Name);
				}
			}
		}
	}
}