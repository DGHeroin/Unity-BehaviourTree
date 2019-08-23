public abstract class NodeRoot {
    protected NodeType nodeType;
    public int NodeIndex;
    public NodeRoot(NodeType type) {
        this.nodeType = type;
    }
    /// <summary>
    /// 执行节点
    /// </summary>
    /// <returns>返回执行结果</returns>
    public abstract ResultType Execute();

}