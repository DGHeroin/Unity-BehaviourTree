/// <summary>
/// 条件叶节点
/// </summary>
public abstract class NodeCondition : NodeLeaf {
    public NodeCondition() : base(NodeType.Condition) { }
}