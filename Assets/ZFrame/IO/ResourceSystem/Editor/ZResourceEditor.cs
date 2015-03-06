#define USING_XML

using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
#if USING_XML
using System.Xml.Serialization;

#else
using LitJson;
#endif

namespace ZFrame.IO.ResourceSystem
{
    public class ZResourceEditor : EditorWindow
    {
#if USING_XML
        private const string Path = "Assets/Resources/ZResourceConfig.xml";
#else
	private const string Path = "Assets/Resources/ZResource.txt";
	private const string GroupInfoPath = "Assets/Resources/ZResourceGroupInfo.txt";
#endif
        private static ZResourceEditor _instance;
        private Vector2 _pos;
        private List<GroupConfig> _groups;
        private List<GroupSelection> _selections;
        private readonly GUIStyle _errorStyle = new GUIStyle(EditorStyles.textField);
        private readonly GUIStyle _warningStyle = new GUIStyle(EditorStyles.label);
        private bool _nameLegal;
        private bool _groupFoldout;

        [MenuItem("ZFrame/ZResourceEditor")]
        private static void Init()
        {
            if (!_instance)
            {
                _instance = GetWindow<ZResourceEditor>();
                _instance.minSize = new Vector2(800, 600);
            }
        }

        private void OnEnable()
        {
            _warningStyle.normal.textColor = Color.red;
            _errorStyle.normal.textColor = Color.red;
            _errorStyle.focused.textColor = Color.red;
            _errorStyle.active.textColor = Color.red;

            Load();
        }

        private void OnGUI()
        {
            DrawMenu();
            if (_selections != null) DrawGroup();
            if (_groups != null) DrawContent();
        }

        private void DrawGroup()
        {
            EditorGUILayout.BeginVertical();
            {
                _groupFoldout = EditorGUILayout.Foldout(_groupFoldout, "GROUP");
                {
                    if (_groupFoldout)
                    {
                        for (int i = 0; i < _selections.Count; i++)
                        {
                            GroupSelection selection = _selections[i];
                            EditorGUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField(i.ToString());
                                selection.id = i;
                                selection.name = EditorGUILayout.TextField(selection.name);
                                selection.desc = EditorGUILayout.TextField(selection.desc);

                                if (GUILayout.Button("+", EditorStyles.miniButtonLeft, GUILayout.Width(25)))
                                    //add resource
                                {
                                    _selections.Insert(i + 1, new GroupSelection());
                                    return;
                                }
                                if (GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(25)))
                                    //remove resource
                                {
                                    _selections.RemoveAt(i);
                                    return;
                                }
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawContent()
        {
            float keyWidth = (position.width - 85)/6;
            float pathWidth = (position.width - 85)/6*4;
            float objWidth = (position.width - 85)/6;

            _pos = EditorGUILayout.BeginScrollView(_pos);
            {
                EditorGUILayout.BeginVertical();
                {
                    for (int i = 0; i < _groups.Count; i++)
                    {
                        GroupConfig config = _groups[i];
                        config.foldout = EditorGUILayout.Foldout(config.foldout, GetGroupInfo(config.id));
                        {
                            if (config.foldout)
                            {
                                EditorGUILayout.BeginHorizontal();
                                {
                                    if (GUILayout.Button("+", EditorStyles.miniButtonLeft, GUILayout.Width(25)))
                                        //Add group
                                    {
                                        GroupConfig group = new GroupConfig();
                                        group.Initialize();
                                        _groups.Insert(i + 1, group);
                                        return;
                                    }
                                    if (GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(25)))
                                        //remove group
                                    {
                                        if (_groups.Count <= 1) return;
                                        _groups.RemoveAt(i);
                                        return;
                                    }
                                    config.id = EditorGUILayout.Popup(config.id,
                                        _selections.Select(s => s.name).ToArray(),
                                        GUILayout.Width(100));
                                }
                                EditorGUILayout.EndHorizontal();

                                for (int j = 0; j < config.resources.Count; j++)
                                {
                                    EditorResourceConfig resource = new EditorResourceConfig(config.resources[j]);
                                    bool legal = CheckKey(config.resources[j]);
                                    _nameLegal = _nameLegal && legal;

                                    EditorGUILayout.BeginHorizontal();
                                    {
                                        resource.key = EditorGUILayout.TextField(resource.key,
                                            legal ? EditorStyles.textField : _errorStyle,
                                            GUILayout.Width(keyWidth));
                                        resource.path = EditorGUILayout.TextField(resource.path,
                                            GUILayout.Width(pathWidth));

                                        if (!string.IsNullOrEmpty(resource.path))
                                            resource.obj = AssetDatabase.LoadAssetAtPath(resource.path, typeof (Object));

                                        resource.obj = EditorGUILayout.ObjectField(resource.obj, typeof (Object), false,
                                            GUILayout.Width(objWidth));

                                        if (resource.obj != null)
                                            resource.path = AssetDatabase.GetAssetPath(resource.obj);
                                        else if (!string.IsNullOrEmpty(resource.path))
                                            resource.obj = AssetDatabase.LoadAssetAtPath(resource.path, typeof (Object));

                                        if (GUILayout.Button("+", EditorStyles.miniButtonLeft, GUILayout.Width(25)))
                                            //add resource
                                        {
                                            ResourceConfig res = new ResourceConfig();
                                            res.Initialize();
                                            config.resources.Insert(j + 1, res);
                                            return;
                                        }
                                        if (GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(25)))
                                            //remove resource
                                        {
                                            if (config.resources.Count <= 1) return;
                                            config.resources.RemoveAt(j);
                                            return;
                                        }

                                        config.resources[j] = resource.ToResourceConfig();
                                    }
                                    EditorGUILayout.EndHorizontal();
                                }
                            }
                        }
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView();
        }

        private string GetGroupInfo(int id)
        {
            if (_selections != null)
            {
                GroupSelection info = _selections.FirstOrDefault(g => g.id == id);
                if (info != null)
                    return info.ToString();
            }
            return string.Empty;
        }

        private void DrawMenu()
        {
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("SAVE"))
                    Save();
                if (GUILayout.Button("RESTORE"))
                    Load();
                if (GUILayout.Button("GENERATE"))
                    Generate();
                if (GUILayout.Button("REORDER"))
                    Reorder();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField(_nameLegal ? "" : "Check resource name!", _warningStyle);

            _nameLegal = true;
        }

        private void Reorder()
        {
            if (_groups == null || _groups.Any(g => g.resources == null)) return;
            foreach (GroupConfig group in _groups)
            {
                group.resources = group.resources.OrderBy(r => r.key).ToList();
            }
            _groups = _groups.OrderBy(g => g.id).ToList();
        }

        private void Generate()
        {
#if USING_XML
            XmlSerializer ser = new XmlSerializer(typeof (ZConfig));
            List<GroupConfig> group;
            using (Stream stream = File.OpenRead(Path))
            {
                ZConfig config = ser.Deserialize(stream) as ZConfig;
                group = config.groups;
            }
#else
		string json = File.ReadAllText(Path);
		List<GroupConfig> group = JsonMapper.ToObject<List<GroupConfig>>(json);
#endif

            if (group == null) return;

            IEnumerable<ResourceConfig> res = group.SelectMany(g => g.resources);

            GameObject asset = new GameObject("ZResources");
            asset.AddComponent<ZResource>().resources = res.Select(r => r.ToZResource()).ToArray();
            AssetDatabase.DeleteAsset("Assets/Resources/ZResources");
            PrefabUtility.CreatePrefab("Assets/Resources/ZResources.prefab", asset);
            DestroyImmediate(asset);

            EditorUtility.DisplayDialog("GENERATE", "ZResources successfully generated!", "OK");
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        private bool CheckKey(ResourceConfig res)
        {
            return _groups != null && res != null && !string.IsNullOrEmpty(res.key.Trim()) &&
                   _groups.TrueForAll(
                       g => g.resources != null && g.resources.TrueForAll(r => r == res || r.key != res.key));
        }


        private void Save()
        {
            if (_groups == null)
                return;
            if (!_groups.TrueForAll(g => g.resources != null && g.resources.TrueForAll(CheckKey)))
                return;
#if USING_XML

            ZConfig config = new ZConfig();
            config.selections = _selections;
            config.groups = _groups;
            XmlSerializer ser = new XmlSerializer(typeof (ZConfig));

            using (StreamWriter sw = File.CreateText(Path))
            {
                ser.Serialize(sw, config);
            }
#else
		string json = JsonMapper.ToJson(_groups);
		File.WriteAllText(Path, json);

		string groupJson = JsonMapper.ToJson(_selections);
		File.WriteAllText(GroupInfoPath, groupJson);
#endif

            EditorUtility.DisplayDialog("SAVE", "File successfully saved!", "OK");
            AssetDatabase.Refresh();
        }

        private void Load()
        {
            if (File.Exists(Path))
            {
#if USING_XML
                XmlSerializer ser = new XmlSerializer(typeof (ZConfig));
                using (Stream stream = File.OpenRead(Path))
                {
                    ZConfig config = ser.Deserialize(stream) as ZConfig;
                    _groups = config.groups;
                    _selections = config.selections;
                }
#else
		string json = File.ReadAllText(Path);
		string groupJson = File.ReadAllText(GroupInfoPath);
		_groups = JsonMapper.ToObject<List<GroupConfig>>(json);
		_selections = JsonMapper.ToObject<List<GroupSelection>>(groupJson);
#endif
            }

            if (_groups == null)
            {
                _groups = new List<GroupConfig>();
                GroupConfig group = new GroupConfig();
                group.Initialize();
                _groups.Add(group);
            }
            if (_selections == null)
            {
                _selections = new List<GroupSelection>();
                GroupSelection sel = new GroupSelection {name = "Default", desc = "Default group"};
                _selections.Add(sel);
            }
        }

        #region Internal

        private class EditorResourceConfig : ResourceConfig
        {
            public Object obj;

            public EditorResourceConfig(ResourceConfig config)
            {
                key = config.key;
                path = config.path;
            }

            public ResourceConfig ToResourceConfig()
            {
                return new ResourceConfig {key = key, path = path};
            }
        }

        public class ResourceConfig
        {
#if USING_XML
            [XmlAttribute]
#endif
                public string key;

#if USING_XML
            [XmlAttribute]
#endif
                public string path;

            public static implicit operator ZResource.Resource(ResourceConfig config)
            {
                ZResource.Resource res = new ZResource.Resource {key = config.key};
                res.resource = AssetDatabase.LoadAssetAtPath(config.path, typeof (Object));
                return res;
            }

            public ZResource.Resource ToZResource()
            {
                return this;
            }

            public void Initialize()
            {
                key = "";
                path = "Assets/";
            }
        }

        public class GroupConfig
        {
#if USING_XML
            [XmlAttribute]
#endif
                public int id;

#if USING_XML
            [XmlIgnore]
#endif
                public bool foldout;

#if USING_XML
            [XmlArray, XmlArrayItem("resource")]
#endif
                public List<ResourceConfig> resources;

            public void Initialize()
            {
                foldout = true;
                resources = new List<ResourceConfig>();
                ResourceConfig res = new ResourceConfig();
                res.Initialize();
                resources.Add(res);
            }
        }

        public class GroupSelection
        {
#if USING_XML
            [XmlAttribute]
#endif
                public int id;

#if USING_XML
            [XmlAttribute]
#endif
                public string name;

#if USING_XML
            [XmlAttribute]
#endif
                public string desc;

            public override string ToString()
            {
                return string.Format("[{0}]: {1}: {2}", id, name, desc);
            }
        }

#if USING_XML
        [XmlRoot]
#endif
        public class ZConfig
        {
#if USING_XML
            [XmlArray, XmlArrayItem("selection")]
#endif
                public List<GroupSelection> selections;

#if USING_XML
            [XmlArray, XmlArrayItem("group")]
#endif
                public List<GroupConfig> groups;
        }

        #endregion
    }
}