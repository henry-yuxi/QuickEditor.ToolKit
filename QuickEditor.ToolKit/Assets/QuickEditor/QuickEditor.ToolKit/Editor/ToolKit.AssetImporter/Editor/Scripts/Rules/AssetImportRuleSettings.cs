namespace QuickEditor.ToolKit
{
    using QuickEditor.Common;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    [System.Serializable]
    public class AssetImportRuleSettings : QuickScriptableObject<AssetImportRuleSettings>
    {
        private static AssetImportRuleSettings mSetting;

        public static new AssetImportRuleSettings Current
        {
            get
            {
                if (mSetting == null)
                {
                    mSetting = QuickAssetDatabase.LoadAsset<AssetImportRuleSettings>("Assets/QuickEditor/QuickEditor.ToolKit/Editor/ToolKit.AssetImporter/Editor/Resources");
                }
                return mSetting;
            }
        }

        public new void Save()
        {
            if (mSetting == null) { return; }
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [SerializeField]
        //[HideInInspector]
        public List<AudioImportRule> AudioImportRuleSettings = new List<AudioImportRule>();

        [SerializeField]
        //[HideInInspector]
        public List<ModelImportRule> ModelImportRuleSettings = new List<ModelImportRule>();

        [SerializeField]
        //[HideInInspector]
        public List<TextureImportRule> TextureImportRuleSettings = new List<TextureImportRule>();
    }
}