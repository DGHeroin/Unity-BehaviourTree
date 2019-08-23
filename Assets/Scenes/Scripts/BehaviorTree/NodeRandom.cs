using System.Collections.Generic;

/// <summary>
/// 随机组合节点
/// </summary>
public class NodeRandom : NodeCombiner {
    private NodeRoot lastRunningNode;
    public NodeRandom() : base(NodeType.Random) { }

    /// <summary>
    /// 随机遍历子节点, 当遇到 Success 时, 返回父节点
    /// 当执行遇到 Fail 时, 尝试执行下一个子节点
    /// 当执行遇到 Running 时, 记录该节点, 下一次继续运行
    /// </summary>
    /// <returns></returns>
    public override ResultType Execute() {
        List<int> randomList = GetRandom(nodeChildList.Count);

        int index = -1;
        if (lastRunningNode != null) {
            index = lastRunningNode.NodeIndex;
            lastRunningNode = null;
        }
        ResultType resultType = ResultType.Fail;
        while ((randomList.Count > 0)) {
            if (index < 0) {
                // 从随机数组中最后一个开始取
                index = randomList[randomList.Count - 1];
                randomList.RemoveAt(randomList.Count - 1);
            }
            NodeRoot nodeRoot = nodeChildList[index];
            index = -1;
            resultType = nodeRoot.Execute();
            if (resultType == ResultType.Fail) {
                continue;
            }
            if (resultType == ResultType.Success) {
                break;
            }
            if (resultType == ResultType.Running) {
                lastRunningNode = nodeRoot;
                break;
            }

        }

        return resultType;
    }

    /// <summary>
    /// 生成一组0到n随机数
    /// </summary>
    /// <param name="n">随机数个数</param>
    /// <returns>随机数数组</returns>
    private List<int> GetRandom(int n) {
        List<int> resultList = new List<int>();
        List<int> tempList = new List<int>();
        
        for (int i = 0; i < n; i++) {
            tempList.Add(i);
        }
        System.Random random = new System.Random();

        while(tempList.Count > 0) {
            int index = random.Next(0, (tempList.Count - 1));
            resultList.Add(tempList[index]);
            tempList.RemoveAt(index);
        }

        return resultList;
    }
}