/// <summary>
/// 顺序组合节点
/// </summary>
public class NodeSequence : NodeCombiner {
    private NodeRoot lastRunningNode;
    public NodeSequence() : base(NodeType.Sequence) { }

    /// <summary>
    /// 顺序节点只要子节点返回 Success 则继续执行后续节点
    /// 如果有节点返回Fail 则停止执行后续节点
    /// 如果有节点返回 Running, 则记录该节点并退出执行, 后续从此节点开始执行
    /// </summary>
    /// <returns></returns>
    public override ResultType Execute() {
        int index = 0;
        if (lastRunningNode != null) {
            index = lastRunningNode.NodeIndex;
        }
        lastRunningNode = null;

        ResultType resultType = ResultType.Fail;
        for(int i = index; i < nodeChildList.Count; i++) {
            NodeRoot nodeRoot = nodeChildList[i];
            resultType = nodeRoot.Execute();
            if (resultType == ResultType.Fail) {
                break;
            }

            if (resultType == ResultType.Success) {
                continue;
            }
            if (resultType == ResultType.Running) {
                lastRunningNode = nodeRoot;
                break;
            }
        }

        return resultType;
    }
}