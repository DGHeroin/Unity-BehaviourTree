using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class NodeValue {
    public string Title = string.Empty;
    public string Desc = string.Empty;
    public NodeType NodeType;
    public List<NodeValue> Children = new List<NodeValue>();

    public bool isRelease = false;
    private Rect _rect = new Rect(0, 0, 100, 100);
    public Rect rect {
        get {
            if (_rect.width < 50) {
                _rect.width = 100;
            }
            if (_rect.height < 50) {
                _rect.height = 100;
            }
            return _rect;
        }
        set {
            _rect = value;
        }
    }
    public void Release() {
        isRelease = true;
    }
    [HideInInspector]
    public static string[] nodeTypeString = new string[] {
        "Unknow", "Select", "Sequence", "Decorator", "Random", "Parallel", "Condition", "Action"
    };
    public int iNodeType {
        get {
            return (int)NodeType;
        }
        set {
            this.NodeType = (NodeType)value;
        }
    }
    
    public string GetNodeTypeString() {
        return nodeTypeString[iNodeType];
    }
    
    static Color[] Colors = new Color[] {
        Color.white,
        new Color(0.5f, 0.5f, 0.7f, 1),
        Color.green,
        Color.red,

        Color.gray,
        Color.yellow,
        Color.magenta,
        Color.cyan
    };
    public Color GetColor() {
        return Colors[iNodeType];
    }
}