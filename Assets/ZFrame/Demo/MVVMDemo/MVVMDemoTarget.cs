using UnityEngine;
using ZFrame.Frame.MVVM;

[BindingTarget(typeof (MVVMDemoSource))]
public class MVVMDemoTarget : MonoBehaviour
{
	[BindingProperty("Key")]
	public string Key { get; set; }

	[BindingProperty("ID")]
	public int ID { get; set; }

	private void OnGUI()
	{
		GUILayout.Label(Key);
		GUILayout.Label(ID.ToString());
	}

	private void Awake()
	{
		MVVMEngine.Instance.Register(this);
	}
}