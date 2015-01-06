using System;
using UnityEngine;
using ZFrame.Frame.MVVM;

[BindingSource]
public class MVVMDemoSource : MonoBehaviour
{
	private string _key;
	private int _id;

	public string Key
	{
		get { return _key; }
		set
		{
			if (_key != value)
			{
				_key = value;
				MVVMEngine.Instance.Notify(() => Key);
			}
		}
	}

	public int ID
	{
		get { return _id; }
		set
		{
			if (_id != value)
			{
				_id = value;
				MVVMEngine.Instance.Notify(this, "ID");
			}
		}
	}

	private void Update()
	{
		Key = DateTime.Now.ToLongTimeString();
		ID = DateTime.Now.Second;
	}

	private void Awake()
	{
		MVVMEngine.Instance.Register(this);
	}
}