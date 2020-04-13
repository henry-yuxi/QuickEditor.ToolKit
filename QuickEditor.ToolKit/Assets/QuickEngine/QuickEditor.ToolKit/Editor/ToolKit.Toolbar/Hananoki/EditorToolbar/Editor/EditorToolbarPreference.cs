//#define ENABLE_LEGACY_PREFERENCE
using Hananoki.Shared.Localize;
using HananokiEditor;
using UnityEditor;
using UnityEngine;
using Pref = Hananoki.EditorToolbar.EditorToolbarPreference;
using Settings = Hananoki.EditorToolbar.EditorToolbarSettings;

namespace Hananoki.EditorToolbar
{
    [InitializeOnLoad]
    [System.Serializable]
    public class EditorToolbarPreference
    {
        public bool enableProjectSettingsProvider;

        public string iconOpenCSProject;

        public static Pref i;

        public static void Load()
        {
            if (i != null) return;
            i = EditorPrefJson<Pref>.Get(EditorToolbar.PREF_NAME);
            Settings.Load();
        }

        public static void Save()
        {
            EditorPrefJson<Pref>.Set(EditorToolbar.PREF_NAME, i);
            Settings.Save();
        }
    }

    public class EditorToolbarPreferenceWindow : HSettingsEditorWindow
    {
        public static void Open()
        {
            var window = GetWindow<EditorToolbarPreferenceWindow>();
            window.SetTitle(new GUIContent(EditorToolbar.PACKAGE_NAME, Icon.Get("SettingsIcon")));
        }

        private void OnEnable()
        {
            drawGUI = DrawGUI;
            Pref.Load();
        }

        /// <summary>
        ///
        /// </summary>
        private static void DrawGUI()
        {
            using (new PreferenceLayoutScope())
            {
                EditorGUI.BeginChangeCheck();
                Pref.i.enableProjectSettingsProvider = HEditorGUILayout.ToggleLeft(S._ProjectSettingsProvider, Pref.i.enableProjectSettingsProvider);
                Pref.i.iconOpenCSProject = HEditorGUILayout.GUIDObjectField<Texture2D>(nameof(Pref.i.iconOpenCSProject).nicify(), Pref.i.iconOpenCSProject);

                if (EditorGUI.EndChangeCheck())
                {
                    EditorToolbar.s_styles.LoadProjectIcon();
                    EditorToolbar.Repaint();
                    Pref.Save();
                }

                if (Pref.i.enableProjectSettingsProvider) return;

                GUILayout.Space(8f);

                GUILayout.Label(S._ProjectSettings, "ShurikenModuleTitle");
#if UNITY_2018_3_OR_NEWER
				EditorToolbarSettingsProvider.DrawGUI();
#endif
            }
        }

#if UNITY_2018_3_OR_NEWER && !ENABLE_LEGACY_PREFERENCE

		static void titleBarGuiHandler() {
			GUILayout.Label( $"{EditorToolbar.VER}", EditorStyles.miniLabel );
		}

		[SettingsProvider]
		public static SettingsProvider PreferenceView() {
			var provider = new SettingsProvider( $"Preferences/Hananoki/{EditorToolbar.PACKAGE_NAME}", SettingsScope.User ) {
				label = $"{EditorToolbar.PACKAGE_NAME}",
				guiHandler = PreferencesGUI,
				titleBarGuiHandler = titleBarGuiHandler,
			};
			return provider;
		}

		public static void PreferencesGUI( string searchText ) {
#else

        [PreferenceItem(EditorToolbar.PACKAGE_NAME)]
        public static void PreferencesGUI()
        {
#endif
            Pref.Load();
            DrawGUI();
        }
    }
}