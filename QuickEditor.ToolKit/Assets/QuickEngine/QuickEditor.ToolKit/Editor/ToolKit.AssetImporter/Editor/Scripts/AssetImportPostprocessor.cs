namespace QuickEditor.ToolKit
{
    using System.Collections;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public class AssetImportPostprocessor : AssetPostprocessor
    {
        private void OnPreprocessAudio()
        {
            ExcuteAudioRule(assetImporter);
        }

        private void OnPreprocessModel()
        {
            ExcuteModelRule(assetImporter);
        }

        private void OnPreprocessTexture()
        {
            ExcuteTextureRule(assetImporter);
        }

        //static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        //{
        //    for (int i = 0; i < movedAssets.Length; i++)
        //    {
        //        AssetImporter importer = AssetImporter.GetAtPath(movedAssets[i]);
        //        ExcuteAudioRule(importer);
        //        ExcuteModelRule(importer);
        //        ExcuteTextureRule(importer);
        //    }
        //}

        private void ExcuteAudioRule(AssetImporter importer)
        {
            if (EditorApplication.isCompiling) return;
            if (AssetImportRuleSettings.Current == null) { return; }
            AudioImportRule rule = FindAssetRule(AssetImportRuleSettings.Current.AudioImportRuleSettings, assetImporter) as AudioImportRule;
            if (rule == null)
            {
                Debug.Log("No asset rules found for asset");
            }
            else
            {
                Debug.Log("Begin to Apply Audio Settings -> asset: " + importer.assetPath);
                rule.ApplySettings(importer);
            }
        }

        private void ExcuteModelRule(AssetImporter importer)
        {
            if (EditorApplication.isCompiling) { return; }
            if (AssetImportRuleSettings.Current == null) { return; }
            ModelImportRule rule = FindAssetRule(AssetImportRuleSettings.Current.ModelImportRuleSettings, assetImporter) as ModelImportRule;
            if (rule == null)
            {
                Debug.Log("No asset rules found for asset");
            }
            else
            {
                Debug.Log("Begin to Apply Model Settings -> asset: " + importer.assetPath);
                rule.ApplySettings(importer);
            }
        }

        private void ExcuteTextureRule(AssetImporter importer)
        {
            if (EditorApplication.isCompiling) { return; }
            if (AssetImportRuleSettings.Current == null) { return; }
            TextureImportRule rule = FindAssetRule(AssetImportRuleSettings.Current.TextureImportRuleSettings, assetImporter) as TextureImportRule;
            if (rule == null)
            {
                Debug.Log("No asset rules found for asset");
            }
            else
            {
                Debug.Log("Begin to Apply Texture Settings -> asset: " + importer.assetPath);
                rule.ApplySettings(importer);
            }
        }

        private AssetImportRule FindAssetRule(IList rules, AssetImporter importer)
        {
            if (rules.Count <= 0) { return null; }
            for (int i = 0; i < rules.Count; i++)
            {
                if (FilterRule(rules[i] as AssetImportRule))
                {
                    return rules[i] as AssetImportRule;
                }
            }
            return null;
        }

        private char[] filterSeparator = new char[] { ';' };

        private bool FilterRule(AssetImportRule rule)
        {
            if (!rule.IsEnabled) return false;

            bool pathFilter = false;
            bool filenameFilter = false;
            bool suffixFilter = false;

            if (string.IsNullOrEmpty(rule.FolderFilter)) { pathFilter = true; }
            if (string.IsNullOrEmpty(rule.FileFilter)) { filenameFilter = true; }
            if (string.IsNullOrEmpty(rule.SuffixFilter)) { suffixFilter = true; }

            string fileName = Path.GetFileName(assetPath);

            string[] pathFilterSplit = rule.FolderFilter.Split(filterSeparator, System.StringSplitOptions.RemoveEmptyEntries);
            string[] filenameFilterSplit = rule.FileFilter.Split(filterSeparator, System.StringSplitOptions.RemoveEmptyEntries);
            string[] suffixFilterSplit = rule.FileFilter.Split(filterSeparator, System.StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < pathFilterSplit.Length; i++)
            {
                if (assetPath.Contains(pathFilterSplit[i]))
                {
                    pathFilter = true;
                }
            }

            for (int i = 0; i < filenameFilterSplit.Length; i++)
            {
                if (fileName.Contains(filenameFilterSplit[i]))
                {
                    filenameFilter = true;
                }
            }
            for (int i = 0; i < suffixFilterSplit.Length; i++)
            {
                if (fileName.Contains(suffixFilterSplit[i]))
                {
                    suffixFilter = true;
                }
            }
            return pathFilter && filenameFilter && suffixFilter;
        }
    }
}