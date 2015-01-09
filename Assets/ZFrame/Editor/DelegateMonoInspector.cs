using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof (DelegateMono))]
public class DelegateMonoInspector : Editor
{
	private EventInfo[] _events;
	private Dictionary<string, MonoBehaviour> _monos = new Dictionary<string, MonoBehaviour>();

	private void OnEnable()
	{
		_events = typeof (DelegateMono).GetEvents();
		foreach (EventInfo eventInfo in _events)
		{
			_monos.Add(eventInfo.Name, null);
		}
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		foreach (EventInfo eventInfo in _events)
		{
			//_monos[eventInfo.Name] =
			//	EditorGUILayout.ObjectField(eventInfo.Name, _monos[eventInfo.Name], typeof (MonoBehaviour), true) as MonoBehaviour;
			//if (_monos[eventInfo.Name] != null)
			//{
			//	var methods = GetMethods(_monos[eventInfo.Name].gameObject).Select(m=>m.methodName).ToArray();
			//	int index = 0;
			//	index = EditorGUILayout.Popup("Method", index, methods);
			//}
			var prop = serializedObject.FindProperty(eventInfo.Name);
			if (prop != null)
			{
				EditorGUILayout.PropertyField(prop);
			}
		}

		serializedObject.ApplyModifiedProperties();
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

					yield return new MonoMethod {target = mb, methodName = mi.Name};
				}
			}
		}
	}

	public class MonoMethod
	{
		public Component target;
		public string methodName;

		public static Delegate CreateDelegate<T>(MonoMethod monoMethod)
		{
			if (monoMethod.target == null || string.IsNullOrEmpty(monoMethod.methodName))
			{
				return null;
			}

			//Check compatibility before CreateDelegate
			//...if false, return null
			
			return Delegate.CreateDelegate(typeof (T), monoMethod.target, monoMethod.methodName);
		}
	}
}