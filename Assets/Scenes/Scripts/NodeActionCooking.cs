
public class NodeActionCooking : NodeAction {
    private HeroActor actor;
    public override ResultType Execute() {
        if (actor.FoodEnough()) {
            return ResultType.Success;
        }
        actor.Cooking(30);
        return ResultType.Running;
    }

    public void SetActor(HeroActor actor) {
        this.actor = actor;
    }
}