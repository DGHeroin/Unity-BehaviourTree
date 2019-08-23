using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
[CreateAssetMenu(fileName = "BTree", menuName = "AI/CreateBTree", order = 1)]
#endif
public class NodeAsset : ScriptableObject {
    public string desc;
    public int openCount;
    [SerializeField]
    public NodeValue Value;
}

[Serializable]
public class NodeValue {
    public NodeValue() { }
    /// <summary>
    /// 根节点
    /// </summary>
    public bool IsRootNode = false;
    // 节点类型
    public NodeType NodeType = NodeType.Select;
    // public List<NodeValue> Children = new List<NodeValue>();

    public string componentName = string.Empty;

    public string description = string.Empty;

    public Rect rect = new Rect(0, 0, 100, 100);

    public bool IsRelease = false;
    public void Release() {
        IsRelease = true;
    }

}