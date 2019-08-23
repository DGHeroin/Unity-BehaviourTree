using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 
/// </summary>
public class TreeNodeWindowEditor : EditorWindow {
    [MenuItem("Window/Behaviour Tree")]
    static void ShowWindow() {
        EditorWindow.GetWindow(typeof(TreeNodeWindowEditor));
        Debug.Log("It should have appeared!");
    }

    public static void ShowWindowWithAsset(NodeAsset asset = null) {
        var win = EditorWindow.GetWindow(typeof(TreeNodeWindowEditor)) as TreeNodeWindowEditor;
        if (asset != null) {
            win.rootAsset = asset.Value;
            win.parse();
        }
    }

    private List<NodeValue> nodeRootList = new List<NodeValue>();
    private NodeValue selectNode = null;

    private bool makeTransitionMode = false;
    private Vector2 mousePosition;
    private bool makeTransitionNode = false;

    private NodeValue selectAsset = null;
    private NodeValue rootAsset = null;

    void guiBuildNotify() {
        GUILayout.BeginArea(new Rect(100, 0, 200, 200));
        if (selectAsset == null) {
            var asset = Selection.activeObject as NodeAsset;
            if (asset == null) {
                GUILayout.Label("NodeAsset not select");
            } else {
                selectAsset = asset.Value;
            }
        }
        GUILayout.EndArea();
    }
    
    /// <summary>
    /// 缩放window
    /// </summary>
    void zoom() {
         
    }

    public void OnGUI() {
        zoom();
        guiBuildNotify();
        if (selectAsset != null) { // we can edit
            privGUI();
        }
    }

    private void OnDestroy() {
        Debug.LogWarning("关闭了");
    }

    void privGUI() {
        Event evt = Event.current;
        mousePosition = evt.mousePosition;
        // 删除无用节点
        for (int i = selectAsset.Children.Count - 1; i >= 0; i--) {
            if (selectAsset.Children[i].isRelease) {
                selectAsset.Children.RemoveAt(i);
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
        if (makeTransitionMode && selectNode != null) {
            // 获取鼠标位置
            Rect mouseRect = new Rect(mousePosition.x, mousePosition.y, 10, 10);
            // 显示连线, 从选中节点到鼠标的位置
            DrawNodeCurve(selectNode.rect, mouseRect);
        }
 
        #region 画所有节点
        {
            BeginWindows();
            for (int i = 0; i < nodeRootList.Count; i++) {
                var node = nodeRootList[i];
                GUI.backgroundColor = node.GetColor();
                node.rect = GUI.Window(i, node.rect, DrawNodeWindow, node.Title);
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
        var w = node.rect.width - 10;
        node.Title = GUI.TextField(new Rect(5, 20, w, 20), node.Title);
        GUI.Label(new Rect(5, 40, w, 20), node.GetNodeTypeString());
        node.iNodeType = GUI.Toolbar(new Rect(5, 60, w, 20),  node.iNodeType, NodeValue.nodeTypeString);
        GUI.DragWindow();
    }

    NodeValue GetMouseInNode(out int index) {
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
        var node = new NodeValue();
        node.rect = new Rect(mousePosition.x, mousePosition.y, 100, 100);
        nodeRootList.Add(node);
    }
    void MakeTransition() {
        makeTransitionMode = true;
    }
    void DeleteNode() {
        int idx;
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

    void DrawToChildCurve(NodeValue node) {
        for (int i = node.Children.Count - 1; i >= 0; i--) {
            var child = node.Children[i];
            if (child == null || child.isRelease) {
                node.Children.RemoveAt(i);
                continue;
            }
            DrawNodeCurve(node.rect, child.rect);
        }
    }

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="value"></param>
    ///// <returns></returns>
    //private NodeRoot GetNodeRoot(NodeValue value) {
    //    switch (value.NodeType) {
    //        case NodeType.Select: // 选择节点
    //            return new NodeSelect();
    //        case NodeType.Sequence: // 顺序节点
    //            return new NodeSequence();
    //        case NodeType.Decorator: // 修饰节点
    //            return new NodeDecorator();
    //        case NodeType.Random: // 随机节点
    //            return new NodeRandom();
    //        case NodeType.Parallel: // 并行节点
    //            return new NodeParallel();
    //        case NodeType.Condition: // 条件节点
    //            break;
    //        case NodeType.Action: // 行为节点
    //            break;
    //    }

    //    return null;
    //}

    void parse() {
        this.nodeRootList.Clear();
        if (rootAsset == null) { return; }

        // 构建出所有检点
        // 根节点
        nodeRootList.Add(rootAsset);
        // 子节点
        createChild(rootAsset);
    }

    void createChild(NodeValue parent) {
        foreach (var node in parent.Children) {
            if (node == null) { continue; }
            // 位置变换
            nodeRootList.Add(node);
            createChild(node);
        }
    }
}


public class ZoomTestWindow : EditorWindow {
    [MenuItem("Window/Zoom Test")]
    private static void Init() {
        ZoomTestWindow window = EditorWindow.GetWindow(typeof(ZoomTestWindow)) as ZoomTestWindow;
        window.minSize = new Vector2(600.0f, 300.0f);
        window.wantsMouseMove = true;
        window.Show();
    }

    private const float kZoomMin = 0.1f;
    private const float kZoomMax = 10.0f;

    private readonly Rect _zoomArea = new Rect(0.0f, 75.0f, 600.0f, 300.0f - 100.0f);
    private float _zoom = 1.0f;
    private Vector2 _zoomCoordsOrigin = Vector2.zero;

    private Vector2 ConvertScreenCoordsToZoomCoords(Vector2 screenCoords) {
        return (screenCoords - _zoomArea.TopLeft()) / _zoom + _zoomCoordsOrigin;
    }

    private void DrawZoomArea() {
        // Within the zoom area all coordinates are relative to the top left corner of the zoom area
        // with the width and height being scaled versions of the original/unzoomed area's width and height.
        EditorZoomArea.Begin(_zoom, _zoomArea);

        GUI.Box(new Rect(0.0f - _zoomCoordsOrigin.x, 0.0f - _zoomCoordsOrigin.y, 100.0f, 25.0f), "Zoomed Box");

        // You can also use GUILayout inside the zoomed area.
        GUILayout.BeginArea(new Rect(300.0f - _zoomCoordsOrigin.x, 70.0f - _zoomCoordsOrigin.y, 130.0f, 50.0f));
        GUILayout.Button("Zoomed Button 1");
        GUILayout.Button("Zoomed Button 2");
        GUILayout.EndArea();

        EditorZoomArea.End();
    }

    private void DrawNonZoomArea() {
        GUI.Box(new Rect(0.0f, 0.0f, 600.0f, 50.0f), "Adjust zoom of middle box with slider or mouse wheel.\nMove zoom area dragging with middle mouse button or Alt+left mouse button.");
        _zoom = EditorGUI.Slider(new Rect(0.0f, 50.0f, 600.0f, 25.0f), _zoom, kZoomMin, kZoomMax);
        GUI.Box(new Rect(0.0f, 300.0f - 25.0f, 600.0f, 25.0f), "Unzoomed Box");
    }

    private void HandleEvents() {
        // Allow adjusting the zoom with the mouse wheel as well. In this case, use the mouse coordinates
        // as the zoom center instead of the top left corner of the zoom area. This is achieved by
        // maintaining an origin that is used as offset when drawing any GUI elements in the zoom area.
        if (Event.current.type == EventType.ScrollWheel) {
            Vector2 screenCoordsMousePos = Event.current.mousePosition;
            Vector2 delta = Event.current.delta;
            Vector2 zoomCoordsMousePos = ConvertScreenCoordsToZoomCoords(screenCoordsMousePos);
            float zoomDelta = -delta.y / 150.0f;
            float oldZoom = _zoom;
            _zoom += zoomDelta;
            _zoom = Mathf.Clamp(_zoom, kZoomMin, kZoomMax);
            _zoomCoordsOrigin += (zoomCoordsMousePos - _zoomCoordsOrigin) - (oldZoom / _zoom) * (zoomCoordsMousePos - _zoomCoordsOrigin);

            Event.current.Use();
            return;
        }

        // Allow moving the zoom area's origin by dragging with the middle mouse button or dragging
        // with the left mouse button with Alt pressed.
        if (Event.current.type == EventType.MouseDrag &&
            (Event.current.button == 0 && Event.current.modifiers == EventModifiers.Alt) ||
            Event.current.button == 2) {
            Vector2 delta = Event.current.delta;
            delta /= _zoom;
            _zoomCoordsOrigin += delta;

            Event.current.Use();
            return;
        }
        
    }

    public void OnGUI() {
        HandleEvents();

        // The zoom area clipping is sometimes not fully confined to the passed in rectangle. At certain
        // zoom levels you will get a line of pixels rendered outside of the passed in area because of
        // floating point imprecision in the scaling. Therefore, it is recommended to draw the zoom
        // area first and then draw everything else so that there is no undesired overlap.
        DrawZoomArea();
        DrawNonZoomArea();
    }
}
 