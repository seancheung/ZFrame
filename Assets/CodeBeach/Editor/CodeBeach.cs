using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.CSharp;
using UnityEditor;
using UnityEngine;

public class CodeBeach : EditorWindow
{
    private Vector2 scroll;
    private Vector2 previewScroll;
    private string className = "";
    private string text = "";
    private float offset = 5;
    private string path = "Assets";

    private static readonly List<CodeMemberMethodFormatter> monoMethods = new List<CodeMemberMethodFormatter>();

    #region Methods

    private enum MethodGroup
    {
        Common,
        Network,
        Physics,
        Input,
        Rendering,
        Other
    }

    private class CodeMemberMethodFormatter : CodeMemberMethod
    {
        private string _comment;

        public string Comment
        {
            get { return _comment; }
            set
            {
                _comment = value;
                Comments.Add(new CodeCommentStatement("<summary>", true));
                Comments.Add(new CodeCommentStatement(value.Replace(Name, string.Format("<see cref=\"{0}\"/>", Name)),
                    true));
                Comments.Add(new CodeCommentStatement("</summary>", true));
            }
        }

        public MethodGroup Tag { get; set; }
    }

    #endregion

    [MenuItem("Assets/Create/CodeBeach Script...", priority = 80)]
    private static void Init()
    {
        CodeBeach win = GetWindow<CodeBeach>("CodeBeach");
        win.minSize = new Vector2(480, 480);
    }

    private void OnEnable()
    {
        className = string.Empty;
        text = string.Empty;
        LoadTemplate();
        UpdatePath();
        UpdatePreview();
    }

    private void OnSelectionChange()
    {
        UpdatePath();
    }

    private void UpdatePath()
    {
        if (!Selection.activeObject) return;
        string tmp = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (!Directory.Exists(tmp))
        {
            tmp = tmp.Remove(tmp.LastIndexOf("/"));
        }
        path = tmp;
        Repaint();
    }

    private void OnGUI()
    {
        Rect nameRect = new Rect(offset, offset, position.width - 2*offset, 20);
        EditorGUI.BeginChangeCheck();
        className = EditorGUI.TextField(nameRect, "Name", className);
        if (EditorGUI.EndChangeCheck())
            UpdatePreview();
        nameRect.y += 20;
        path = EditorGUI.TextField(nameRect, "Path", path);


        Rect scrollRect = new Rect(offset, offset + nameRect.yMax, (position.width)/3 + 6*offset,
            position.height - nameRect.yMax - 2*offset);
        GUILayout.BeginArea(scrollRect);
        {
            scroll = EditorGUILayout.BeginScrollView(scroll);
            {
                EditorGUILayout.BeginVertical();
                {
                    foreach (MethodGroup group in Enum.GetValues(typeof (MethodGroup)))
                    {
                        if (!monoMethods.Exists(p => p.Tag == group))
                            continue;
                        bool fold = EditorPrefs.GetBool(group.ToString(), false);
                        EditorGUI.BeginChangeCheck();
                        fold = EditorGUILayout.Foldout(fold, group.ToString());
                        if (fold)
                        {
                            EditorGUILayout.BeginVertical(EditorStyles.textArea);
                            {
                                MethodGroup tag = group;
                                foreach (CodeMemberMethodFormatter method in monoMethods.Where(m => m.Tag == tag))
                                {
                                    EditorGUI.BeginChangeCheck();
                                    bool val = EditorGUILayout.Toggle(new GUIContent(method.Name, method.Comment),
                                        EditorPrefs.GetBool(method.Name, false));
                                    if (EditorGUI.EndChangeCheck())
                                    {
                                        EditorPrefs.SetBool(method.Name, val);
                                        UpdatePreview();
                                    }
                                }
                            }
                            EditorGUILayout.EndVertical();
                        }
                        if (EditorGUI.EndChangeCheck())
                            EditorPrefs.SetBool(group.ToString(), fold);

                        EditorGUILayout.Separator();
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView();
        }
        GUILayout.EndArea();

        Rect previewRect = new Rect(scrollRect.xMax + offset, scrollRect.yMin,
            position.width - scrollRect.xMax - 2*offset,
            scrollRect.height - 2*offset - 20);

        GUILayout.BeginArea(previewRect);
        {
            previewScroll = EditorGUILayout.BeginScrollView(previewScroll);
            {
                EditorGUILayout.BeginHorizontal();
                {
                    Rect rect = GUILayoutUtility.GetRect(new GUIContent(text), EditorStyles.textArea,
                        GUILayout.ExpandWidth(true),
                        GUILayout.ExpandHeight(true));
                    EditorGUI.SelectableLabel(rect, text, EditorStyles.textArea);
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }
        GUILayout.EndArea();


        Rect buttonRect = new Rect(previewRect.xMin, previewRect.yMax + offset, previewRect.width, 20);
        if (GUI.Button(buttonRect, "Create"))
        {
            Create();
        }
    }

    private void UpdatePreview()
    {
        string tmp = string.IsNullOrEmpty(className) ? "NewMonoBehaviour" : className;
        StringBuilder sb = new StringBuilder();
        TextWriter tw = new StringWriter(sb);
        ICodeGenerator codeGenerator = new CSharpCodeProvider().CreateGenerator();
        CodeGeneratorOptions options = new CodeGeneratorOptions {BlankLinesBetweenMembers = true, BracingStyle = "C"};
        CodeTypeDeclaration cls = new CodeTypeDeclaration(tmp);
        cls.BaseTypes.Add(typeof (MonoBehaviour));
        cls.IsClass = true;
        cls.Attributes = MemberAttributes.Public;
        foreach (CodeMemberMethodFormatter method in monoMethods)
        {
            if (EditorPrefs.GetBool(method.Name, false))
                cls.Members.Add(method);
        }
        codeGenerator.GenerateCodeFromType(cls, tw, options);
        tw.Flush();
        tw.Close();

        text = sb.ToString();
    }

    private void Create()
    {
        if (!Directory.Exists(path) || string.IsNullOrEmpty(className))
            return;

        string fileName = path + "/" + className + ".cs";

        TextWriter tw = new StreamWriter(new FileStream(fileName, FileMode.Create));
        ICodeGenerator codeGenerator = new CSharpCodeProvider().CreateGenerator();
        CodeGeneratorOptions options = new CodeGeneratorOptions
        {
            BlankLinesBetweenMembers = true,
            BracingStyle = "C"
        };
        CodeTypeDeclaration cls = new CodeTypeDeclaration(className);
        cls.BaseTypes.Add(typeof (MonoBehaviour));
        cls.IsClass = true;
        cls.Attributes = MemberAttributes.Public;

        foreach (CodeMemberMethodFormatter method in monoMethods)
        {
            if (EditorPrefs.GetBool(method.Name, false))
                cls.Members.Add(method);
        }

        codeGenerator.GenerateCodeFromType(cls, tw, options);

        tw.Flush();
        tw.Close();
        AssetDatabase.Refresh();
    }

    private void LoadTemplate()
    {
        if (!File.Exists("Assets/CodeBeach/Template.xml"))
            return;

        monoMethods.Clear();

        XmlDocument doc = new XmlDocument();
        doc.Load("Assets/CodeBeach/Template.xml");

        foreach (XmlNode node in doc.SelectNodes("template/MonoBehaviour/method"))
        {
            string name = node.Attributes["name"].Value;
            CodeMemberMethodFormatter formatter = new CodeMemberMethodFormatter
            {
                Name = name,
                ReturnType = new CodeTypeReference(typeof (void)),
                Attributes = MemberAttributes.Private
            };

            XmlNode comment = node.SelectSingleNode("comment");
            if (comment != null)
                formatter.Comment = comment.InnerText;

            XmlAttribute tag = node.Attributes["tag"];
            if (tag != null)
                formatter.Tag = (MethodGroup) Enum.Parse(typeof (MethodGroup), tag.Value);

            XmlNodeList parameters = node.SelectNodes("parameter");
            CodeParameterDeclarationExpressionCollection cdc = new CodeParameterDeclarationExpressionCollection();
            foreach (XmlNode parameter in parameters)
            {
                cdc.Add(new CodeParameterDeclarationExpression(parameter.Attributes["type"].Value,
                    parameter.Attributes["name"].Value));
                if (comment != null)
                    formatter.Comments.Add(
                        new CodeCommentStatement(string.Format("<param name=\"{0}\"></param>", parameter.Attributes["name"].Value), true));
            }
            formatter.Parameters.AddRange(cdc);

            if (node.Attributes["override"] != null)
                formatter.Attributes |= MemberAttributes.Override;

            XmlAttribute ret = node.Attributes["return"];
            if (ret != null)
                formatter.ReturnType = new CodeTypeReference(ret.Value);

            monoMethods.Add(formatter);
        }
    }
}