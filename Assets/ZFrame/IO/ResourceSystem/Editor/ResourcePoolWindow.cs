using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using ZFrame.Utilities;
using Object = UnityEngine.Object;

namespace ZFrame.IO.ResourceSystem
{
    public class ResourcePoolWindow : EditorWindow
    {
        private const string Path = "Resources/ResourceAsset.asset";
        private static ResourcePoolWindow _window;

        private ResourceAsset _asset;
        private ReorderableList _list;
        private ResourceAsset.Group _lastGroup;
        private Color _normalColor;
        private Vector2 _pos;
        private Vector2 _offset = new Vector2(3f, 6f);
        private Vector2 _defaultSize = new Vector2(25f, EditorGUIUtility.singleLineHeight);
        private float _itemStartY;
        private int maxPerLine = 7;
        private int _selected;

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
                ResourceAsset.Group group = DrawGroupToolbar();
                DrawGroupItems(group);
            }
        }

        private void DrawGroupItems(ResourceAsset.Group group)
        {
            if (group == null)
                return;
            if (group.resources == null)
                group.resources = new List<ResourceAsset.Resource>();

            Rect areaRect = new Rect(_offset.x, _itemStartY + _offset.y, _window.position.width - 2 * _offset.x,
                _window.position.height - 2 * _offset.y);

            GUILayout.BeginArea(areaRect);
            {
                if (_list == null || _lastGroup != group)
                {
                    _lastGroup = group;

                    _list = new ReorderableList(group.resources, typeof(ResourceAsset.Resource), true, true,
                        true, true);

                    _list.drawHeaderCallback += rect => EditorGUI.LabelField(rect, "Resources");

                    _list.drawElementCallback += (rect, index, active, focused) =>
                    {
                        if (index >= _list.count) return;

                        ResourceAsset.Resource res = group.resources[index];

                        if (string.IsNullOrEmpty(res.resourceKey) ||
                            group.resources.Any(r => r.resourceKey == res.resourceKey && r != res))
                            GUI.backgroundColor = Color.red;

                        res.resourceKey =
                            EditorGUI.TextField(new Rect(rect.x, rect.y, rect.width / 4, EditorGUIUtility.singleLineHeight),
                                res.resourceKey);

                        GUI.backgroundColor = _normalColor;

                        res.desc =
                            EditorGUI.TextField(
                                new Rect(rect.x + rect.width / 4, rect.y, rect.width / 2, EditorGUIUtility.singleLineHeight),
                                res.desc);

                        if (res.resource == null) GUI.backgroundColor = Color.red;

                        res.resource =
                            EditorGUI.ObjectField(
                                new Rect(rect.x + rect.width * 3 / 4, rect.y, rect.width / 4, EditorGUIUtility.singleLineHeight),
                                res.resource,
                                typeof(Object), false);

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

            Rect assetRect = new Rect(_offset.x, _offset.y, _window.position.width - 2 * _offset.x, _defaultSize.y);

            GUILayout.BeginArea(assetRect);
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.ObjectField(_asset, typeof(ResourceAsset), false);
                    TextAsset text = null;
                    EditorGUI.BeginChangeCheck();
                    text = EditorGUILayout.ObjectField(text, typeof(TextAsset), false).To<TextAsset>();
                    if (EditorGUI.EndChangeCheck())
                        ImportXml(text, _asset);
                }
                EditorGUILayout.EndHorizontal();
            }
            GUILayout.EndArea();

            Rect toolbarRect = new Rect(assetRect.xMin, assetRect.yMax + _offset.y,
                _window.position.width - 3 * _offset.x - 2 * _defaultSize.x, _defaultSize.y);

            Rect toolbarAddRect = new Rect(toolbarRect.xMax + _offset.x, toolbarRect.yMin, _defaultSize.x, _defaultSize.y);
            Rect toolbarDelRect = new Rect(toolbarAddRect.xMax, toolbarAddRect.yMin, _defaultSize.x, _defaultSize.y);

            var lines = (_asset.groups.Count/(float) maxPerLine).RoundUp();

            for (int i = 0; i < lines; i++)
            {
                GUIContent[] contents = _asset.groups.Where(g => _asset.groups.IndexOf(g).Between(i * maxPerLine - 1, (i + 1) * maxPerLine)).Select(g => new GUIContent(g.groupName, g.desc)).ToArray();
                _selected = GUI.Toolbar(toolbarRect, _selected - maxPerLine * i, contents) + maxPerLine * i;
                if(i < lines)
                toolbarRect = new Rect(toolbarRect.xMin, toolbarRect.yMax, toolbarRect.width, toolbarRect.height);
            }

            /*
            GUIContent[] contents = _asset.groups.Select(g => new GUIContent(g.groupName, g.desc)).ToArray();
            int num = EditorPrefs.GetInt("ResourcePoolGroupSelection", 0);

            EditorGUI.BeginChangeCheck();
            num = GUI.Toolbar(toolbarRect, num, contents);
            if (EditorGUI.EndChangeCheck())
                EditorPrefs.SetInt("ResourcePoolGroupSelection", num);
             * */

            if (GUI.Button(toolbarAddRect, "+", EditorStyles.miniButtonLeft))
            {
                _asset.groups.Add(new ResourceAsset.Group { groupName = "NewGroup", desc = "Description" });
            }
            if (GUI.Button(toolbarDelRect, "-", EditorStyles.miniButtonRight))
            {
                if (_asset.groups.Count > _selected && _selected >= 0 && EditorUtility.DisplayDialog("Warning!",
                    "Group resources will be deleted too. Are you sure you want to delete the group?", "Yes", "No"))
                    _asset.groups.RemoveAt(_selected);
            }

            if (_asset.groups.Count > _selected && _selected >= 0)
            {
                ResourceAsset.Group group = _asset.groups[_selected];

                Rect nameLabelRect = new Rect(_offset.x, toolbarRect.yMax + _offset.y, 80f, _defaultSize.y);
                Rect nameRect = new Rect(nameLabelRect.xMax, nameLabelRect.yMin, toolbarRect.width / 4f, _defaultSize.y);
                Rect descLabelRect = new Rect(nameRect.xMax, nameLabelRect.yMin, 80f, _defaultSize.y);
                Rect descRect = new Rect(descLabelRect.xMax + _offset.x, nameLabelRect.yMin,
                    toolbarRect.xMax - descLabelRect.xMax - _offset.x, _defaultSize.y);

                EditorGUI.LabelField(nameLabelRect, "GroupName");

                if (string.IsNullOrEmpty(group.groupName) ||
                    _asset.groups.Any(g => g.groupName == group.groupName && g != group))
                    GUI.backgroundColor = Color.red;

                group.groupName =
                    EditorGUI.TextField(nameRect, group.groupName);

                GUI.backgroundColor = _normalColor;

                EditorGUI.LabelField(descLabelRect, "Description");
                group.desc =
                    EditorGUI.TextField(descRect, group.desc);

                _itemStartY = descRect.yMax;
            }

            return _asset.groups.Count > _selected && _selected >= 0 ? _asset.groups[_selected] : null;
        }

        [MenuItem("Assets/Generate XML")]
        private static void ExportXml()
        {
            ResourceAsset asset = (ResourceAsset)Selection.activeObject;
            string path = AssetDatabase.GetAssetPath(asset).Replace(".asset", ".xml");
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("resourcePool");
            root.SetAttribute("date", DateTime.Now.ToString("g"));
            doc.AppendChild(root);
            foreach (ResourceAsset.Group group in asset.groups)
            {
                XmlElement groupElement = doc.CreateElement("group");
                root.AppendChild(groupElement);
                groupElement.SetAttribute("name", group.groupName);
                groupElement.SetAttribute("desc", group.desc);

                foreach (ResourceAsset.Resource resource in group.resources)
                {
                    XmlElement resourceElement = doc.CreateElement("resource");
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
            return Selection.activeObject && Selection.activeObject.GetType() == typeof(ResourceAsset);
        }

        [MenuItem("Assets/Import From XML")]
        private static void ImportXml()
        {
            ResourceAsset asset = (ResourceAsset)Selection.activeObject;
            string path = AssetDatabase.GetAssetPath(asset).Replace(".asset", ".xml");
            if (File.Exists(path))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                asset.groups = new List<ResourceAsset.Group>();
                foreach (XmlNode groupNode in doc.SelectNodes("resourcePool/group"))
                {
                    ResourceAsset.Group group = new ResourceAsset.Group();
                    group.groupName = groupNode.Attributes["name"].Value;
                    group.desc = groupNode.Attributes["desc"].Value;

                    group.resources = new List<ResourceAsset.Resource>();
                    foreach (XmlNode resourceNode in groupNode.SelectNodes("resource"))
                    {
                        ResourceAsset.Resource resource = new ResourceAsset.Resource();
                        resource.resourceKey = resourceNode.Attributes["key"].Value;
                        resource.desc = resourceNode.Attributes["desc"].Value;
                        resource.resource = AssetDatabase.LoadAssetAtPath(resourceNode.Attributes["path"].Value,
                            typeof(Object));
                        group.resources.Add(resource);
                    }
                    asset.groups.Add(group);
                }

                AssetDatabase.Refresh();
            }
        }

        private static void ImportXml(TextAsset text, ResourceAsset asset)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(text.text);
            asset.groups = new List<ResourceAsset.Group>();
            foreach (XmlNode groupNode in doc.SelectNodes("resourcePool/group"))
            {
                ResourceAsset.Group group = new ResourceAsset.Group();
                group.groupName = groupNode.Attributes["name"].Value;
                group.desc = groupNode.Attributes["desc"].Value;

                group.resources = new List<ResourceAsset.Resource>();
                foreach (XmlNode resourceNode in groupNode.SelectNodes("resource"))
                {
                    ResourceAsset.Resource resource = new ResourceAsset.Resource();
                    resource.resourceKey = resourceNode.Attributes["key"].Value;
                    resource.desc = resourceNode.Attributes["desc"].Value;
                    resource.resource = AssetDatabase.LoadAssetAtPath(resourceNode.Attributes["path"].Value,
                        typeof(Object));
                    group.resources.Add(resource);
                }
                asset.groups.Add(group);
            }

            AssetDatabase.Refresh();
        }

        [MenuItem("Assets/Import From XML", true)]
        private static bool ImportXmlValidation()
        {
            return Selection.activeObject && Selection.activeObject.GetType() == typeof(ResourceAsset) &&
                   File.Exists(AssetDatabase.GetAssetPath(Selection.activeObject).Replace(".asset", ".xml"));
        }
    }

}