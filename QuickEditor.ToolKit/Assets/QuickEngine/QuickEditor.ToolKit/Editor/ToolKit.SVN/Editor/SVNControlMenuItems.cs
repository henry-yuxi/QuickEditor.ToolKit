namespace QuickEditor.ToolKit.SVN
{
    using QuickEditor.Common;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    public class SVNControlMenuItems
    {
        private const string ToolsMenuEntry = "Tools/QuickEditor.ToolKit/SVNTools/";
        private const string AssetMenuEntry = "Assets/QuickEditor.ToolKit.SVNTools/";
        private const int MenuEntryPriority = 9000;

        [MenuItem(ToolsMenuEntry + "SVN CheckOut", false, priority = MenuEntryPriority + 100)]
        [MenuItem(AssetMenuEntry + "SVN CheckOut", false, priority = MenuEntryPriority + 100)]
        private static void SVNCheckOut()
        {
            SVNControlSettings.Current.ApplySettings();
            SVNControlTools.RunCommand(eSVNCmd.Checkout, ExternalSVNProjectPath, SVNControlSettings.Current.SVNRemoteUrl);
        }

        [MenuItem(ToolsMenuEntry + "SVN Add", true, priority = MenuEntryPriority + 200)]
        public static bool CheckSVNAdd()
        {
            return (Selection.assetGUIDs != null && Selection.assetGUIDs.Length >= 1);
        }

        [MenuItem(ToolsMenuEntry + "SVN Add", false, priority = MenuEntryPriority + 200)]
        [MenuItem(AssetMenuEntry + "SVN Add", false, priority = MenuEntryPriority + 200)]
        private static void SVNAdd()
        {
            SVNControlTools.RunCommand(eSVNCmd.Add, UnitySelectedPath);
        }

        [MenuItem(ToolsMenuEntry + "SVN Add", false, priority = MenuEntryPriority + 201)]
        [MenuItem(AssetMenuEntry + "SVN Add", false, priority = MenuEntryPriority + 201)]
        private static void SVNAddAll()
        {
            SVNControlTools.RunCommand(eSVNCmd.Add, UnityWholeProjectPath);
        }

        [MenuItem(ToolsMenuEntry + "SVN Commit", true, priority = MenuEntryPriority + 202)]
        public static bool CheckSVNCommit()
        {
            return (Selection.assetGUIDs != null && Selection.assetGUIDs.Length >= 1);
        }

        [MenuItem(ToolsMenuEntry + "SVN Commit", false, priority = MenuEntryPriority + 202)]
        [MenuItem(AssetMenuEntry + "SVN Commit", false, priority = MenuEntryPriority + 202)]
        private static void SVNCommit()
        {
            if (Selection.assetGUIDs == null || Selection.assetGUIDs.Length < 1) { return; }

            SVNControlTools.RunCommand(eSVNCmd.Commit, UnitySelectedPath);
        }

        [MenuItem(ToolsMenuEntry + "SVN Commit...", false, priority = MenuEntryPriority + 203)]
        [MenuItem(AssetMenuEntry + "SVN Commit...", false, priority = MenuEntryPriority + 203)]
        private static void SVNCommitAll()
        {
            SVNControlTools.RunCommand(eSVNCmd.Commit, UnityWholeProjectPath);
        }

        [MenuItem(ToolsMenuEntry + "SVN Update", true, priority = MenuEntryPriority + 204)]
        public static bool CheckSVNUpdate()
        {
            return (Selection.assetGUIDs != null && Selection.assetGUIDs.Length >= 1);
        }

        [MenuItem(ToolsMenuEntry + "SVN Update", false, priority = MenuEntryPriority + 204)]
        [MenuItem(AssetMenuEntry + "SVN Update", false, priority = MenuEntryPriority + 204)]
        private static void SVNUpdate()
        {
            SVNControlTools.RunCommand(eSVNCmd.Update, UnitySelectedPath);
        }

        [MenuItem(ToolsMenuEntry + "SVN Update...", false, priority = MenuEntryPriority + 205)]
        [MenuItem(AssetMenuEntry + "SVN Update...", false, priority = MenuEntryPriority + 205)]
        private static void SVNUpdateAll()
        {
            SVNControlTools.RunCommand(eSVNCmd.Update, ExternalSVNProjectPath);
        }

        [MenuItem(ToolsMenuEntry + "SVN Revert", false, priority = MenuEntryPriority + 206)]
        [MenuItem(AssetMenuEntry + "SVN Revert", false, priority = MenuEntryPriority + 206)]
        private static void SVNRevert()
        {
            SVNControlTools.RunCommand(eSVNCmd.Update, ExternalSVNProjectPath);
        }

        [MenuItem(ToolsMenuEntry + "Lock ", true, priority = MenuEntryPriority + 300)]
        public static bool CheckSVNLock()
        {
            return (Selection.assetGUIDs != null && Selection.assetGUIDs.Length >= 1);
        }

        [MenuItem(ToolsMenuEntry + "SVN Lock", false, priority = MenuEntryPriority + 300)]
        [MenuItem(AssetMenuEntry + "SVN Lock", false, priority = MenuEntryPriority + 300)]
        private static void SVNLock()
        {
            SVNControlTools.RunCommand(eSVNCmd.Lock, UnitySelectedPath);
        }

        [MenuItem(ToolsMenuEntry + "SVN Lock...", false, priority = MenuEntryPriority + 301)]
        [MenuItem(AssetMenuEntry + "SVN Lock...", false, priority = MenuEntryPriority + 301)]
        private static void SVNLockAll()
        {
            SVNControlTools.RunCommand(eSVNCmd.Lock, ExternalSVNProjectPath);
        }

        [MenuItem(ToolsMenuEntry + "SVN Unlock", true, priority = MenuEntryPriority + 302)]
        public static bool CheckSVNUnLock()
        {
            return (Selection.assetGUIDs != null && Selection.assetGUIDs.Length >= 1);
        }

        [MenuItem(ToolsMenuEntry + "SVN Unlock", false, priority = MenuEntryPriority + 302)]
        [MenuItem(AssetMenuEntry + "SVN Unlock", false, priority = MenuEntryPriority + 302)]
        private static void SVNUnLock()
        {
            SVNControlTools.RunCommand(eSVNCmd.Unlock, UnitySelectedPath);
        }

        [MenuItem(ToolsMenuEntry + "SVN Unlock...", false, priority = MenuEntryPriority + 303)]
        [MenuItem(AssetMenuEntry + "SVN Unlock...", false, priority = MenuEntryPriority + 303)]
        private static void SVNUnLockAll()
        {
            SVNControlTools.RunCommand(eSVNCmd.Unlock, ExternalSVNProjectPath);
        }

        [MenuItem(ToolsMenuEntry + "Log", false, priority = MenuEntryPriority + 400)]
        [MenuItem(AssetMenuEntry + "Log", false, priority = MenuEntryPriority + 400)]
        private static void SVNLog()
        {
            SVNControlTools.RunCommand(eSVNCmd.Log, UnitySelectedPath);
        }

        [MenuItem(ToolsMenuEntry + "Log...", false, priority = MenuEntryPriority + 401)]
        [MenuItem(AssetMenuEntry + "Log...", false, priority = MenuEntryPriority + 401)]
        private static void SVNLogAll()
        {
            SVNControlTools.RunCommand(eSVNCmd.Log, ExternalSVNProjectPath);
        }

        [MenuItem(ToolsMenuEntry + "Cleanup", false, priority = MenuEntryPriority + 500)]
        [MenuItem(AssetMenuEntry + "Cleanup", false, priority = MenuEntryPriority + 500)]
        private static void SVNCleanUp()
        {
            SVNControlTools.RunCommand(eSVNCmd.Cleanup, ExternalSVNProjectPath);
        }

        [MenuItem(ToolsMenuEntry + "Settings", false, priority = MenuEntryPriority + 501)]
        [MenuItem(AssetMenuEntry + "Settings", false, priority = MenuEntryPriority + 501)]
        private static void SVNSettings()
        {
            SVNControlTools.RunCommand(eSVNCmd.Settings);
        }

        [MenuItem(ToolsMenuEntry + "Help", false, priority = MenuEntryPriority + 502)]
        private static void SVNHelp()
        {
            Application.OpenURL("https://tortoisesvn.net/docs/release/TortoiseSVN_zh_CN/tsvn-automation.html#tsvn-automation-basics");
        }

        private static string UnitySelectedPath
        {
            get
            {
                List<string> assetPaths = new List<string>();
                string path = string.Empty;
                foreach (var guid in Selection.assetGUIDs)
                {
                    if (string.IsNullOrEmpty(guid)) { continue; }
                    path = AssetDatabase.GUIDToAssetPath(guid);
                    if (!string.IsNullOrEmpty(path)) { assetPaths.Add(path); }
                }
                return string.Join("*", assetPaths.ToArray());
            }
        }

        private static string UnityWholeProjectPath
        {
            get
            {
                List<string> pathList = new List<string>(){
                    ExternalSVNProjectPath + "/Assets",
                    ExternalSVNProjectPath + "/ProjectSettings"
                };
                return string.Join("*", pathList.ToArray());
            }
        }

        private static string UnitySVNProjectPath
        {
            get
            {
                return QuickPathUtils.ProjectPath;
            }
        }

        private static string ExternalSVNProjectPath
        {
            get
            {
                return SVNControlSettings.Current.SVNProjectPath;
            }
        }
    }
}