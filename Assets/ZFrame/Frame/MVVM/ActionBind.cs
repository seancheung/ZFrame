using UnityEngine;
using System.Collections;
using System;

public class ActionBind : MonoBehaviour {

	[Serializable]
	public class MonoAction
	{
		[SerializeField] private Component component;
		[SerializeField]private string method;
		private Action action;
	}
}
