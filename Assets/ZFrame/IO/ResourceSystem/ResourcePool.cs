using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
#if UNITY_EDITOR
using System.Xml.Serialization;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
#endif
using UnityEngine;
using Object = UnityEngine.Object;

public class ResourcePool : MonoBehaviour
{
    public ResourceAsset asset;
}

public class ResourceAsset : ScriptableObject
{
    [HideInInspector] public List<Group> groups;

    [Serializable]
    public class Resource
    {
        public string resourceKey;
        public string desc;
        public Object resource;
    }

    [Serializable]
    public class Group
    {
        public string groupName;
        public string desc;
        public List<Resource> resources;
    }
}

#if UNITY_EDITOR
public class ResourcePoolWindow : EditorWindow
{
    private static string Path = "Resources/ResourceAsset.asset";
    private static ResourcePoolWindow _window;

    private ResourceAsset _asset;
    private ReorderableList _list;
    private ResourceAsset.Group _lastGroup;
    private Color _normalColor;
    private Vector2 _pos;

    [MenuItem("Window/Resource Pool")]
    private static void Init()
    {
        GenerateAsset();

        if (!_window)
        {
            _window = GetWindow<ResourcePoolWindow>(false, "Resource Pool");
            _window.minSize = new Vector2(800, 600);
        }
    }

    private static void GenerateAsset()
    {
        if (!Directory.Exists(Application.dataPath + "/Resources"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Resources");
        }
        if (!File.Exists(Application.dataPath + "/" + Path))
        {
            ResourceAsset asset = CreateInstance<ResourceAsset>();
            AssetDatabase.CreateAsset(asset, "Assets/" + Path);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    private void OnEnable()
    {
        _asset = Resources.LoadAssetAtPath<ResourceAsset>("Assets/" + Path);
        _normalColor = GUI.backgroundColor;
    }

    private void OnGUI()
    {
        if (_asset)
        {
            var group = DrawGroupToolbar();
            DrawGroupItems(group);
        }
    }

    private void DrawGroupItems(ResourceAsset.Group group)
    {
        if (group == null)
            return;
        if (group.resources == null)
            group.resources = new List<ResourceAsset.Resource>();

        var buttonWidth = 24f;
        var areaRect = new Rect(3f, 12f + 2*buttonWidth, _window.position.width - 10f,
            _window.position.height - 20f);

        GUILayout.BeginArea(areaRect);
        {
            if (_list == null || _lastGroup != group)
            {
                _lastGroup = group;

                _list = new ReorderableList(group.resources, typeof (ResourceAsset.Resource), true, true,
                    true, true);

                _list.drawHeaderCallback += rect => EditorGUI.LabelField(rect, "Resources");

                _list.drawElementCallback += (rect, index, active, focused) =>
                {
                    if (index >= _list.count) return;

                    var res = group.resources[index];

                    if (string.IsNullOrEmpty(res.resourceKey) ||
                        group.resources.Any(r => r.resourceKey == res.resourceKey && r != res))
                        GUI.backgroundColor = Color.red;

                    res.resourceKey =
                        EditorGUI.TextField(new Rect(rect.x, rect.y, rect.width/4, EditorGUIUtility.singleLineHeight),
                            res.resourceKey);

                    GUI.backgroundColor = _normalColor;

                    res.desc =
                        EditorGUI.TextField(
                            new Rect(rect.x + rect.width/4, rect.y, rect.width/2, EditorGUIUtility.singleLineHeight),
                            res.desc);

                    if (res.resource == null) GUI.backgroundColor = Color.red;

                    res.resource =
                        EditorGUI.ObjectField(
                            new Rect(rect.x + rect.width*3/4, rect.y, rect.width/4, EditorGUIUtility.singleLineHeight),
                            res.resource,
                            typeof (Object), false);

                    GUI.backgroundColor = _normalColor;
                };

                _list.onRemoveCallback += list =>
                {
                    if (EditorUtility.DisplayDialog("Warning!",
                        "Are you sure you want to delete the resource?", "Yes", "No"))
                        ReorderableList.defaultBehaviours.DoRemoveButton(list);
                };

                //_list.onAddDropdownCallback += (rect, list) =>
                //{
                //    var menu = new GenericMenu();
                //    foreach (var gp in _asset.groups)
                //    {
                //        menu.AddItem(new GUIContent("Move To/" + gp.groupName), false, () =>
                //        {
                //            gp.resources.Add(_lastGroup.resources[list.index]);
                //            ReorderableList.defaultBehaviours.DoRemoveButton(list);
                //        });
                //    }
                //    menu.ShowAsContext();
                //};
            }

            _pos = EditorGUILayout.BeginScrollView(_pos);
            {
                _list.DoLayoutList();
                GUILayout.Space(40f);
            }
            EditorGUILayout.EndScrollView();
        }
        GUILayout.EndArea();
    }

    private ResourceAsset.Group DrawGroupToolbar()
    {
        if (_asset.groups == null)
            _asset.groups = new List<ResourceAsset.Group>();

        var buttonWidth = 24f;
        var toobarRect = new Rect(3f, 6f, _window.position.width - 10f - 2*buttonWidth, buttonWidth);
        var toolbarAddRect = new Rect(toobarRect.xMax + 1f, toobarRect.yMin, buttonWidth, buttonWidth);
        var toolbarDelRect = new Rect(toolbarAddRect.xMax, toolbarAddRect.yMin, buttonWidth, buttonWidth);

        var contents = _asset.groups.Select(g => new GUIContent(g.groupName, g.desc)).ToArray();
        var num = EditorPrefs.GetInt("ResourcePoolGroupSelection", 0);

        EditorGUI.BeginChangeCheck();
        num = GUI.Toolbar(toobarRect, num, contents);
        if (EditorGUI.EndChangeCheck())
            EditorPrefs.SetInt("ResourcePoolGroupSelection", num);

        if (GUI.Button(toolbarAddRect, "+", EditorStyles.miniButtonLeft))
        {
            _asset.groups.Add(new ResourceAsset.Group {groupName = "NewGroup", desc = "Description"});
        }
        if (GUI.Button(toolbarDelRect, "-", EditorStyles.miniButtonRight))
        {
            if (_asset.groups.Count > num && EditorUtility.DisplayDialog("Warning!",
                "Group resources will be deleted too. Are you sure you want to delete the group?", "Yes", "No"))
                _asset.groups.RemoveAt(num);
        }

        if (_asset.groups.Count > num)
        {
            var group = _asset.groups[num];
            var labelWidth = 85f;

            EditorGUI.LabelField(new Rect(3f, 12f + buttonWidth, labelWidth,
                EditorGUIUtility.singleLineHeight), "Resource Key");

            if (string.IsNullOrEmpty(group.groupName) ||
                _asset.groups.Any(g => g.groupName == group.groupName && g != group))
                GUI.backgroundColor = Color.red;

            group.groupName =
                EditorGUI.TextField(new Rect(3f + labelWidth, 12f + buttonWidth, toobarRect.width/2f,
                    EditorGUIUtility.singleLineHeight), group.groupName);

            GUI.backgroundColor = _normalColor;

            EditorGUI.LabelField(new Rect(3f + labelWidth + toobarRect.width/2f, 12f + buttonWidth, labelWidth,
                EditorGUIUtility.singleLineHeight), "Description");
            group.desc =
                EditorGUI.TextField(
                    new Rect(3f + 2*labelWidth + toobarRect.width/2f, 12f + buttonWidth,
                        toobarRect.width/2 - 2*labelWidth - 3f,
                        EditorGUIUtility.singleLineHeight), group.desc);
        }

        return _asset.groups.Count > num ? _asset.groups[num] : null;
    }

    [MenuItem("Assets/Generate XML")]
    private static void ExportXml()
    {
        ResourceAsset asset = (ResourceAsset) Selection.activeObject;
        var path = AssetDatabase.GetAssetPath(asset).Replace(".asset", ".xml");
        XmlDocument doc = new XmlDocument();
        var root = doc.CreateElement("resourcePool");
        doc.AppendChild(root);
        foreach (ResourceAsset.Group group in asset.groups)
        {
            var groupElement = doc.CreateElement("group");
            root.AppendChild(groupElement);
            groupElement.SetAttribute("name", group.groupName);
            groupElement.SetAttribute("desc", group.desc);

            foreach (ResourceAsset.Resource resource in group.resources)
            {
                var resourceElement = doc.CreateElement("resource");
                groupElement.AppendChild(resourceElement);
                resourceElement.SetAttribute("key", resource.resourceKey);
                resourceElement.SetAttribute("desc", resource.desc);
                resourceElement.SetAttribute("path", AssetDatabase.GetAssetPath(resource.resource));
            }
        }
        doc.Save(path);
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/Generate XML", true)]
    private static bool ExportXmlValidation()
    {
        return Selection.activeObject.GetType() == typeof (ResourceAsset);
    }

    [MenuItem("Assets/Import From XML")]
    private static void ImportXml()
    {
        ResourceAsset asset = (ResourceAsset) Selection.activeObject;
        var path = AssetDatabase.GetAssetPath(asset).Replace(".asset", ".xml");
        if (File.Exists(path))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            asset.groups = new List<ResourceAsset.Group>();
            foreach (XmlNode groupNode in doc.SelectNodes("resourcePool/group"))
            {
                var group = new ResourceAsset.Group();
                group.groupName = groupNode.Attributes["name"].Value;
                group.desc = groupNode.Attributes["desc"].Value;

                group.resources = new List<ResourceAsset.Resource>();
                foreach (XmlNode resourceNode in groupNode.SelectNodes("resource"))
                {
                    var resource = new ResourceAsset.Resource();
                    resource.resourceKey = resourceNode.Attributes["key"].Value;
                    resource.desc = resourceNode.Attributes["desc"].Value;
                    resource.resource = AssetDatabase.LoadAssetAtPath(resourceNode.Attributes["path"].Value,
                        typeof (Object));
                    group.resources.Add(resource);
                }
                asset.groups.Add(group);
            }

            AssetDatabase.Refresh();
        }
    }

    [MenuItem("Assets/Import From XML", true)]
    private static bool ImportXmlValidation()
    {
        return Selection.activeObject.GetType() == typeof(ResourceAsset) && File.Exists(AssetDatabase.GetAssetPath(Selection.activeObject).Replace(".asset", ".xml"));
    }
}
#endif