namespace QuickEditor.ToolKit
{
    using QuickEditor.Common;
    using UnityEditor;
    using UnityEngine;

    public class AssetImportRuleWindow : QuickEditorWindow
    {
        internal const string ToolsMenuEntry = "Tools/QuickEditor.ToolKit/";
        internal const int MenuEntryPriority = 1000;

        private enum AssetType
        {
            Audio,
            Model,
            Texture
        }

        private string[] mAssetTabNames = System.Enum.GetNames(typeof(AssetType));
        private int mCurAssetTabIndex = 0;
        private TextureImportRuleGUI textureRuleGUI = new TextureImportRuleGUI();
        private ModelImportRuleGUI modelRuleGUI = new ModelImportRuleGUI();
        private AudioImportRuleGUI audioRuleGUI = new AudioImportRuleGUI();
        private IAssetRuleGUI curPanel;

        protected static AssetImportRuleWindow mWindow;

        [MenuItem(ToolsMenuEntry + "Asset Import Rule", false, MenuEntryPriority)]
        private static void Init()
        {
            if (Application.isPlaying || EditorApplication.isPlaying || EditorApplication.isPaused)
            {
                EditorUtility.DisplayDialog("错误", "游戏正在运行或者暂停中, 请不要操作!", "确定");
                return;
            }

            if (EditorApplication.isCompiling)
            {
                EditorUtility.DisplayDialog("错误", "游戏脚本正在编译, 请不要操作!", "确定");
                return;
            }
            WindowTitle = "Asset Import Rule";
            mWindow = GetEditorWindow<AssetImportRuleWindow>();
            Undo.undoRedoPerformed += () =>
            {
                mWindow.Repaint();
            };
        }

        public static AssetImportRuleWindow Instance
        {
            get { return mWindow; }
        }

        protected override void OnEnable()
        {
            mWindow = this;
            curPanel = GetCurRuleGUI();
        }

        protected override void OnGUI()
        {
            //EditorGUI.BeginChangeCheck();

            DrawTopBar();

            if (curPanel != null) { curPanel.OnGUI(); }

            //if (EditorGUI.EndChangeCheck())
            //{
            //    EditorUtility.SetDirty(AssetImportRuleSettings.Current);
            //}
        }

        protected void DrawTopBar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

            EditorGUI.BeginChangeCheck();
            QuickGUILayout.Toolbar(ref mCurAssetTabIndex, mAssetTabNames, EditorStyles.toolbarButton, GUILayout.Width(350));
            GUILayout.Toolbar(0, new[] { "" }, EditorStyles.toolbar, GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Apply", EditorStyles.toolbarButton, GUILayout.Width(100)))
            {
                AssetImportRuleSettings.Current.Save();
            }
            if (EditorGUI.EndChangeCheck())
            {
                curPanel = GetCurRuleGUI();
                if (curPanel != null) { curPanel.OnInit(); }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 2), QuickEditorColors.DarkGrayX11);
            EditorGUILayout.Space();
        }

        public IAssetRuleGUI GetCurRuleGUI()
        {
            switch ((AssetType)mCurAssetTabIndex)
            {
                case AssetType.Audio: return audioRuleGUI;
                case AssetType.Model: return modelRuleGUI;
                case AssetType.Texture: return textureRuleGUI;
                default: break;
            }
            return null;
        }
    }
}