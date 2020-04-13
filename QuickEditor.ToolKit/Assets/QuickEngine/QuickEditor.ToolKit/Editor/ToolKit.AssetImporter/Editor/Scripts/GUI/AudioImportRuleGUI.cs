namespace QuickEditor.ToolKit
{
    using QuickEditor.Common;
    using System;
    using UnityEditor;
    using UnityEngine;

    public class AudioImportRuleGUI : AssetImportRuleGUI<AudioImportRule>
    {
        public override void OnInit()
        {
            base.OnInit();
        }

        protected override void DrawLeftGUI()
        {
            base.DrawLeftGUI();
            DoReorderableList(AssetImportRuleSettings.Current.AudioImportRuleSettings, typeof(AudioImportRule));
        }

        protected override void DrawRightGUI()
        {
            base.DrawRightGUI();

            using (var scroll = new EditorGUILayout.ScrollViewScope(mAssetRuleScroll))
            {
                mAssetRuleScroll = scroll.scrollPosition;
                if (mAssetImportRuleList.index >= AssetImportRuleSettings.Current.AudioImportRuleSettings.Count)
                {
                    mAssetImportRuleList.index = 0;
                }
                if (AssetImportRuleSettings.Current.AudioImportRuleSettings.Count > 0)
                {
                    //OnRuleGUI(AssetImportRuleSettings.Current.AudioImportRuleSettings[mAssetImportRuleList.index]);
                    AssetImportRuleSettings.Current.AudioImportRuleSettings[mAssetImportRuleList.index].DrawRuleGUI();
                }
            }
        }

        protected override void OnRuleGUI(AudioImportRule rule)
        {
            base.OnRuleGUI(rule);
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Audio Import Settings", EditorStyles.boldLabel);
            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 2), QuickEditorColors.DarkGrayX11);
            GUILayout.Space(3);

            rule.forceToMono = EditorGUILayout.Toggle(AssetImportStyles.Audio.ForceToMono, rule.forceToMono);
            EditorGUI.indentLevel++;
            EditorGUI.BeginDisabledGroup(rule.forceToMono);
            rule.normalize = EditorGUILayout.Toggle(AssetImportStyles.Audio.Normalize, rule.normalize);
            EditorGUI.EndDisabledGroup();
            EditorGUI.indentLevel--;
            rule.loadInBackground = EditorGUILayout.Toggle(AssetImportStyles.Audio.LoadBackground, rule.loadInBackground);
            rule.ambisonic = EditorGUILayout.Toggle(AssetImportStyles.Audio.Ambisonic, rule.ambisonic);

            GUILayout.Space(6);
            EditorGUILayout.LabelField("Default Import Settings", EditorStyles.boldLabel);
            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 2), QuickEditorColors.DarkGrayX11);
            GUILayout.Space(3);

            rule.loadType = (AudioClipLoadType)EditorGUILayout.EnumPopup(AssetImportStyles.Audio.LoadType, rule.loadType);

            EditorGUI.BeginDisabledGroup(rule.loadType == AudioClipLoadType.Streaming ? true : false);
            rule.preloadAudioData = EditorGUILayout.Toggle(AssetImportStyles.Audio.PreloadAudioData, rule.preloadAudioData);
            EditorGUI.EndDisabledGroup();

            if (rule.loadType == AudioClipLoadType.Streaming)
                rule.preloadAudioData = false;

            int compressionFormatIndex = Math.Max(Array.FindIndex(AssetImportStyles.Audio.CompressionEnumOpts, opt => opt.Equals(rule.compressionFormat)), 0);
            compressionFormatIndex = EditorGUILayout.Popup(AssetImportStyles.Audio.CompressionFormat, compressionFormatIndex, AssetImportStyles.Audio.CompressionFormatOpts);
            rule.compressionFormat = AssetImportStyles.Audio.CompressionEnumOpts[compressionFormatIndex];
            EditorGUI.BeginDisabledGroup(rule.compressionFormat != AudioCompressionFormat.Vorbis ? true : false);
            rule.quality = EditorGUILayout.IntSlider(AssetImportStyles.Audio.Quality, rule.quality, 1, 100);
            EditorGUI.EndDisabledGroup();

            rule.sampleRateSetting = (AudioSampleRateSetting)EditorGUILayout.EnumPopup(AssetImportStyles.Audio.SampleRateSetting, rule.sampleRateSetting);
            if (rule.sampleRateSetting == AudioSampleRateSetting.OverrideSampleRate)
            {
                rule.sampleRate = (AudioImportRule.SampleRates)EditorGUILayout.EnumPopup(AssetImportStyles.Audio.SampleRate, rule.sampleRate);
            }
        }
    }
}