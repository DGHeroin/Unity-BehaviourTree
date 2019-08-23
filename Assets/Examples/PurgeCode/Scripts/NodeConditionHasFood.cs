
public class NodeConditionHasFood : NodeCondition {
    private HeroActor actor;
    public void SetActor(HeroActor actor) {
        this.actor = actor;
    }

    public override ResultType Execute() {
        return actor.HasFood() ? ResultType.Success : ResultType.Fail;
    }
}