using UnityEditor;
using UnityEngine;

public class NodePadWindow : EditorWindow
{
    [MenuItem("Window/NodePad")]
    private static void Launch()
    {
        GetWindow<NodePadWindow>(false, "NodePad").minSize = new Vector2(500, 500);
    }

    private Node _selection;
    private NodePadData data;

    private Node Selection
    {
        get { return _selection; }
        set
        {
            _selection = value;
            Repaint();
        }
    }

    private void OnEnable()
    {
        data = AssetDatabase.LoadAssetAtPath("Assets/NodePad/NodePadData.asset", typeof (NodePadData)) as NodePadData;
        if (!data)
        {
            data = CreateInstance<NodePadData>();
            AssetDatabase.CreateAsset(data, "Assets/NodePad/NodePadData.asset");
        }
    }

    private void OnGUI()
    {
        switch (Event.current.type)
        {
            case EventType.ContextClick:
            {
                Vector2 pos = Event.current.mousePosition;
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Add Node"), false, () => CreateNode(pos));
                foreach (Node node in data.nodes)
                {
                    if (node.Rect.Contains(pos))
                    {
                        Node del = node;
                        menu.AddItem(new GUIContent("Delete Node"), false, () => DeleteNode(del));
                    }
                }
                menu.ShowAsContext();
            }
                break;
            case EventType.MouseDown:
            {
                if (Event.current.button == 0)
                    foreach (Node node in data.nodes)
                    {
                        if (node.Rect.Contains(Event.current.mousePosition))
                        {
                            Selection = node;
                        }
                    }
            }
                break;
        }

        foreach (Node node in data.nodes)
        {
            node.IsSelected = Selection == node;
            node.OnGUI();
        }

        if (GUI.changed)
        {
            Repaint();
            EditorUtility.SetDirty(data);
        }

        if (data.nodes.Count > 1)
            for (int i = 0; i < data.nodes.Count; i++)
            {
                int j = i + 1;
                if (j >= data.nodes.Count) j = 0;
                Handles.DrawBezier(data.nodes[i].Position, data.nodes[j].Position,
                    data.nodes[i].Position + Vector2.right*50f,
                    data.nodes[j].Position + Vector2.right*50f, Color.red, null, 2f);
            }
    }

    private void DeleteNode(Node node)
    {
        data.nodes.Remove(node);
        EditorUtility.SetDirty(data);
    }

    private void CreateNode(Vector2 pos)
    {
        data.nodes.Add(new Node(pos, new Vector2(50, 50), "New Node"));
        EditorUtility.SetDirty(data);
    }
}