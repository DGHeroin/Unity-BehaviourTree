using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NodeAsset))]
public class NodeAssetEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        NodeAsset asset = (NodeAsset)target;
        if (GUILayout.Button("打开编辑器")) {
            Debug.Log("打开编辑器");
            TreeNodeEditor.ShowWindow(asset);
        }
    }
}