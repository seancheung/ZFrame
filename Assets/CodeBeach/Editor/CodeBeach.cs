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
        public string Comment { get; private set; }

        public MethodGroup Tag { get; set; }

        public CodeMemberMethodFormatter(string name, string comment,
            params CodeParameterDeclarationExpression[] parameters)
        {
            Name = name;
            ReturnType = new CodeTypeReference(typeof (void));
            Attributes = MemberAttributes.Private;
            Comment = comment;
            Comments.Add(new CodeCommentStatement("<summary>", true));
            Comments.Add(new CodeCommentStatement(comment.Replace(name, string.Format("<see cref=\"{0}\"/>", name)),
                true));
            Comments.Add(new CodeCommentStatement("</summary>", true));
            if (parameters != null)
            {
                Parameters.AddRange(parameters);
                foreach (CodeParameterDeclarationExpression parameter in parameters)
                {
                    Comments.Add(new CodeCommentStatement(
                        string.Format("<param name=\"{0}\"></param>", parameter.Name), true));
                }
            }
        }

        public CodeMemberMethodFormatter(string name, string comment,
            CodeParameterDeclarationExpressionCollection parameters) : this(name, comment)
        {
            if (parameters != null)
            {
                Parameters.AddRange(parameters);
                foreach (CodeParameterDeclarationExpression parameter in parameters)
                {
                    Comments.Add(new CodeCommentStatement(
                        string.Format("<param name=\"{0}\"></param>", parameter.Name), true));
                }
            }
        }
    }

    private static List<CodeMemberMethodFormatter> methods = new List<CodeMemberMethodFormatter>
    {
        new CodeMemberMethodFormatter("Awake", "is called when the script instance is being loaded"),
        new CodeMemberMethodFormatter("OnEnable", "is called when the object becomes enabled and active"),
        new CodeMemberMethodFormatter("Start",
            "is called on the frame when a script is enabled just before any of the Update methods is called the first time"),
        new CodeMemberMethodFormatter("Update", "is called every frame, if the MonoBehaviour is enabled"),
        new CodeMemberMethodFormatter("LateUpdate", "is called every frame, if the Behaviour is enabled"),
        new CodeMemberMethodFormatter("FixedUpdate",
            "is called every fixed framerate frame, if the MonoBehaviour is enabled"),
        new CodeMemberMethodFormatter("OnDisable", "is called when the behaviour becomes disabled or inactive"),
        new CodeMemberMethodFormatter("OnDestroy", "is called when the MonoBehaviour will be destroyed"),
        new CodeMemberMethodFormatter("OnLevelWasLoaded", "is called after a new level was loaded",
            new CodeParameterDeclarationExpression(typeof (int), "level")),
        new CodeMemberMethodFormatter("OnApplicationQuit", "sent to all game objects before the application is quit"),
        new CodeMemberMethodFormatter("OnApplicationFocus",
            "sent to all game objects when the player gets or loses focus"),
        new CodeMemberMethodFormatter("OnApplicationPause", "sent to all game objects when the player pauses"),
        new CodeMemberMethodFormatter("OnCollisionEnter",
            "is called when this collider/rigidbody has begun touching another rigidbody/collider",
            new CodeParameterDeclarationExpression(typeof (Collision), "collision")),
        new CodeMemberMethodFormatter("OnCollisionStay",
            "is called once per frame for every collider/rigidbody that is touching rigidbody/collider",
            new CodeParameterDeclarationExpression(typeof (Collision), "collision")) {Tag = MethodGroup.Physics},
        new CodeMemberMethodFormatter("OnCollisionExit",
            "is called when this collider/rigidbody has stopped touching another rigidbody/collider",
            new CodeParameterDeclarationExpression(typeof (Collision), "collision")) {Tag = MethodGroup.Physics},
        new CodeMemberMethodFormatter("OnTriggerEnter", "is called when the Collider other enters the trigger",
            new CodeParameterDeclarationExpression(typeof (Collider), "other")) {Tag = MethodGroup.Physics},
        new CodeMemberMethodFormatter("OnTriggerStay",
            "is called once per frame for every Collider other that is touching the trigger",
            new CodeParameterDeclarationExpression(typeof (Collider), "other")) {Tag = MethodGroup.Physics},
        new CodeMemberMethodFormatter("OnTriggerExit",
            "is called when the Collider other has stopped touching the trigger",
            new CodeParameterDeclarationExpression(typeof (Collider), "other")) {Tag = MethodGroup.Physics},
        new CodeMemberMethodFormatter("OnMouseDown",
            "is called when the user has pressed the mouse button while over the GUIElement or Collider")
        {
            Tag = MethodGroup.Input
        },
        new CodeMemberMethodFormatter("OnMouseUp",
            "is called when the user has released the mouse button") {Tag = MethodGroup.Input},
        new CodeMemberMethodFormatter("OnMouseUpAsButton",
            "is only called when the mouse is released over the same GUIElement or Collider as it was pressed")
        {
            Tag = MethodGroup.Input
        },
        new CodeMemberMethodFormatter("OnMouseOver",
            "is called every frame while the mouse is over the GUIElement or Collider") {Tag = MethodGroup.Input},
        new CodeMemberMethodFormatter("OnMouseEnter",
            "is called when the mouse entered the GUIElement or Collider") {Tag = MethodGroup.Input},
        new CodeMemberMethodFormatter("OnMouseDrag",
            "is called when the user has clicked on a GUIElement or Collider and is still holding down the mouse")
        {
            Tag = MethodGroup.Input
        },
        new CodeMemberMethodFormatter("OnMouseExit",
            "is called when the mouse is not any longer over the GUIElement or Collider") {Tag = MethodGroup.Input},
        new CodeMemberMethodFormatter("OnBecameVisible",
            "is called when the renderer became visible by any camera") {Tag = MethodGroup.Rendering},
        new CodeMemberMethodFormatter("OnBecameInvisible",
            "is called when the renderer is no longer visible by any camera") {Tag = MethodGroup.Rendering},
        new CodeMemberMethodFormatter("OnWillRenderObject",
            "is called once for each camera if the object is visible") {Tag = MethodGroup.Rendering},
        new CodeMemberMethodFormatter("OnGUI",
            "is called for rendering and handling GUI events"),
        new CodeMemberMethodFormatter("Reset",
            "to default values") {Tag = MethodGroup.Other},
        new CodeMemberMethodFormatter("OnTransformChildrenChange",
            "is called when the list of children of the transform of the GameObject has changed")
        {
            Tag = MethodGroup.Other
        },
        new CodeMemberMethodFormatter("OnTransformParentChanged",
            "is called when the parent property of the transform of the GameObject has changed")
        {
            Tag = MethodGroup.Other
        },
        new CodeMemberMethodFormatter("OnServerInitialized",
            "is called on the server whenever a Network.InitializeServer was invoked and has completed")
        {
            Tag = MethodGroup.Network
        },
        new CodeMemberMethodFormatter("OnSerializeNetworkView",
            "is used to customize synchronization of variables in a script watched by a network view",
            new CodeParameterDeclarationExpression(typeof (BitStream), "stream"),
            new CodeParameterDeclarationExpression(typeof (NetworkMessageInfo), "info")) {Tag = MethodGroup.Network},
        new CodeMemberMethodFormatter("OnRenderObject",
            "is called after camera has rendered the scene") {Tag = MethodGroup.Rendering},
        new CodeMemberMethodFormatter("OnRenderImage",
            "is called after all rendering is complete to render image",
            new CodeParameterDeclarationExpression(typeof (RenderTexture), "src"),
            new CodeParameterDeclarationExpression(typeof (RenderTexture), "dest")) {Tag = MethodGroup.Rendering},
        new CodeMemberMethodFormatter("OnPreRender",
            "is called before a camera starts rendering the scene") {Tag = MethodGroup.Rendering},
        new CodeMemberMethodFormatter("OnPreCull",
            "is called before a camera culls the scene") {Tag = MethodGroup.Rendering},
        new CodeMemberMethodFormatter("OnPostRender",
            "is called after a camera finished rendering the scene") {Tag = MethodGroup.Rendering},
        new CodeMemberMethodFormatter("OnPlayerDisconnected",
            "is called on the server whenever a player disconnected from the server",
            new CodeParameterDeclarationExpression(typeof (NetworkPlayer), "player")) {Tag = MethodGroup.Network},
        new CodeMemberMethodFormatter("OnPlayerConnected",
            "is called on the server whenever a new player has successfully connected",
            new CodeParameterDeclarationExpression(typeof (NetworkPlayer), "player")) {Tag = MethodGroup.Network},
        new CodeMemberMethodFormatter("OnParticleCollision",
            "is called when a particle hits a collider",
            new CodeParameterDeclarationExpression(typeof (GameObject), "other")) {Tag = MethodGroup.Physics},
        new CodeMemberMethodFormatter("OnNetworkInstantiate",
            "is called on objects which have been network instantiated with Network.Instantiate",
            new CodeParameterDeclarationExpression(typeof (NetworkMessageInfo), "info")) {Tag = MethodGroup.Network},
        new CodeMemberMethodFormatter("OnMasterServerEvent",
            "is called on clients or servers when reporting events from the MasterServer",
            new CodeParameterDeclarationExpression(typeof (MasterServerEvent), "msEvent"))
        {
            Tag = MethodGroup.Network
        },
        new CodeMemberMethodFormatter("OnJointBreak",
            "is called when a joint attached to the same game object broke",
            new CodeParameterDeclarationExpression(typeof (float), "breakForce")) {Tag = MethodGroup.Other},
        new CodeMemberMethodFormatter("OnFailedToConnectToMasterServer",
            "is called on clients or servers when there is a problem connecting to the MasterServer",
            new CodeParameterDeclarationExpression(typeof (NetworkConnectionError), "info"))
        {
            Tag = MethodGroup.Network
        },
        new CodeMemberMethodFormatter("OnFailedToConnect",
            "is called on the client when a connection attempt fails for some reason",
            new CodeParameterDeclarationExpression(typeof (NetworkConnectionError), "error"))
        {
            Tag = MethodGroup.Network
        },
        new CodeMemberMethodFormatter("OnDrawGizmosSelected",
            "is implemented if you want to draw gizmos only if the object is selected") {Tag = MethodGroup.Other},
        new CodeMemberMethodFormatter("OnDrawGizmos",
            "is implemented if you want to draw gizmos that are also pickable and always drawn")
        {
            Tag = MethodGroup.Other
        },
        new CodeMemberMethodFormatter("OnDisconnectedFromServer",
            "is called on the client when the connection was lost or you disconnected from the server",
            new CodeParameterDeclarationExpression(typeof (NetworkDisconnection), "info"))
        {
            Tag = MethodGroup.Network
        },
        new CodeMemberMethodFormatter("OnControllerColliderHit",
            "is called when the controller hits a collider while performing a Move",
            new CodeParameterDeclarationExpression(typeof (ControllerColliderHit), "hit"))
        {
            Tag = MethodGroup.Physics
        },
        new CodeMemberMethodFormatter("OnConnectedToServer",
            "is called on the client when you have successfully connected to a server") {Tag = MethodGroup.Network},
        new CodeMemberMethodFormatter("OnCollisionStay2D",
            "sent each frame where a collider on another object is touching this object's collider (2D physics only)",
            new CodeParameterDeclarationExpression(typeof (Collision2D), "coll")) {Tag = MethodGroup.Physics},
        new CodeMemberMethodFormatter("OnCollisionExit2D",
            "sent when a collider on another object stops touching this object's collider (2D physics only)",
            new CodeParameterDeclarationExpression(typeof (Collision2D), "coll")) {Tag = MethodGroup.Physics},
        new CodeMemberMethodFormatter("OnCollisionEnter2D",
            "sent when an incoming collider makes contact with this object's collider (2D physics only)",
            new CodeParameterDeclarationExpression(typeof (Collision2D), "coll")) {Tag = MethodGroup.Physics},
        new CodeMemberMethodFormatter("OnAudioFilterRead",
            "is implemented, Unity will insert a custom filter into the audio DSP chain",
            new CodeParameterDeclarationExpression(typeof (float[]), "data"),
            new CodeParameterDeclarationExpression(typeof (int), "channels")) {Tag = MethodGroup.Other},
        new CodeMemberMethodFormatter("OnAnimatorMove",
            "is the callback for processing animation movements for modifying root motion")
        {
            Tag = MethodGroup.Other
        },
        new CodeMemberMethodFormatter("OnAnimatorIK",
            "is the callback for setting up animation IK (inverse kinematics)",
            new CodeParameterDeclarationExpression(typeof (int), "layerIndex")) {Tag = MethodGroup.Other}
    };

    #endregion

    [MenuItem("Window/CodeBeach/Window")]
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
        methods = methods.OrderBy(m => m.Name).OrderBy(m => m.Tag).ToList();
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
        className = EditorGUI.TextField(nameRect, "Name", className);
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
                        if (!methods.Exists(p => p.Tag == group))
                            continue;
                        bool fold = EditorPrefs.GetBool(group.ToString(), false);
                        EditorGUI.BeginChangeCheck();
                        fold = EditorGUILayout.Foldout(fold, group.ToString());
                        if (fold)
                        {
                            EditorGUILayout.BeginVertical(EditorStyles.textArea);
                            {
                                MethodGroup tag = group;
                                foreach (CodeMemberMethodFormatter method in methods.Where(m => m.Tag == tag))
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
        foreach (CodeMemberMethodFormatter method in methods)
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

        foreach (CodeMemberMethodFormatter method in methods)
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
        methods.Clear();
        XmlDocument doc = new XmlDocument();
        doc.Load("Assets/CodeBeach/Template.xml");
        foreach (XmlNode node in doc.SelectNodes("template/method"))
        {
            string name = node.Attributes["name"].Value;
            string comment = node.Attributes["comment"].Value;
            string tag = node.Attributes["tag"].Value;
            CodeParameterDeclarationExpressionCollection cdc = new CodeParameterDeclarationExpressionCollection();
            XmlNodeList parameters = node.SelectNodes("parameters/parameter");
            foreach (XmlNode parameter in parameters)
            {
                cdc.Add(new CodeParameterDeclarationExpression(parameter.Attributes["type"].Value,
                    parameter.Attributes["name"].Value));
            }
            CodeMemberMethodFormatter formatter = new CodeMemberMethodFormatter(name, comment, cdc)
            {
                Tag = (MethodGroup) Enum.Parse(typeof (MethodGroup), tag)
            };
            methods.Add(formatter);
        }
    }

    [MenuItem("Window/CodeBeach/SaveTemplate")]
    private static void SaveTemplate()
    {
        XmlDocument doc = new XmlDocument();
        XmlElement root = doc.CreateElement("template");
        doc.AppendChild(root);
        foreach (CodeMemberMethodFormatter method in methods)
        {
            XmlNode node = root.AppendChild(doc.CreateElement("method"));
            node.Attributes.Append(doc.CreateAttribute("name")).Value = method.Name;
            node.Attributes.Append(doc.CreateAttribute("comment")).Value = method.Comment;
            node.Attributes.Append(doc.CreateAttribute("tag")).Value = method.Tag.ToString();
            XmlNode para = node.AppendChild(doc.CreateElement("parameters"));
            foreach (CodeParameterDeclarationExpression parameter in method.Parameters)
            {
                XmlNode pnode = para.AppendChild(doc.CreateElement("parameter"));
                pnode.Attributes.Append(doc.CreateAttribute("name")).Value = parameter.Name;
                pnode.Attributes.Append(doc.CreateAttribute("type")).Value = parameter.Type.ToString();
            }
        }

        doc.Save("Assets/CodeBeach/Template.xml");
    }
}