# Unity 行为树
行为树实例

### 1. 用代码写的行为树
```
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
```

### 2. 简单图形编辑器(未完成)
<img src="https://github.com/DGHeroin/Unity-BehaviourTree/blob/master/Images/Unity_yxjpvopjJr.png" width="700" height="600" alt="图片描述文字"/>
