
public class NodeSelect : NodeCombiner {
    private NodeRoot lastRunningNode;
    public NodeSelect() : base(NodeType.Select) { }

    /// <summary>
    /// 依次遍历所有子节点, 如果都返回Fail, 则向父节点返回Fail
    /// 遍历直到返回 Sucess 或者 Running
    /// 如果返回Running, 则需要记住节点, 下一次从这个节点开始执行
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
                continue;
            }

            if (resultType == ResultType.Success) {
                break;
            }

            if (resultType == ResultType.Running) {
                lastRunningNode = nodeRoot;
                break;
            }
        }

        return resultType;
    }
}