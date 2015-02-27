using System;
using UnityEngine;
using ZFrame.Frame.MVVM;

[View(typeof (MVVMDemoViewModel))]
[RequireComponent(typeof(MVVMBind))]
public class MVVMDemoView : MonoBehaviour
{
	[BindingMember("Key")]
	public string Key { get; set; }

	[BindingMember("ID")]
	public int ID { get; set; }

	[BindingMember("MethodA")]
	public event Action EventA;

	private void OnGUI()
	{
		GUILayout.Label(Key);
		GUILayout.Label(ID.ToString());
		if (GUILayout.Button("MethodA"))
		{
			MethodA();
		}
		if (GUILayout.Button("MethodB"))
		{
			MethodB(Key);
		}
		if (GUILayout.Button("MethodC"))
		{
			MethodC(Key, ID);
		}
		if (GUILayout.Button("Event"))
		{
			EventA.Invoke();
		}
	}

	[BindingMember("MethodA")]
	public void MethodA()
	{
		MVVMEngine.Instance.Notify(MethodA);
	}

	[BindingMember("MethodB")]
	public void MethodB(string value)
	{
		MVVMEngine.Instance.Notify(MethodB, value);
	}

	[BindingMember("MethodC")]
	public void MethodC(string value, int value2)
	{
		MVVMEngine.Instance.Notify(MethodC, value, value2);
	}
}