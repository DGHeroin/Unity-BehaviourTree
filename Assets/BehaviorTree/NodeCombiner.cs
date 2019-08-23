using System.Collections.Generic;

public abstract class NodeCombiner : NodeRoot {
    /// <summary>
    /// 子节点容器
    /// </summary>
    protected List<NodeRoot> nodeChildList = new List<NodeRoot>();

    public NodeCombiner(NodeType nodeType) : base(nodeType) { }

    /// <summary>
    /// 添加节点
    /// </summary>
    /// <param name="node"></param>
    public void AddNode(NodeRoot node) {
        int count = nodeChildList.Count;
        node.NodeIndex = count;
        nodeChildList.Add(node);
    }
}