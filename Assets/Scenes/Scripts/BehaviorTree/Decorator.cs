/// <summary>
/// 修饰组合节点
/// </summary>
public class Decorator : NodeCombiner {
    private ResultType unitlResultType = ResultType.Fail;

    public Decorator() : base(NodeType.Decorator) { }
    /// <summary>
    /// 修饰节点只有一个子节点, 执行子节点直到等于期待的结果, 再返回给父节点
    /// 否则一直返回Running
    /// </summary>
    /// <returns></returns>
    public override ResultType Execute() {
        NodeRoot nodeRoot = nodeChildList[0];
        ResultType resultType = nodeRoot.Execute();
        if (resultType != unitlResultType) {
            return ResultType.Running;
        }

        return resultType;
    }

    public void SetResultType(ResultType resultType) {
        unitlResultType = resultType;
    }
}