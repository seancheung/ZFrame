using System.Xml;
using UnityEditor;
using UnityEngine;

public class ZUIEditor : EditorWindow
{
	private static ZUIEditor _instance;
	private TextAsset _file;
	private Vector2 _pos;
	private XmlDocument _doc = new XmlDocument();

	[MenuItem("ZFrame/ZUIEditor")]
	private static void Init()
	{
		if (!_instance)
		{
			_instance = GetWindow<ZUIEditor>();
			_instance.minSize = new Vector2(320, 480);
		}
	}

	private void OnEnable()
	{
		if (Selection.activeObject && Selection.activeObject.GetType() == typeof(TextAsset))
		{
			_file = Selection.activeObject as TextAsset;
			
		}
	}

	private void OnGUI()
	{
		TextAsset file =
			EditorGUILayout.ObjectField("XML File", _file, typeof(TextAsset), false) as TextAsset;
		if (file != null && _file != file)
		{
			_file = file;
			_doc.LoadXml(file.text);
		}

		Debug.Log(_doc["gameobject"]);
	}
}