using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
[CreateAssetMenu(fileName ="BehaviourTree", menuName ="AI/Create Behaviour Tree")]
#endif
public class NodeAsset : ScriptableObject {
    public NodeValue Value = new NodeValue();
}