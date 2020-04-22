using QuickEditor.Common;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace QuickEditor.ToolKit
{
    [System.Serializable]
    public class TextureImportRule : AssetImportRule
    {
        public enum MaxTextureSizeEnum
        {
            _32 = 32,
            _64 = 64,
            _128 = 128,
            _256 = 256,
            _512 = 512,
            _1024 = 1024,
            _2048 = 2048,
            _4096 = 4096,
            _8192 = 8192
        }

        public TextureImporterType textureType = TextureImporterType.Default;
        public TextureImporterShape textureShape = TextureImporterShape.Texture2D;
        public bool allowsAlphaSplitting;
        public bool isReadable = false;
        public bool mipmapEnabled = false;
        public TextureImporterNPOTScale npotScale = TextureImporterNPOTScale.None;

        public TextureWrapMode wrapMode = TextureWrapMode.Clamp;
        public FilterMode filterMode = FilterMode.Bilinear;
        public int anisoLevel = 1;

        public bool autoMaxSize = true;
        public int maxTextureSize = 1024;
        public TextureImporterCompression textureCompression = TextureImporterCompression.Compressed;
        public UnityEditor.TextureCompressionQuality compressionQuality = UnityEditor.TextureCompressionQuality.Best;
        public TextureImporterPlatformSettings StandaloneTextureImporterSettings = new TextureImporterPlatformSettings();
        public TextureImporterPlatformSettings AndroidTextureImporterSettings = new TextureImporterPlatformSettings();
        public TextureImporterPlatformSettings iOSTextureImporterSettings = new TextureImporterPlatformSettings();

        private bool advanceFoldout = true;
        private int curPlatformIndex;
        private string[] mPlatformNames = new string[] { "Standalone", "Android", "iOS" };

        public override bool IsMatch(UnityEditor.AssetImporter importer)
        {
            if (importer is TextureImporter)
            {
                return true;
            }
            return false;
        }

        public override void ApplyDefaults()
        {
            this.RuleName = "New Texture Rule";
            this.SuffixFilter = ".png";

            textureType = TextureImporterType.Default;

            isReadable = false;
            mipmapEnabled = false;
            allowsAlphaSplitting = false;
            npotScale = TextureImporterNPOTScale.None;

            wrapMode = TextureWrapMode.Clamp;
            filterMode = FilterMode.Bilinear;

            textureCompression = TextureImporterCompression.Compressed;
        }

        public override void ApplySettings(UnityEditor.AssetImporter assetImporter)
        {
            TextureImporter importer = assetImporter as TextureImporter;
            if (importer == null) { return; }
            dirty = true;
            importer.textureType = textureType;
            importer.textureShape = textureShape;
            //importer.allowAlphaSplitting = allowsAlphaSplitting;
            importer.npotScale = npotScale;
            importer.isReadable = isReadable;
            importer.mipmapEnabled = mipmapEnabled;

            importer.wrapMode = wrapMode;
            importer.filterMode = filterMode;
            importer.anisoLevel = anisoLevel;

            importer.maxTextureSize = maxTextureSize;
            importer.textureCompression = textureCompression;

            SetAndroidTextureImporterSettings(importer, AndroidTextureImporterSettings.format);
            SetiPhoneTextureImporterSettings(importer, iOSTextureImporterSettings.format);
        }

        private void SetAndroidTextureImporterSettings(TextureImporter importer, TextureImporterFormat format)
        {
            TextureImporterPlatformSettings settings = importer.GetPlatformTextureSettings("Android");
            settings.maxTextureSize = maxTextureSize;
            settings.overridden = true;
            settings.allowsAlphaSplitting = allowsAlphaSplitting;
            settings.format = format;
            settings.compressionQuality = (int)compressionQuality;
            importer.SetPlatformTextureSettings(settings);
        }

        private void SetiPhoneTextureImporterSettings(TextureImporter importer, TextureImporterFormat format)
        {
            TextureImporterPlatformSettings settings = importer.GetPlatformTextureSettings("iPhone");
            settings.maxTextureSize = maxTextureSize;
            settings.allowsAlphaSplitting = allowsAlphaSplitting;
            settings.overridden = true;
            settings.format = format;
            settings.compressionQuality = (int)compressionQuality;
            importer.SetPlatformTextureSettings(settings);
        }

        public override void DrawRuleGUI()
        {
            base.DrawRuleGUI();
            GUILayout.Space(10);

            EditorGUILayout.LabelField("Texture Rule Settings", EditorStyles.boldLabel);
            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 2), QuickEditorColors.DarkGrayX11);
            GUILayout.Space(3);

            int mTextureTypeIndex = Math.Max(Array.FindIndex(AssetImportStyles.Texture.TextureTypeOpts, opt => opt.text.Equals(textureType.ToString())), 0);
            mTextureTypeIndex = EditorGUILayout.Popup(AssetImportStyles.Texture.TextureType, mTextureTypeIndex, AssetImportStyles.Texture.TextureTypeOpts);
            textureType = AssetImportStyles.Texture.TextureTypeEnumOpts[mTextureTypeIndex];

            bool disabled = (textureType == TextureImporterType.Default || textureType == TextureImporterType.NormalMap || textureType == TextureImporterType.SingleChannel) ? false : true;
            EditorGUI.BeginDisabledGroup(disabled);
            textureShape = (TextureImporterShape)EditorGUILayout.EnumPopup("Texture Shape", textureShape);
            EditorGUI.EndDisabledGroup();
            if (disabled)
                textureShape = TextureImporterShape.Texture2D;

            GUILayout.Space(5);

            advanceFoldout = EditorGUILayout.Foldout(advanceFoldout, AssetImportStyles.Texture.Advanced);

            if (advanceFoldout)
            {
                QuickGUILayout.IndentBlock(() =>
                {
                    npotScale = (TextureImporterNPOTScale)EditorGUILayout.EnumPopup(AssetImportStyles.Texture.NonPowerof2, npotScale);
                    isReadable = EditorGUILayout.Toggle(AssetImportStyles.Texture.ReadWriteEnable, isReadable);
                    mipmapEnabled = EditorGUILayout.Toggle(AssetImportStyles.Texture.GenMipMaps, mipmapEnabled);
                });
            }

            GUILayout.Space(5);

            wrapMode = (TextureWrapMode)EditorGUILayout.EnumPopup(AssetImportStyles.Texture.WrapMode, wrapMode);
            filterMode = (FilterMode)EditorGUILayout.EnumPopup(AssetImportStyles.Texture.FilterMode, filterMode);
            anisoLevel = EditorGUILayout.IntSlider(AssetImportStyles.Texture.AnisoLevel, anisoLevel, 0, 16);

            GUILayout.Space(10);

            GUILayout.BeginHorizontal(EditorStyles.toolbar);

            QuickGUILayout.Toolbar(ref curPlatformIndex, mPlatformNames, EditorStyles.toolbarButton, GUILayout.ExpandWidth(true));
            GUILayout.Space(10);
            GUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical("Box");

            if (curPlatformIndex == 0)
            {
                StandaloneTextureImporterSettings.overridden = EditorGUILayout.Toggle("Override for Standalone", StandaloneTextureImporterSettings.overridden);
            }
            else if (curPlatformIndex == 1)
            {
                AndroidTextureImporterSettings.overridden = EditorGUILayout.Toggle("Override for Android", AndroidTextureImporterSettings.overridden);
            }
            else if (curPlatformIndex == 2)
            {
                iOSTextureImporterSettings.overridden = EditorGUILayout.Toggle("Override for iOS", iOSTextureImporterSettings.overridden);
            }

            int num = Array.FindIndex(AssetImportStyles.Texture.MaxSizeOpts, content => content.text.Equals(maxTextureSize.ToString()));
            num = Math.Max(num, 0);
            num = EditorGUILayout.Popup(AssetImportStyles.Texture.MaxSize, num, AssetImportStyles.Texture.MaxSizeOpts);
            maxTextureSize = Convert.ToInt32(AssetImportStyles.Texture.MaxSizeOpts[num].text);

            if (curPlatformIndex == 0)
            {
                StandaloneTextureImporterSettings.resizeAlgorithm = (TextureResizeAlgorithm)EditorGUILayout.EnumPopup("Resize Algorithm", StandaloneTextureImporterSettings.resizeAlgorithm);
                StandaloneTextureImporterSettings.format = (TextureImporterFormat)EditorGUILayout.EnumPopup("Format", StandaloneTextureImporterSettings.format);
            }
            else if (curPlatformIndex == 1)
            {
                AndroidTextureImporterSettings.resizeAlgorithm = (TextureResizeAlgorithm)EditorGUILayout.EnumPopup("Resize Algorithm", AndroidTextureImporterSettings.resizeAlgorithm);
                AndroidTextureImporterSettings.format = (TextureImporterFormat)EditorGUILayout.EnumPopup("Format", AndroidTextureImporterSettings.format);
            }
            else if (curPlatformIndex == 2)
            {
                iOSTextureImporterSettings.resizeAlgorithm = (TextureResizeAlgorithm)EditorGUILayout.EnumPopup("Resize Algorithm", iOSTextureImporterSettings.resizeAlgorithm);
                iOSTextureImporterSettings.format = (TextureImporterFormat)EditorGUILayout.EnumPopup("Format", iOSTextureImporterSettings.format);
            }

            EditorGUILayout.EndVertical();
        }
    }
}