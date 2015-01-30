using System;
using UnityEditor;
using UnityEngine;
using ZFrame.IO.CSV;

public partial class CSVEditor : EditorWindow
{
	private static CSVEditor _instance;
	private TextAsset _file;
	private CSVEngine _reader = new CSVEngine();
	private Vector2 _pos;

	[MenuItem("ZFrame/CSVEditor")]
	private static void Init()
	{
		if (!_instance)
		{
			_instance = GetWindow<CSVEditor>();
			_instance.minSize = new Vector2(800, 600);
		}
	}

	private void OnEnable()
	{
		if (Selection.activeObject && Selection.activeObject.GetType() == typeof (TextAsset))
		{
			_file = Selection.activeObject as TextAsset;
			_reader.Load(_file.text);
		}
	}

	private void OnGUI()
	{
		TextAsset file =
			EditorGUILayout.ObjectField("CSV File", _file, typeof (TextAsset), false) as TextAsset;
		if (file != null && _file != file)
		{
			bool result = _reader.Load(file.text);
			if (result)
			{
				_file = file;
			}
		}

		DrawContent();
	}

	private void DrawContent()
	{
		if (!_reader.CanRead)
			return;

		_pos = EditorGUILayout.BeginScrollView(_pos);
		EditorGUILayout.BeginVertical();
		{
			DrawMenu();

			for (int i = 0; i < _reader.RowCount; i++)
			{
				var rect = EditorGUILayout.BeginHorizontal(GUILayout.Width(position.width - 10));
				{
					for (int j = 0; j < _reader.ColumnCount; j++)
					{
						if (j == 0)
							DrawRowMarker(i);
						EditorGUILayout.SelectableLabel(_reader[i, j]);
					}
				}
				EditorGUILayout.EndHorizontal();
			}

			DrawColumnMarkers();
		}
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndScrollView();
	}

	private void DrawMenu()
	{
		EditorGUILayout.BeginHorizontal(GUILayout.Width(position.width - 10));
		{
			if (GUILayout.Button("Save"))
			{
				Save();
			}
			if (GUILayout.Button("Restore"))
			{
				Restore();
			}
		}
		EditorGUILayout.EndHorizontal();
	}

	private void Restore()
	{
		throw new NotImplementedException();
	}

	private void Save()
	{
		throw new NotImplementedException();
	}

	private void DrawRowMarker(int i)
	{
		EditorGUILayout.BeginHorizontal(GUILayout.Width(65));
		{
			EditorGUILayout.LabelField(i.ToString(), EditorStyles.miniLabel, GUILayout.Width(20));
			GUILayout.Button("x", EditorStyles.miniButtonLeft, GUILayout.Width(20));
			GUILayout.Button("+", EditorStyles.miniButtonRight, GUILayout.Width(20));
		}
		EditorGUILayout.EndHorizontal();
	}

	private void DrawColumnMarkers()
	{
		EditorGUILayout.BeginHorizontal(GUILayout.Width(position.width - 10));
		{
			EditorGUILayout.LabelField("", GUILayout.Width(65));

			GUILayoutOption width = GUILayout.Width((position.width - 100)/_reader.ColumnCount);
			for (int i = 0; i < _reader.ColumnCount; i++)
			{
				EditorGUILayout.BeginHorizontal(width);
				{
					GUILayout.Button("x", EditorStyles.miniButtonLeft);
					GUILayout.Button("+", EditorStyles.miniButtonRight);
				}
				EditorGUILayout.EndHorizontal();
			}
		}
		EditorGUILayout.EndHorizontal();
	}
}

public partial class CSVEditor
{
	 
}