using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TreeNodeEditor : EditorWindow {
    private List<NodeRoot> nodeRootList = new List<NodeRoot>();
    private NodeAsset _currentAsset = null;
    private NodeAsset currentAsset {
        get {
            if (_currentAsset == null) {
                _currentAsset = new NodeAsset();
            }
            return _currentAsset;
        }
        set {
            _currentAsset = value;
        }
    }
    private NodeRoot selectNode = null;

    private bool makeTransitionMode = false;

    [MenuItem("Window/CreateTree")]
    static void ShowWindow() {
        TreeNodeEditor window = EditorWindow.GetWindow<TreeNodeEditor>();
       
    }

    public static void ShowWindow(NodeAsset asset) {
        TreeNodeEditor window = EditorWindow.GetWindow<TreeNodeEditor>();
        window.currentAsset = asset;
        window.parseAsset();
    }

    void parseAsset() {
        Debug.Log("解析" + currentAsset.desc);
        currentAsset.openCount++;
    }

    private Vector2 mousePosition;
    private bool makeTransitionNode = false;
    private void OnGUI() {
        Event evt = Event.current;
        mousePosition = evt.mousePosition;
        // 删除无用节点
        for (int i = nodeRootList.Count - 1; i >= 0; i--) {
            if (nodeRootList[i].isRelease) {
                nodeRootList.RemoveAt(i);
            }
        }
        // 鼠标右键
        if (evt.button == 1) {
            if (evt.type == EventType.MouseDown) {
                if (!makeTransitionNode) {
                    bool clickedOnNode = false;
                    int selectIndex = 0;
                    selectNode = GetMouseInNode(out selectIndex);
                    clickedOnNode = selectNode != null;
                    if (!clickedOnNode) {
                        showMenu(0);
                    } else {
                        showMenu(1);
                    }
                }
            }
        }

        // 选择节点为空的时候, 无法连线
        if (selectNode == null) {
            makeTransitionMode = false;
        }
        if (!makeTransitionMode) {
            if (evt.type == EventType.MouseUp) {
                selectNode = null;
            }
        }

        // 在连线状态 按下鼠标
        if (makeTransitionMode && evt.type == EventType.MouseDown) {
            int selectIndex = 0;
            var n = GetMouseInNode(out selectIndex);
            // 如果按下鼠标, 选中了一个节点
            // 则 新选中的节点, 添加为selectNode的子节点
            if (selectNode != n) {
                selectNode.Children.Add(n);
            }
            // 取消连线状态
            makeTransitionMode = false;
            // 清空选中
            selectNode = null;
        }
        // 连线状态下, 选中节点不为空
        if(makeTransitionMode && selectNode != null) {
            // 获取鼠标位置
            Rect mouseRect = new Rect(mousePosition.x, mousePosition.y, 10, 10);
            // 显示连线, 从选中节点到鼠标的位置
            DrawNodeCurve(selectNode.rect, mouseRect);
        }
        // 画节点
        #region 画节点
        {
            BeginWindows();
            for (int i = 0; i < nodeRootList.Count; i++) {
                var node = nodeRootList[i];
                node.rect = GUI.Window(i, node.rect, DrawNodeWindow, "节点");
                DrawToChildCurve(node);
            }
            EndWindows();
        }
        #endregion

        // 重绘
        Repaint();
    }

    void DrawNodeWindow(int i) {
        var node = nodeRootList[i];
        GUI.DragWindow();
    }

    NodeRoot GetMouseInNode(out int index) {
        index = 0;
        for (int i = 0; i < nodeRootList.Count; i++) {
            var n = nodeRootList[i];
            // 如果鼠标位置在 节点的Rect 范围内, 则认为选中节点
            if (n.rect.Contains(mousePosition)) {
                index = i;
                return n;
            }
        }
        return null;
    }

    void showMenu(int type) {
        GenericMenu menu = new GenericMenu();
        switch (type) {
            case 0:
                // 添加一个新节点
                menu.AddItem(new GUIContent("Add Node"), false, AddNode);
                break;
            case 1:
                // 连线
                menu.AddItem(new GUIContent("Make Transition"), false, MakeTransition);
                // 删除节点
                menu.AddItem(new GUIContent("Delete Node"), false, DeleteNode);
                break;
        }
        menu.ShowAsContext();
        Event.current.Use();
    }

    void AddNode() {
        var node = new NodeRoot();
        node.rect = new Rect(mousePosition.x, mousePosition.y, 100, 100);
        nodeRootList.Add(node);
    }
    void MakeTransition() {
        makeTransitionMode = true;
    }
    void DeleteNode() {
        int idx = 0;
        selectNode = GetMouseInNode(out idx);
        if (selectNode != null) {
            nodeRootList[idx].Release();
            nodeRootList.Remove(selectNode);
        }
    }

    public static void DrawNodeCurve(Rect start, Rect end) {
        Vector3 sp = new Vector3(start.x + start.width / 2, start.y + start.height / 2, 0);
        Vector3 ep = new Vector3(end.x + end.width / 2, end.y + end.height / 2, 0);

        Handles.DrawLine(sp, ep);
    }

    void DrawToChildCurve(NodeRoot node) {
        for (int i = node.Children.Count - 1; i >= 0; i--) {
            var child = node.Children[i];
            if (child == null || child.isRelease) {
                node.Children.RemoveAt(i);
                continue;
            }
            DrawNodeCurve(node.rect, child.rect);
        }
    }

    private NodeRoot GetNodeRoot(NodeValue value) {
        switch (value.NodeType) {
            case NodeType.Select: // 选择节点
                return new NodeSelect();
            case NodeType.Sequence: // 顺序节点
                return new NodeSequence();
            case NodeType.Decorator: // 修饰节点
                return new NodeDecorator();
            case NodeType.Random: // 随机节点
                return new NodeRandom();
            case NodeType.Parallel: // 并行节点
                return new NodeParallel();
            case NodeType.Condition: // 条件节点
                break;
            case NodeType.Action: // 行为节点
                break;
        }

        return null;
    }
}
