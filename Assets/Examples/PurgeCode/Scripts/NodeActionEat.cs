
/// <summary>
/// 吃饭动作
/// </summary>
public class NodeActionEat : NodeAction {
    private HeroActor actor;
    public void SetActor(HeroActor actor) {
        this.actor = actor;
    }
    public override ResultType Execute() {
        if (actor.IsFull()) {
            return ResultType.Success;
        }
        // 1 份 食物 换 2 份能量
        actor.ChangeFood(-10);
        actor.AddEnergy(20);
        
        return ResultType.Running;
    }
}