using QuickEditor.Common;
using UnityEditor;
using UnityEngine;

namespace QuickEditor.ToolKit
{
    [System.Serializable]
    public class AssetImportRule
    {
        public bool IsEnabled = true;
        public string RuleName = "New Asset Rule";
        public string FolderFilter = string.Empty;
        public string FileFilter = string.Empty;
        public string SuffixFilter = string.Empty;
        protected bool dirty;

        public virtual bool IsMatch(UnityEditor.AssetImporter importer)
        {
            return false;
        }

        public virtual void ApplyDefaults()
        {
            this.IsEnabled = true;
            this.RuleName = "New Asset Rule";
            this.FolderFilter = string.Empty;
            this.FileFilter = string.Empty;
            this.SuffixFilter = string.Empty;
        }

        public void ApplySettings(string assetPath)
        {
            ApplySettings(UnityEditor.AssetImporter.GetAtPath(assetPath));
        }

        public virtual void ApplySettings(UnityEditor.AssetImporter importer)
        {
        }

        protected virtual void DrawFilterGUI()
        {
            EditorGUILayout.LabelField("Rule Settings", EditorStyles.boldLabel);
            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 2), QuickEditorColors.DarkGrayX11);
            GUILayout.Space(3);

            RuleName = EditorGUILayout.TextField(new GUIContent("Rule Name", "Only used for organisation"), RuleName);
            FolderFilter = EditorGUILayout.TextField(new GUIContent("Folder Contains Filter", "Applied only if the path contains this string. Leave empty to apply to all paths. Separate multiple filters with ;"), FolderFilter);
            FileFilter = EditorGUILayout.TextField(new GUIContent("Filename Contains Filter", "Applied only if the filename contains this string. Leave empty to apply to all filenames. Separate multiple filters with ;"), FileFilter);
            SuffixFilter = EditorGUILayout.TextField(new GUIContent("Suffix Contains Filter", "Applied only if the suffix contains this string. Leave empty to apply to all suffixs. Separate multiple filters with ;"), SuffixFilter);
            //if (FolderFilter.Length == 0 && FileFilter.Length == 0)
            //{
            //    EditorGUILayout.HelpBox("Empty filters means this will be applied to all imported meshes", MessageType.Info);
            //}
        }

        public virtual void DrawRuleGUI()
        {
            DrawFilterGUI();
        }
    }
}