/// <summary>
/// 并行组合节点
/// </summary>
public class NodeParallel : NodeCombiner {
    public NodeParallel() : base(NodeType.Parallel) { }
    /// <summary>
    /// 并行组合节点同时执行所有节点, 直到一个节点返回 Fail 或者全部节点 Success 才返回给父节点Success
    /// 其他情况, 则向父节点返回Running
    /// </summary>
    /// <returns></returns>
    public override ResultType Execute() {
        ResultType resultType = ResultType.Fail;

        int successCount = 0;
        for (int i = 0; i < nodeChildList.Count; i++) {
            NodeRoot nodeRoot = nodeChildList[i];
            resultType = nodeRoot.Execute();
            if (resultType == ResultType.Fail) {
                break;
            }
            if (resultType == ResultType.Running) {
                break;
            }

            if (resultType == ResultType.Success) {
                successCount++;
                continue;
            }
        }

        if (resultType == ResultType.Fail) {
            return ResultType.Fail;
        }
        if (successCount == nodeChildList.Count) {
            return ResultType.Success;
        }
        return ResultType.Running;
    }
}