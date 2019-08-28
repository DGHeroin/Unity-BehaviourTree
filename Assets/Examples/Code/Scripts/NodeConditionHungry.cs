
public class NodeConditionHungry : NodeCondition {

    private HeroActor actor;
    public void SetActor(HeroActor actor) {
        this.actor = actor;
    }

    public override ResultType Execute() {
        return actor.IsHungry() ? ResultType.Success : ResultType.Fail;

    }

    
}