using System.Collections;
/// <summary>
/// 行为树节点类型
/// </summary>
public enum NodeType {
    /// <summary>
    /// 
    /// </summary>
    Unknow,
    /// <summary>
    /// 选择节点
    /// </summary>
    Select,
    /// <summary>
    /// 顺序节点
    /// </summary>
    Sequence,
    /// <summary>
    /// 修饰节点
    /// </summary>
    Decorator,
    /// <summary>
    /// 随机节点
    /// </summary>
    Random,
    /// <summary>
    /// 并行节点
    /// </summary>
    Parallel,
    /// <summary>
    /// 条件节点
    /// </summary>
    Condition,
    /// <summary>
    /// 行为节点
    /// </summary>
    Action
}