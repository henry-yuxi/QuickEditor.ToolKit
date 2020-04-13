using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using Pref = Hananoki.EditorToolbar.EditorToolbarPreference;
using Settings = Hananoki.EditorToolbar.EditorToolbarSettings;

namespace Hananoki.EditorToolbar
{
    [Serializable]
    public class EditorToolbarSettings
    {
        private static string jsonPath => $"{Environment.CurrentDirectory}/ProjectSettings/Hananoki.EditorToolbar.json";

        [System.Serializable]
        public class Module
        {
            public string assemblyName;
            public string className;

            public Module(string assemblyName, string className)
            {
                this.assemblyName = assemblyName;
                this.className = className;
            }
        }

        public List<Module> reg;

        public static Settings i;

        private EditorToolbarSettings()
        {
            reg = new List<Module>();
        }

        public static void Load()
        {
            i = JsonUtility.FromJson<Settings>(fs.ReadAllText(jsonPath));
            if (i == null)
            {
                i = new Settings();
                //Debug.Log( "new EditorToolbarSettings" );
                Save();
            }
        }

        public static void Save()
        {
            File.WriteAllText(jsonPath, JsonUtility.ToJson(i, true));
        }
    }

#if UNITY_2018_3_OR_NEWER

    public class EditorToolbarSettingsProvider : UnityEditor.SettingsProvider
    {
        public EditorToolbarSettingsProvider(string path, UnityEditor.SettingsScope scope) : base(path, scope)
        {
        }

        //public override void OnActivate( string searchContext, VisualElement rootElement ) {}

        //public override void OnDeactivate() {}

        public override void OnTitleBarGUI()
        {
            GUILayout.Label($"{EditorToolbar.VER}", UnityEditor.EditorStyles.miniLabel);
        }

        public static void DrawGUI()
        {
            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Register Class"))
                {
                    var t = typeof(EditorToolbarClass);
                    Settings.i.reg = new List<Settings.Module>();
                    foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        foreach (Type type in assembly.GetTypes())
                        {
                            if (type.GetCustomAttribute(t) == null) continue;
                            Settings.i.reg.Add(new Settings.Module(assembly.FullName.Split(',')[0], type.FullName));
                        }
                    }
                    Settings.Save();
                    EditorToolbar.MakeMenuCommand();
                }
                if (GUILayout.Button("Unregister Class"))
                {
                    Settings.i.reg = new List<Settings.Module>();
                    Settings.Save();
                    EditorToolbar.MakeMenuCommand();
                }
            }
            if (Settings.i.reg != null)
            {
                foreach (var p in Settings.i.reg)
                {
                    UnityEditor.EditorGUILayout.LabelField($"{p.assemblyName} : {p.className}");
                }
            }
        }

        public override void OnGUI(string searchContext)
        {
            DrawGUI();
        }

        //public override void OnFooterBarGUI() {}

        [UnityEditor.SettingsProvider]
        private static UnityEditor.SettingsProvider Create()
        {
            if (!Pref.i.enableProjectSettingsProvider) return null;
            var provider = new EditorToolbarSettingsProvider($"Hananoki/{EditorToolbar.PACKAGE_NAME}", UnityEditor.SettingsScope.Project);

            return provider;
        }
    }

#endif
}