using UnityEngine;
using UnityEditor;

public class GUIStyleViewer : EditorWindow
{
    private Vector2 scrollVector2 = Vector2.zero;
    private string search = "";

    [MenuItem("UFramework/GUIStyle查看器")]
    public static void InitWindow()
    {
        EditorWindow.GetWindow(typeof(GUIStyleViewer));
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal("HelpBox");
        GUILayout.Space(30);
        search = EditorGUILayout.TextField("", search, "SearchTextField", GUILayout.MaxWidth(position.x / 3));
        GUILayout.Label("", "SearchCancelButtonEmpty");
        GUILayout.EndHorizontal();
        scrollVector2 = GUILayout.BeginScrollView(scrollVector2);
        foreach (GUIStyle style in GUI.skin.customStyles)
        {
            if (style.name.ToLower().Contains(search.ToLower()))
            {
                DrawStyleItem(style);
            }
        }
        GUILayout.EndScrollView();
    }

    void DrawStyleItem(GUIStyle style)
    {
        GUILayout.BeginHorizontal("box");
        GUILayout.Space(40);
        EditorGUILayout.SelectableLabel(style.name);
        GUILayout.FlexibleSpace();
        EditorGUILayout.SelectableLabel(style.name, style);
        GUILayout.Space(40);
        EditorGUILayout.SelectableLabel("", style, GUILayout.Height(40), GUILayout.Width(40));
        GUILayout.Space(50);
        if (GUILayout.Button("复制到剪贴板"))
        {
            TextEditor textEditor = new TextEditor();
            textEditor.text = style.name;
            textEditor.OnFocus();
            textEditor.Copy();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
    }
}

//public class GUIStyleViewer : EditorWindow
//{

//    Vector2 scrollPosition = new Vector2(0, 0);
//    string search = "";
//    GUIStyle textStyle;


//    private static GUIStyleViewer window;
//    [MenuItem("Tools/GUIStyleViewer", false, 100)]
//    private static void OpenStyleViewer()
//    {
//        window = GetWindow<GUIStyleViewer>(false, "查看内置GUIStyle");
//    }

//    void OnGUI()
//    {
//        if (textStyle == null)
//        {
//            textStyle = new GUIStyle("HeaderLabel");
//            textStyle.fontSize = 20;
//        }

//        GUILayout.BeginHorizontal("HelpBox");
//        GUILayout.Label("点击示例，可以将其名字复制下来", textStyle);
//        GUILayout.FlexibleSpace();
//        GUILayout.Label("Search:");
//        search = EditorGUILayout.TextField(search);
//        GUILayout.EndHorizontal();

//        GUILayout.BeginHorizontal("PopupCurveSwatchBackground");
//        GUILayout.Label("示例", textStyle, GUILayout.Width(300));
//        GUILayout.Label("名字", textStyle, GUILayout.Width(300));
//        GUILayout.EndHorizontal();


//        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

//        foreach (var style in GUI.skin.customStyles)
//        {
//            if (style.name.ToLower().Contains(search.ToLower()))
//            {
//                GUILayout.Space(15);
//                GUILayout.BeginHorizontal("PopupCurveSwatchBackground");
//                if (GUILayout.Button(style.name, style, GUILayout.Width(300)))
//                {
//                    EditorGUIUtility.systemCopyBuffer = style.name;
//                    Debug.LogError(style.name);
//                }
//                EditorGUILayout.SelectableLabel(style.name, GUILayout.Width(300));
//                GUILayout.EndHorizontal();
//            }
//        }

//        GUILayout.EndScrollView();
//    }
//}