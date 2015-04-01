using System;
using UnityEngine;

[Serializable]
public class Node
{
    [SerializeField] private Vector2 _size;
    [SerializeField] private Vector2 _position;

    public Vector2 Size
    {
        get { return _size; }
        set
        {
            _size = value;
            Rect = new Rect(Position.x - Size.x/2, Position.y - Size.y/2, Size.x, Size.y);
        }
    }

    public Vector2 Position
    {
        get { return _position; }
        set
        {
            _position = value;
            Rect = new Rect(Position.x - Size.x/2, Position.y - Size.y/2, Size.x, Size.y);
        }
    }

    [SerializeField] public string name;

    public Rect Rect { get; protected set; }

    public bool IsSelected { get; set; }

    private void Draw()
    {
        GUI.skin.box.Draw(Rect, new GUIContent(name), false, false, false, false);
    }

    public void OnGUI()
    {
        switch (Event.current.type)
        {
            case EventType.MouseDrag:
            {
                if (Event.current.button == 0)
                    if (Rect.Contains(Event.current.mousePosition))
                    {
                        Position += Event.current.delta;
                        Event.current.Use();
                    }
            }
                break;
            case EventType.Repaint:
            {
                if (IsSelected)
                    GUI.color = Color.blue;
                Draw();
                GUI.color = Color.white;
            }
                break;
        }
    }

    public Node(Vector2 position, Vector2 size, string name)
    {
        Position = position;
        Size = size;
        this.name = name;
    }
}