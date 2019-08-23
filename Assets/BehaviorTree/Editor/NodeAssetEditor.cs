using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class NodeAssetHandler {
    [OnOpenAssetAttribute(1)]
    public static bool step1(int instanceID, int line) {
        var asset = EditorUtility.InstanceIDToObject(instanceID) as NodeAsset;
        if (asset != null) {
            TreeNodeWindowEditor.ShowWindowWithAsset(asset);
            return true; // we handled
        }

        return false; // we did not handle the open
    }
}