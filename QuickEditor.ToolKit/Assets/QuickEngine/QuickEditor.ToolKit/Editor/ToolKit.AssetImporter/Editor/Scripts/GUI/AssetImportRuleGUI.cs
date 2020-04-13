using QuickEditor.Common;
using System;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace QuickEditor.ToolKit
{
    public class AssetImportRuleGUI<T> : IAssetRuleGUI where T : AssetImportRule
    {
        internal ReorderableList mAssetImportRuleList;
        internal Vector2 mAssetImportRuleListScroll;
        internal Vector2 mAssetRuleScroll;

        public AssetImportRuleGUI()
        {
        }

        public virtual void OnInit()
        {
            mAssetImportRuleList = null;
        }

        public virtual void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();

            Rect windowRect = AssetImportRuleWindow.Instance.position;
            EditorGUILayout.BeginVertical(GUILayout.Width(windowRect.width * 0.4f), GUILayout.ExpandHeight(true));
            //GUILayoutHelper.DrawHeader(typeof(T).Name);
            DrawLeftGUI();
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            DrawRightGUI();
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }

        protected virtual void DrawLeftGUI()
        {
        }

        protected virtual void DrawRightGUI()
        {
            if (mAssetImportRuleList.index < 0)
            {
                mAssetImportRuleList.index = 0;
            }
        }

        protected virtual void OnRuleGUI(T rule)
        {
            EditorGUILayout.LabelField("Rule Settings", EditorStyles.boldLabel);
            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 2), QuickEditorColors.DarkGrayX11);
            GUILayout.Space(3);

            rule.RuleName = EditorGUILayout.TextField(new GUIContent("Rule Name", "Only used for organisation"), rule.RuleName);
            rule.FolderFilter = EditorGUILayout.TextField(new GUIContent("Path Contains Filter", "Applied only if the path contains this string. Leave empty to apply to all paths. Separate multiple filters with ;"), rule.FolderFilter);
            rule.FileFilter = EditorGUILayout.TextField(new GUIContent("Filename Contains Filter", "Applied only if the filename contains this string. Leave empty to apply to all filenames. Separate multiple filters with ;"), rule.FileFilter);
        }

        protected void DoReorderableList(IList list, Type listType)
        {
            QuickGUILayout.DoReorderableList<AssetImportRuleSettings>(AssetImportRuleSettings.Current, list, listType, ref mAssetImportRuleList, ref mAssetImportRuleListScroll, ref mAssetRuleScroll, (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var elem = (AssetImportRule)list[index];
                GUI.Label(rect, elem.RuleName);
                rect.x = rect.xMax - 15;
                elem.IsEnabled = GUI.Toggle(rect, elem.IsEnabled, string.Empty);
            }, () =>
            {
                for (int i = 0; i < list.Count; i++)
                {
                    ((AssetImportRule)list[i]).IsEnabled = true;
                }
            }, () =>
            {
                for (int i = 0; i < list.Count; i++)
                {
                    ((AssetImportRule)list[i]).IsEnabled = false;
                }
            });
        }
    }
}