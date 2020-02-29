#if UNITY_EDITOR

namespace QuickEditor.ToolKit
{
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public class FolderOpener
    {
        private const int FolderOpenerMenuEntryPriority = 8000;
        private const string FolderOpenerMenuEntry = "Tools/QuickEditor.ToolKit/Folder Opener/";

        /// <summary>
        /// 打开 Data Path 文件夹。
        /// </summary>
        [MenuItem(FolderOpenerMenuEntry + "Application.dataPath", false, FolderOpenerMenuEntryPriority + 103)]
        private static void OpenDataPath()
        {
            RevealInFinder(Application.dataPath);
        }

        /// <summary>
        /// 打开 Persistent Data Path 文件夹。
        /// </summary>
        [MenuItem(FolderOpenerMenuEntry + "Application.persistentDataPath", false, FolderOpenerMenuEntryPriority + 101)]
        private static void OpenPersistentDataPath()
        {
            RevealInFinder(Application.persistentDataPath);
        }

        /// <summary>
        /// 打开 Streaming Assets Path 文件夹。
        /// </summary>
        [MenuItem(FolderOpenerMenuEntry + "Application.streamingAssetsPath", false, FolderOpenerMenuEntryPriority + 102)]
        private static void OpenStreamingAssets()
        {
            RevealInFinder(Application.streamingAssetsPath);
        }

        /// <summary>
        /// 打开 Temporary Cache Path 文件夹。
        /// </summary>
        [MenuItem(FolderOpenerMenuEntry + "Application.temporaryCachePath", false, FolderOpenerMenuEntryPriority + 100)]
        private static void OpenCachePath()
        {
            RevealInFinder(Application.temporaryCachePath);
        }

        [MenuItem(FolderOpenerMenuEntry + "Asset Store Packages Folder", false, FolderOpenerMenuEntryPriority + 200)]
        private static void OpenAssetStorePackagesFolder()
        {
            //http://answers.unity3d.com/questions/45050/where-unity-store-saves-the-packages.html
            //
#if UNITY_EDITOR_OSX
            string path = GetAssetStorePackagesPathOnMac();
#elif UNITY_EDITOR_WIN
            string path = GetAssetStorePackagesPathOnWindows();
#endif

            RevealInFinder(path);
        }

        [MenuItem(FolderOpenerMenuEntry + "Editor Application Path", false, FolderOpenerMenuEntryPriority + 200)]
        private static void OpenUnityEditorPath()
        {
            RevealInFinder(new FileInfo(EditorApplication.applicationPath).Directory.FullName);
        }

        [MenuItem(FolderOpenerMenuEntry + "Editor Log Folder", false, FolderOpenerMenuEntryPriority + 200)]
        private static void OpenEditorLogFolderPath()
        {
#if UNITY_EDITOR_OSX
			string rootFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			var libraryPath = Path.Combine(rootFolderPath, "Library");
			var logsFolder = Path.Combine(libraryPath, "Logs");
			var UnityFolder = Path.Combine(logsFolder, "Unity");
			RevealInFinder(UnityFolder);
#elif UNITY_EDITOR_WIN
            var rootFolderPath = System.Environment.ExpandEnvironmentVariables("%localappdata%");
            var unityFolder = Path.Combine(rootFolderPath, "Unity");
            RevealInFinder(Path.Combine(unityFolder, "Editor"));
#endif
        }

        [MenuItem(FolderOpenerMenuEntry + "Asset Backup Folder", false, FolderOpenerMenuEntryPriority + 300)]
        public static void OpenAEBackupFolder()
        {
            var folder = Path.Combine(Application.persistentDataPath, "AEBackup");
            Directory.CreateDirectory(folder);
            RevealInFinder(folder);
        }

        private const string ASSET_STORE_FOLDER_NAME = "Asset Store-5.x";

        private static string GetAssetStorePackagesPathOnMac()
        {
            var rootFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var libraryPath = Path.Combine(rootFolderPath, "Library");
            var unityFolder = Path.Combine(libraryPath, "Unity");
            return Path.Combine(unityFolder, ASSET_STORE_FOLDER_NAME);
        }

        private static string GetAssetStorePackagesPathOnWindows()
        {
            var rootFolderPath = System.Environment.ExpandEnvironmentVariables("%appdata%");
            var unityFolder = Path.Combine(rootFolderPath, "Unity");
            return Path.Combine(unityFolder, ASSET_STORE_FOLDER_NAME);
        }

        private static void RevealInFinder(string folder)
        {
            if (!Directory.Exists(folder))
            {
                UnityEngine.Debug.LogWarning(string.Format("Folder '{0}' is not Exists", folder));
                return;
            }

            EditorUtility.RevealInFinder(folder);
            //folder = string.Format("\"{0}\"", folder);
            //switch (Application.platform)
            //{
            //    case RuntimePlatform.WindowsEditor:
            //        Process.Start("Explorer.exe", folder.Replace('/', '\\'));
            //        break;

            //    case RuntimePlatform.OSXEditor:
            //        Process.Start("open", folder);
            //        break;

            //    default:
            //        throw new Exception(string.Format("Not support open folder on '{0}' platform.", Application.platform.ToString()));
            //}
        }
    }
}

#endif