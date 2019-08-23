using UnityEngine;
public class NodeActionMove : NodeAction {
    private HeroActor actor;
    public void SetActor(HeroActor actor) {
        this.actor = actor;
    }

    /// <summary>
    /// 移动的目标
    /// </summary>
    public Transform myTransform = null;
    public Transform targetTransform = null;
    public override ResultType Execute() {
        if (targetTransform == null) { // 没有目标
            return ResultType.Fail;
        }
        bool isArrive = Move(targetTransform.position);
        return isArrive ? ResultType.Success : ResultType.Running;
    }

    private bool Move(Vector3 p) {
        Vector3 moveDir = p - myTransform.position;
        moveDir = moveDir.normalized;

        myTransform.Translate(moveDir * Time.deltaTime * 50);

        float distance = Vector3.Distance(myTransform.position, p);
        return distance < 1;
    }
}