public class NodeActionDrinkWater : NodeAction {
    public override ResultType Execute() {
        UnityEngine.Debug.LogWarning("今天功力不行, 只能喝水了");
        return ResultType.Success;
    }
}
