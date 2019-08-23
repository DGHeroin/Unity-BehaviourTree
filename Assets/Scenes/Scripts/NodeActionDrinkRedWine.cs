public class NodeActionDrinkRedWine : NodeAction {
    public override ResultType Execute() {
        UnityEngine.Debug.LogWarning("喝了红酒");
        return ResultType.Success;
    }
}