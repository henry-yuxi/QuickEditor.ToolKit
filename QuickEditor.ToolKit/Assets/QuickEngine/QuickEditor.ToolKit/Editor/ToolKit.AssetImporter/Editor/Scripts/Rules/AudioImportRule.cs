namespace QuickEditor.ToolKit
{
    using QuickEditor.Common;
    using System;
    using UnityEditor;
    using UnityEngine;

    [System.Serializable]
    public class AudioImportRule : AssetImportRule
    {
        public enum SampleRates
        {
            _8000Hz = 8000,
            _11025Hz = 11025,
            _22050Hz = 22050,
            _44100Hz = 44100,
            _48000Hz = 48000,
            _96000Hz = 96000,
            _192000Hz = 192000,
        }

        public bool forceToMono = false;
        public bool normalize = true;
        public bool loadInBackground = true;
        public bool ambisonic = false;

        public AudioClipLoadType loadType = AudioClipLoadType.Streaming;
        public bool preloadAudioData = true;
        public AudioCompressionFormat compressionFormat = AudioCompressionFormat.Vorbis;
        public int quality = 100;
        public AudioSampleRateSetting sampleRateSetting = AudioSampleRateSetting.PreserveSampleRate;
        public SampleRates sampleRate = SampleRates._8000Hz;

        public override bool IsMatch(AssetImporter importer)
        {
            if (importer is AudioImporter)
            {
                return true;
            }
            return false;
        }

        public override void ApplyDefaults()
        {
            base.ApplyDefaults();

            this.SuffixFilter = ".ogg";

            this.forceToMono = false;
            this.normalize = true;
            this.loadInBackground = true;
            this.ambisonic = false;
            this.loadType = AudioClipLoadType.Streaming;
            this.preloadAudioData = true;
            this.compressionFormat = AudioCompressionFormat.Vorbis;
            this.quality = 100;
            this.sampleRateSetting = AudioSampleRateSetting.PreserveSampleRate;
        }

        public override void ApplySettings(UnityEditor.AssetImporter assetImporter)
        {
            AudioImporter importer = assetImporter as AudioImporter;

            if (importer == null) { return; }

            importer.forceToMono = forceToMono;
            if (forceToMono)
            {
                //importer.normalize = normalize;
            }
            importer.loadInBackground = loadInBackground;
            importer.ambisonic = ambisonic;
            importer.preloadAudioData = preloadAudioData;

            AudioImporterSampleSettings sampleSettings = importer.defaultSampleSettings;
            sampleSettings.loadType = loadType;
            sampleSettings.compressionFormat = compressionFormat;
            sampleSettings.quality = quality;
            sampleSettings.sampleRateSetting = sampleRateSetting;
            sampleSettings.sampleRateOverride = (uint)sampleRate;

            importer.defaultSampleSettings = sampleSettings;
            importer.SetOverrideSampleSettings("Android", sampleSettings);
            importer.SetOverrideSampleSettings("iPhone", sampleSettings);
            //importer.SaveAndReimport();
        }

        public override void DrawRuleGUI()
        {
            base.DrawRuleGUI();
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Audio Import Settings", EditorStyles.boldLabel);
            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 2), QuickEditorColors.DarkGrayX11);
            GUILayout.Space(3);

            forceToMono = EditorGUILayout.Toggle(AssetImportStyles.Audio.ForceToMono, forceToMono);
            EditorGUI.indentLevel++;
            EditorGUI.BeginDisabledGroup(forceToMono);
            normalize = EditorGUILayout.Toggle(AssetImportStyles.Audio.Normalize, normalize);
            EditorGUI.EndDisabledGroup();
            EditorGUI.indentLevel--;
            loadInBackground = EditorGUILayout.Toggle(AssetImportStyles.Audio.LoadBackground, loadInBackground);
            ambisonic = EditorGUILayout.Toggle(AssetImportStyles.Audio.Ambisonic, ambisonic);

            GUILayout.Space(6);
            EditorGUILayout.LabelField("Default Import Settings", EditorStyles.boldLabel);
            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 2), QuickEditorColors.DarkGrayX11);
            GUILayout.Space(3);

            loadType = (AudioClipLoadType)EditorGUILayout.EnumPopup(AssetImportStyles.Audio.LoadType, loadType);

            EditorGUI.BeginDisabledGroup(loadType == AudioClipLoadType.Streaming ? true : false);
            preloadAudioData = EditorGUILayout.Toggle(AssetImportStyles.Audio.PreloadAudioData, preloadAudioData);
            EditorGUI.EndDisabledGroup();

            if (loadType == AudioClipLoadType.Streaming)
                preloadAudioData = false;

            int compressionFormatIndex = Math.Max(Array.FindIndex(AssetImportStyles.Audio.CompressionEnumOpts, opt => opt.Equals(compressionFormat)), 0);
            compressionFormatIndex = EditorGUILayout.Popup(AssetImportStyles.Audio.CompressionFormat, compressionFormatIndex, AssetImportStyles.Audio.CompressionFormatOpts);
            compressionFormat = AssetImportStyles.Audio.CompressionEnumOpts[compressionFormatIndex];
            EditorGUI.BeginDisabledGroup(compressionFormat != AudioCompressionFormat.Vorbis ? true : false);
            quality = EditorGUILayout.IntSlider(AssetImportStyles.Audio.Quality, quality, 1, 100);
            EditorGUI.EndDisabledGroup();

            sampleRateSetting = (AudioSampleRateSetting)EditorGUILayout.EnumPopup(AssetImportStyles.Audio.SampleRateSetting, sampleRateSetting);
            if (sampleRateSetting == AudioSampleRateSetting.OverrideSampleRate)
            {
                sampleRate = (SampleRates)EditorGUILayout.EnumPopup(AssetImportStyles.Audio.SampleRate, sampleRate);
            }
        }
    }
}