public class NodeConditionHasMoney : NodeCondition {
    public override ResultType Execute() {
        UnityEngine.Debug.Log("一直有钱 一直爽");
        return ResultType.Success; // 一直有钱
    }
}