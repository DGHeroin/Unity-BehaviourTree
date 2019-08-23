using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
流程说明
->RootNode(SelectNode)
    -> SequenceNode
        -> ConditionNode 是否饿了
        -> SelectNode
            -> ConditionNode 是否有饭
                -> SequenceNode 做饭流程
                    -> ActionNode 走去厨房
                    -> ActionNode 做饭
        -> ActionNode 走去餐桌
        -> ActionNode 吃饭
    -> ParallelNode 并行节点
        -> ConditionNode 是否有钱
        -> SequenceNode
            -> MoveNode 走去酒馆
            -> RandomNode
                -> ActionNode 喝水
                -> ActionNode 喝酒
*/

public class HeroActor : MonoBehaviour {

    private NodeSelect rootNode = new NodeSelect();// 根节点
    private int energy = 0;       // 能量值
    private int minEnergy = 50;   // <= min, 感到饥饿
    private int maxEnergy = 100;  // >= max, 感到吃饱

    private int food = 0;        // 食物量
    private int minFood = 0;     // <= min, 没有食物了
    private int maxFood = 100;   // >= max, 饭做好了

    [SerializeField, Header("厨房")]
    private Transform doCookingTransform = null;
    [SerializeField, Header("餐桌")]
    private Transform eatFoodTransform = null;
    [SerializeField, Header("酒吧")]
    private Transform saloonTransform = null;


    // Start is called before the first frame update
    void Start()
    {
        Init();
        StartCoroutine(Sim());
    }

    IEnumerator Sim() {
        while (true) {
            ChangeEnergy(-2);
            rootNode.Execute();
            yield return new WaitForSeconds(0.5f);
            // yield return null;
        }
    }

    private void Update() {
        
    }

    void Init() {
        NodeSequence  nodeSequence_1 = new NodeSequence();
        rootNode.AddNode(nodeSequence_1);

        #region 条件 1.1 是否饿了
        {
            NodeConditionHungry cond_1_1 = new NodeConditionHungry();
            cond_1_1.SetActor(this);
            nodeSequence_1.AddNode(cond_1_1);
        }
        #endregion

        #region 选择条件 1.2 是否有饭
        {
            NodeSelect sel_1_2 = new NodeSelect();
            nodeSequence_1.AddNode(sel_1_2);

            // 1.2.1 是否有饭
            NodeConditionHasFood hasFood_1_2_1 = new NodeConditionHasFood();
            hasFood_1_2_1.SetActor(this);
            sel_1_2.AddNode(hasFood_1_2_1);

            // 没有饭的话, 走去厨房 => 做饭
            // 1.2.2 顺序节点
            NodeSequence sequence_1_2_2 = new NodeSequence();
            sel_1_2.AddNode(sequence_1_2_2);
            // 1.2.2.1 走厨房
            NodeActionMove move_1_2_2_1 = new NodeActionMove();
            move_1_2_2_1.SetActor(this);
            move_1_2_2_1.myTransform = this.transform;
            move_1_2_2_1.targetTransform = doCookingTransform;
            sequence_1_2_2.AddNode(move_1_2_2_1);
            // 1.2.2.2 做饭
            NodeActionCooking cooking_1_2_2_2 = new NodeActionCooking();
            cooking_1_2_2_2.SetActor(this);
            sequence_1_2_2.AddNode(cooking_1_2_2_2);
        }
        #endregion
        #region 1.3 去餐桌
        {
            NodeActionMove move_1_3 = new NodeActionMove();
            move_1_3.SetActor(this);
            move_1_3.myTransform = this.transform;
            move_1_3.targetTransform = eatFoodTransform;

            nodeSequence_1.AddNode(move_1_3);
        }
        #endregion

        #region 1.4 吃饭
        {
            NodeActionEat eat_1_4 = new NodeActionEat();
            eat_1_4.SetActor(this);
            nodeSequence_1.AddNode(eat_1_4);
        }
        #endregion

        #region 2
        // 并行节点
        NodeParallel p_2 = new NodeParallel();
        rootNode.AddNode(p_2);

        // 2.1 是否有钱
        NodeConditionHasMoney hasMoney_2_1 = new NodeConditionHasMoney();
        p_2.AddNode(hasMoney_2_1);

        // 2.2 走去酒馆
        NodeSequence seq_saloon = new NodeSequence();
        p_2.AddNode(seq_saloon);

        NodeActionMove move_2_2 = new NodeActionMove();
        move_2_2.myTransform = this.transform;
        move_2_2.targetTransform = saloonTransform;
        seq_saloon.AddNode(move_2_2);
        // 2.3 随机点一份酒水
        NodeRandom r_2_3 = new NodeRandom();
        seq_saloon.AddNode(r_2_3);
        // 2.3.1 喝红酒
        r_2_3.AddNode(new NodeActionDrinkRedWine());
        // 2.3.2 喝白开水
        r_2_3.AddNode(new NodeActionDrinkWater());
        #endregion
    }

    #region 饿了吃饭
    public bool IsHungry() {
        return energy <= minEnergy;
    }
    public void AddEnergy(int val) {
        ChangeEnergy(val);
    }
    private void ChangeEnergy(int val) {
        energy += val;
        Debug.Log("能量: " + val + " => " + this);
    }
    public bool IsFull() {
        return energy >= maxEnergy;
    }
    #endregion

    #region 食物
    public bool FoodEnough() {
        return food >= maxFood;
    }
    public bool HasFood() {
        return food > minFood;
    }
    public void Cooking(int val) {
        ChangeFood(val);
    }
    public void ChangeFood(int val) {
        food += val;
        Debug.Log("食物:" + val + " => " + this);
    }
    #endregion


    public override string ToString() {
        return string.Format(" 能量: {0} 食物: {1}", energy, food);
    }

}
