using System.IO;
using System.Linq;
using LitJson;
using UnityEditor;
using UnityEngine;
using ZFrame.IO;

namespace ZFrame
{
	[CustomEditor(typeof (ResourceRef))]
	public class ResourceRefEditor : Editor
	{
		private const string ConfigPath = "Assets/Resources/ReferenceConfig.txt";
		private bool editable;
		private string[] groupdps = {"None", "A", "B", "C", "D", "5", "6", "7", "8", "9", "10"};
		private int[] groupNums = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10};

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			SerializedProperty groups = serializedObject.FindProperty("groups");

			editable = EditorGUILayout.BeginToggleGroup("Editable", editable);

			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.PropertyField(groups.FindPropertyRelative("Array.size"), GUIContent.none, GUILayout.MaxWidth(50));

			if (GUILayout.Button(new GUIContent("Add", "Add game resource group")))
			{
				groups.InsertArrayElementAtIndex(groups.arraySize);
				SerializedProperty group = groups.GetArrayElementAtIndex(groups.arraySize - 1);
				ResetGroup(group);
			}

			if (GUILayout.Button(new GUIContent("Backup", "Backup references to config file")))
			{
				Backup();
			}

			if (GUILayout.Button(new GUIContent("Restore", "Restore reference from file")))
			{
				Restore();
			}

			EditorGUILayout.EndHorizontal();

			for (int i = 0; i < groups.arraySize; i++)
			{
				SerializedProperty group = groups.GetArrayElementAtIndex(i);
				SerializedProperty id = group.FindPropertyRelative("id");
				SerializedProperty folded = group.FindPropertyRelative("folded");
				SerializedProperty resources = group.FindPropertyRelative("resources");

				folded.boolValue = EditorGUILayout.Foldout(folded.boolValue, groupdps[id.intValue]);

				if (folded.boolValue)
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PropertyField(resources.FindPropertyRelative("Array.size"), GUIContent.none, GUILayout.MaxWidth(50));
					if (GUILayout.Button(new GUIContent("Add", "Add game resource group"), EditorStyles.miniButton, GUILayout.Width(50)))
					{
						resources.InsertArrayElementAtIndex(resources.arraySize);
						SerializedProperty resource = resources.GetArrayElementAtIndex(resources.arraySize - 1);
						ResetResource(resource);
					}
					id.intValue = EditorGUILayout.IntPopup(id.intValue, groupdps, groupNums, GUILayout.Width(100));

					if (GUILayout.Button("x", EditorStyles.miniButtonRight, GUILayout.Width(25)))
					{
						groups.DeleteArrayElementAtIndex(i);
						break;
					}
					EditorGUILayout.EndHorizontal();

					for (int j = 0; j < resources.arraySize; j++)
					{
						SerializedProperty res = resources.GetArrayElementAtIndex(j);
						SerializedProperty key = res.FindPropertyRelative("key");
						SerializedProperty obj = res.FindPropertyRelative("resource");

						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.PropertyField(key, GUIContent.none);

						if (GUILayout.Button(new GUIContent("A", "Auto assign key"), EditorStyles.miniButtonMid, GUILayout.Width(25)))
						{
							if (obj.objectReferenceValue != null)
							{
								key.stringValue = obj.objectReferenceValue.name;
							}
						}

						if (GUILayout.Button("x", EditorStyles.miniButtonRight, GUILayout.Width(25)))
						{
							resources.DeleteArrayElementAtIndex(j);
							break;
						}
						EditorGUILayout.PropertyField(obj, GUIContent.none);
						EditorGUILayout.EndHorizontal();
					}
				}
			}
			serializedObject.ApplyModifiedProperties();
		}

		private void ResetGroup(SerializedProperty resGroup)
		{
			SerializedProperty id = resGroup.FindPropertyRelative("id");
			SerializedProperty resources = resGroup.FindPropertyRelative("resources");

			id.intValue = 0;
			resources.ClearArray();
		}

		private void ResetResource(SerializedProperty res)
		{
			SerializedProperty key = res.FindPropertyRelative("key");
			SerializedProperty obj = res.FindPropertyRelative("resource");
			key.stringValue = null;
			obj.objectReferenceValue = null;
		}

		private void Restore()
		{
			ResourceRef references = serializedObject.targetObject as ResourceRef;
			if (references != null)
			{
				string json = File.ReadAllText(ConfigPath);
				GroupConfig[] config = JsonMapper.ToObject<GroupConfig[]>(json);

				references.groups = config.Select(c => c.ToResourceGroup()).ToArray();

				Debug.Log("Config Loaded!");
			}
		}

		private void Backup()
		{
			ResourceRef references = serializedObject.targetObject as ResourceRef;
			if (references != null && references.groups != null &&
			    references.groups.All(g => g.resources != null))
			{
				string json =
					JsonMapper.ToJson(
						references.groups.Select(
							g =>
								new GroupConfig
								{
									id = g.id,
									resources =
										g.resources.Select(
											r =>
												new ResourceConfig
												{
													key = r.key,
													path = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(r.resource))
												})
											.ToArray()
								})
							.ToArray());
				File.WriteAllText(ConfigPath, json);

				Debug.Log("Config saved!");
			}
			else
			{
				Debug.LogError("Config saving failed! Make sure all key and value is assigned!");
			}
		}

		private class ResourceConfig
		{
			public string key;
			public string path;

			public static implicit operator ResourceRef.GameResource(ResourceConfig config)
			{
				ResourceRef.GameResource res = new ResourceRef.GameResource {key = config.key};
				res.resource = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(config.path), typeof (Object));
				return res;
			}

			public ResourceRef.GameResource ToGameResource()
			{
				return this;
			}
		}

		private class GroupConfig
		{
			public int id;
			public ResourceConfig[] resources;

			public static implicit operator ResourceRef.ResourceGroup(GroupConfig config)
			{
				ResourceRef.ResourceGroup group = new ResourceRef.ResourceGroup {id = config.id};
				group.resources = config.resources.Select(r => r.ToGameResource()).ToArray();

				return group;
			}

			public ResourceRef.ResourceGroup ToResourceGroup()
			{
				return this;
			}
		}
	}
}