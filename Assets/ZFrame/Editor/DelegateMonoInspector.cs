using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof (DelegateMono))]
public class DelegateMonoInspector : Editor
{
	private static EventInfo[] _events;
	//private static Dictionary<string, MonoBehaviour> _monos = new Dictionary<string, MonoBehaviour>();
	//private static Dictionary<string, MonoMethod> _methods = new Dictionary<string, MonoMethod>();

	private void OnEnable()
	{
		if (_events == null)
		{
			_events = typeof(DelegateMono).GetEvents();
			//foreach (EventInfo eventInfo in _events)
			//{
			//	_monos.Add(eventInfo.Name, null);
			//	_methods.Add(eventInfo.Name, null);
			//}
		}
	}

	//void Reset()
	//{
	//	_events = null;
	//	_methods.Clear();
	//	_monos.Clear();
	//}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		//foreach (EventInfo eventInfo in _events)
		//{
		//	_monos[eventInfo.Name] =
		//		EditorGUILayout.ObjectField(eventInfo.Name, _monos[eventInfo.Name], typeof(MonoBehaviour), true) as MonoBehaviour;
		//	if (_monos[eventInfo.Name] != null)
		//	{
		//		MonoMethod[] methods =
		//			GetMethods(_monos[eventInfo.Name].gameObject)
		//				.Where(m => MonoMethod.CreateDelegate(eventInfo.EventHandlerType, m) != null).ToArray();
		//		string[] selections = methods.Select(m => m.ToString()).ToArray();
		//		int index = 0;
		//		if (_methods[eventInfo.Name] != null)
		//		{
		//			index = selections.ToList().IndexOf(_methods[eventInfo.Name].ToString());
		//		}

		//		index = EditorGUILayout.Popup("Method", index, selections);
		//		if (index >= 0)
		//		{
		//			_methods[eventInfo.Name] = methods[index];
		//		}
		//	}
		//}

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

		public static Delegate CreateDelegate(Type type, MonoMethod monoMethod)
		{
			if (monoMethod.target == null || string.IsNullOrEmpty(monoMethod.methodName))
			{
				return null;
			}

			return Delegate.CreateDelegate(type, monoMethod.target, monoMethod.methodName, false, false);
		}

		public static Delegate CreateDelegate<T>(MonoMethod monoMethod)
		{
			return CreateDelegate(typeof (T), monoMethod);
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		public override string ToString()
		{
			return string.Format("{0}:{1}.{2}", target.name, target.GetType(), methodName ?? "NULL");
		}
	}
}