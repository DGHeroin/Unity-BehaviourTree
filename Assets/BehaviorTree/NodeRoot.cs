using UnityEngine;
using System.Collections.Generic;
public class NodeRoot {
    protected NodeType nodeType;
    public int NodeIndex;

    public List<NodeRoot> Children { get; set; } = new List<NodeRoot>();
    public NodeRoot() {
        this.nodeType = NodeType.Unknow;
    }
    public NodeRoot(NodeType type) {
        this.nodeType = type;
    }
    /// <summary>
    /// 执行节点
    /// </summary>
    /// <returns>返回执行结果</returns>
    public virtual ResultType Execute() { return ResultType.Fail; }

    public Rect rect = new Rect(0, 0, 100, 100);
    public bool isRelease = false;
    public void Release() {
        isRelease = true;
    }

}