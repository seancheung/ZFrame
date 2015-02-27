using System;
using System.ComponentModel;
using UnityEngine;
using ZFrame.Debugger;
using ZFrame.Frame.MVVM;

[ViewModel]
[RequireComponent(typeof(MVVMBind))]
public class MVVMDemoViewModel : MonoBehaviour, INotifyPropertyChanged
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
				OnPropertyChanged("Key");
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
                OnPropertyChanged("ID");
			}
		}
	}

	private void Update()
	{
		Key = DateTime.Now.ToLongTimeString();
		ID = DateTime.Now.Second;
	}

	public void MethodA()
	{
		ZDebug.Log("MethodA");
	}

	public void MethodB(string value)
	{
		ZDebug.Log("MethodB: " + value);
	}

	public void MethodC(string value, int value2)
	{
		ZDebug.Log("MethodB: " + value + " -- " + value2);
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected virtual void OnPropertyChanged(string propertyName)
	{
		PropertyChangedEventHandler handler = PropertyChanged;
		if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
	}
}