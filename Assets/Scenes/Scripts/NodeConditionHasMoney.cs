public class NodeConditionHasMoney : NodeCondition {
    public override ResultType Execute() {
        UnityEngine.Debug.LogError("一直有钱 一直爽");
        return ResultType.Success; // 一直有钱
    }
}