using Hananoki.Reflection;
using Hananoki.Shared.Localize;
using HananokiEditor;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityToolbarExtender;
using Pref = Hananoki.EditorToolbar.EditorToolbarPreference;
using Settings = Hananoki.EditorToolbar.EditorToolbarSettings;

namespace Hananoki.EditorToolbar
{
    [InitializeOnLoad]
    public class EditorToolbar
    {
        public const string PACKAGE_NAME = "EditorToolbar";
        public const string PREF_NAME = "Hananoki.EditorToolbar";
        public const string VER = "WIP";

        public struct BuildTargetInfo
        {
            public BuildTargetGroup group;
            public Texture2D icon;

            public BuildTargetInfo(BuildTargetGroup group, Texture2D icon)
            {
                this.group = group;
                this.icon = icon;
            }
        }

        public class Styles
        {
            public GUIStyle Button;
            public GUIStyle ButtonLeft;
            public GUIStyle ButtonMid;
            public GUIStyle ButtonRight;
            public GUIStyle DropDown;
            public GUIStyle DropDown2;
            public GUIStyle DropDownButton;

            public GUIStyle Button2;

            public Texture2D IconSceneAsset;
            public Texture2D IconRefresh;
            public Texture2D IconSettings;
            public Texture2D IconCS;
            public Texture2D IconAssetStore;
            //public Texture2D IconSceneViewFx;

            public float IconButtonSize;

            public void LoadProjectIcon()
            {
                var ico = GUIDUtils.LoadAssetAtGUID<Texture2D>(Pref.i.iconOpenCSProject);
                IconCS = ico ?? Icon.Get("dll Script Icon");
            }

            public Styles()
            {
                IconButtonSize = 30;
#if UNITY_2019_3_OR_NEWER
				IconButtonSize = 32;
#endif
                IconSceneAsset = Icon.Get("SceneAsset Icon");
                IconRefresh = Icon.Get("Refresh");

#if UNITY_2018_3_OR_NEWER
                IconSettings = Icon.Get("Settings");
#else
                IconSettings = Icon.Get("SettingsIcon");
#endif
                LoadProjectIcon();
                //IconSceneViewFx = Shared.Icon.Get( "SceneViewFx" );
                IconAssetStore = Icon.Get("Asset Store");

                ButtonLeft = new GUIStyle("ButtonLeft");
                var r = new RectOffset(6, 6, 0, 0);
                ButtonLeft.padding = r;

                ButtonMid = new GUIStyle("ButtonMid");
                ButtonMid.padding = r;

                ButtonRight = new GUIStyle("ButtonRight");
                ButtonRight.padding = r;

#if UNITY_2019_3_OR_NEWER
                Button = new GUIStyle("AppCommand");
                Button.margin = new RectOffset(3, 3, 2, 2);
                Button.imagePosition = ImagePosition.ImageLeft;
                Button.fixedWidth = 0;
#else
                Button = new GUIStyle("button");
                Button.fixedHeight = 18;
#endif
                Button.padding = r;
                Button.alignment = TextAnchor.MiddleCenter;

#if UNITY_2019_3_OR_NEWER
                Button2 = new GUIStyle("AppCommand");
                Button2.margin = new RectOffset(3, 3, 2, 2);
                Button2.imagePosition = ImagePosition.ImageLeft;
                Button2.fixedWidth = 0;
                Button2.padding = new RectOffset(4, 4, 3, 3);
#else
                Button2 = new GUIStyle("button");
                Button2.fixedHeight = 18;
                Button2.padding = EditorStyles.label.padding;
#endif

                //Button2.margin = EditorStyles.label.margin;

                DropDown = new GUIStyle("DropDown");
                //Button.padding = new RectOffset( 6, 6, 1, 1 );
                DropDown.alignment = TextAnchor.MiddleCenter;
                //Button.lineHeight = ButtonRight.lineHeight;

                DropDown2 = new GUIStyle("DropDown");
                DropDown2.padding = new RectOffset(DropDown2.padding.left, DropDown2.padding.right, 0, 0);
                DropDown2.alignment = TextAnchor.MiddleCenter;

#if UNITY_2019_3_OR_NEWER
                DropDownButton = new GUIStyle("DropDown");
                //DropDownButton.padding = new RectOffset(DropDownButton.padding.left, DropDownButton.padding.right, 0, 0);
#else
                DropDownButton = new GUIStyle("DropDownButton");
                DropDownButton.padding = new RectOffset(6, DropDownButton.padding.right, 2, 2);
                DropDownButton.fixedHeight = 18;
#endif
            }
        }

        public static Styles s_styles;

        private const float SPACE = 8;
        private static BuildTargetInfo[] s_buildTargetInfo;

        private static List<Action> addon;

        static EditorToolbar()
        {
            Pref.Load();

            List<BuildTargetInfo> lst = new List<BuildTargetInfo>();
            lst.Add(new BuildTargetInfo(BuildTargetGroup.Standalone, Icon.Get("BuildSettings.Standalone")));
            lst.Add(new BuildTargetInfo(BuildTargetGroup.WebGL, Icon.Get("BuildSettings.WebGL")));
            lst.Add(new BuildTargetInfo(BuildTargetGroup.iOS, Icon.Get("BuildSettings.iPhone")));
            lst.Add(new BuildTargetInfo(BuildTargetGroup.tvOS, Icon.Get("BuildSettings.tvOS")));
            lst.Add(new BuildTargetInfo(BuildTargetGroup.Android, Icon.Get("BuildSettings.Android")));
            lst.Add(new BuildTargetInfo(BuildTargetGroup.XboxOne, Icon.Get("BuildSettings.XboxOne")));
            lst.Add(new BuildTargetInfo(BuildTargetGroup.PS4, Icon.Get("BuildSettings.PS4")));
            lst.Add(new BuildTargetInfo(BuildTargetGroup.WSA, Icon.Get("BuildSettings.Metro")));
#if UNITY_2019_3_OR_NEWER
#else
            lst.Add(new BuildTargetInfo(BuildTargetGroup.Facebook, Icon.Get("BuildSettings.Facebook")));
#endif
            s_buildTargetInfo = lst.ToArray();

            ToolbarExtender.LeftToolbarGUI.Add(OnLeftToolbarGUI);
            ToolbarExtender.RightToolbarGUI.Add(OnRightToolbarGUI);

            MakeMenuCommand();
        }

        public static void Repaint()
        {
            ToolbarCallback.Repaint();
        }

        public static void MakeMenuCommand()
        {
            addon = new List<Action>();
            foreach (var p in Settings.i.reg)
            {
                var lst = R.Methods(typeof(EditorToolbarMethod), p.className, p.assemblyName);
                foreach (var pp in lst)
                {
                    addon.Add((Action)Delegate.CreateDelegate(typeof(Action), null, pp));
                }
            }
            Repaint();
        }

        private static void OnLeftToolbarGUI()
        {
            if (s_styles == null) { s_styles = new Styles(); }

#if UNITY_2019_3_OR_NEWER
#else
            GUILayout.BeginVertical();
            GUILayout.Space(2);
            GUILayout.BeginHorizontal();
#endif

            GUILayout.FlexibleSpace();

            CreateSceneMenus();

            GUILayout.Space(SPACE);

            CreatePlatformMenus();

            CreateSettingsMenus();

            GUILayout.Space(SPACE);

            if (addon != null)
            {
                foreach (var p in addon)
                {
                    p.Invoke();
                }
            }

#if UNITY_2019_3_OR_NEWER
			GUILayout.Space(10);
#else
            GUILayout.Space(20);
#endif

#if UNITY_2019_3_OR_NEWER
#else
            GUILayout.EndHorizontal();
            GUILayout.Space(2);
            GUILayout.EndVertical();
#endif
        }

        private static void OnRightToolbarGUI()
        {
            if (s_styles == null) { s_styles = new Styles(); }

#if UNITY_2019_3_OR_NEWER
#else
            GUILayout.BeginVertical();
            GUILayout.Space(2);
            GUILayout.BeginHorizontal();
#endif

            CreateOpenCSProjectMenus();

            GUILayout.Space(SPACE);

            CreateScreenShotMenus();

            GUILayout.Space(SPACE);

            CreateAssetStoreMenus();

            GUILayout.Space(SPACE);

            Time.timeScale = EditorGUILayout.Slider("", Time.timeScale, 0, 1, GUILayout.Width(120));

            GUILayout.FlexibleSpace();
#if UNITY_2019_3_OR_NEWER
#else
            GUILayout.EndHorizontal();
            GUILayout.Space(2);
            GUILayout.EndVertical();
#endif
        }

        private static void CreateSceneMenus()
        {
            GUILayout.Label(EditorHelper.TempContent(SceneManager.GetActiveScene().name, s_styles.IconSceneAsset), s_styles.DropDown, GUILayout.Width(150));
            if (EditorHelper.HasMouseClick(GUILayoutUtility.GetLastRect()))
            {
                var m = new GenericMenu();
                var lst = EditorHelper.GetBuildSceneNames();
                if (0 < lst.Length)
                {
                    foreach (var path in lst)
                    {
                        if (File.Exists(path))
                        {
                            m.AddItem(path.FileNameWithoutExtension(), false, obj => EditorHelper.OpenScene(obj.ToCast<string>()), path);
                        }
                        else
                        {
                            m.AddDisabledItem($"{path.FileNameWithoutExtension()} : {S._Filenotfound}");
                        }
                    }
                }
                else
                {
                    m.AddDisabledItem(S._Therearenoscenesregisteredinthebuildsettings);
                }
                m.DropDown(EditorHelper.PopupRect(GUILayoutUtility.GetLastRect()));
                Event.current.Use();
            }
            CreateSceneAddButton();
            if (GUILayout.Button(EditorHelper.TempContent(s_styles.IconRefresh, S._ReloadScene), s_styles.Button, GUILayout.Width(30)))
            {
                if (Application.isPlaying)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                else
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(SceneManager.GetActiveScene().path);
                    }
                }
            }
        }

        private static void CreateSceneAddButton()
        {
            if (GUILayout.Button(EditorHelper.TempContent(EditorGUIUtility.IconContent("Toolbar Plus More").image, "Auto add all scene to Build Settings"), s_styles.Button, GUILayout.Width(30)))
            {
                string[] files = Directory.GetFiles(Application.dataPath, "*.unity", SearchOption.AllDirectories);
                List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();
                foreach (var file in files)
                {
                    if (string.IsNullOrEmpty(file)) { continue; }
                    scenes.Add(new EditorBuildSettingsScene(file.Replace(Application.dataPath, "Assets"), true));
                }
                EditorBuildSettings.scenes = scenes.ToArray();
                AssetDatabase.SaveAssets();
            }
        }

        private static void CreatePlatformMenus()
        {
            var cont = EditorHelper.TempContent(Array.Find(s_buildTargetInfo, x => x.group == UEditorUserBuildSettings.activeBuildTargetGroup).icon, S._OpenBuildSettings);

            Rect r = GUILayoutUtility.GetRect(cont, s_styles.DropDownButton, GUILayout.Width(50));
            Rect rr = r;
            rr.width = 20;
            rr.x += 30;

            if (EditorHelper.HasMouseClick(rr))
            {
                var m = new GenericMenu();
                m.AddDisabledItem("SwitchPlatform");
                m.AddSeparator("");
                foreach (var e in s_buildTargetInfo)
                {
                    m.AddItem(e.group.ToString(), UEditorUserBuildSettings.activeBuildTargetGroup == e.group, CallbackEventOnSwitchPlatform, e.group);
                }
                m.DropDown(EditorHelper.PopupRect(r));
                Event.current.Use();
            }
            if (GUI.Button(r, cont, s_styles.DropDownButton))
            {
                UnityEditorMenu.File_Build_Settings();
            }

#if UNITY_2019_3_OR_NEWER // DopesheetBackground
			//EditorGUI.DrawRect( rr, new Color(0,0,1,0.2f));
			rr.y += 3;
			rr.height -= 6;
			GUI.Label( rr, GUIContent.none, "DopesheetBackground" );
#endif
        }

        public static void CallbackEventOnSwitchPlatform(object userData)
        {
            BuildTargetGroup group = (BuildTargetGroup)userData;
            EditorHelper.SwitchActiveBuildTarget(group);
        }

        private static void CreateSettingsMenus()
        {
            var cont = EditorHelper.TempContent(s_styles.IconSettings, S._OpenSettings);
            var ssr = GUILayoutUtility.GetRect(cont, s_styles.DropDown2);
            ssr.width = 40;

            if (EditorHelper.HasMouseClick(GUILayoutUtility.GetLastRect()))
            {
                var m = new GenericMenu();
                m.AddItem(S._Preferences, false, () => UnityEditorMenu.Edit_Preferences());
#if UNITY_2018_3_OR_NEWER
                m.AddItem(S._ProjectSettings, false, () => UnityEditorMenu.Edit_Project_Settings());
#else
                m.AddSeparator("");
                m.AddItem(new GUIContent(S._Input), false, () => UnityEditorMenu.Edit_Project_Settings_Input());
                m.AddItem(new GUIContent(S._TagsandLayers), false, () => UnityEditorMenu.Edit_Project_Settings_Tags_and_Layers());
                m.AddItem(new GUIContent(S._Audio), false, () => UnityEditorMenu.Edit_Project_Settings_Audio());
                m.AddItem(new GUIContent(S._Time), false, () => UnityEditorMenu.Edit_Project_Settings_Time());
                m.AddItem(new GUIContent(S._Player), false, () => UnityEditorMenu.Edit_Project_Settings_Player());
                m.AddItem(new GUIContent(S._Physics), false, () => UnityEditorMenu.Edit_Project_Settings_Physics());
                m.AddItem(new GUIContent(S._Physics2D), false, () => UnityEditorMenu.Edit_Project_Settings_Physics_2D());
                m.AddItem(new GUIContent(S._Quality), false, () => UnityEditorMenu.Edit_Project_Settings_Quality());
                m.AddItem(new GUIContent(S._Graphics), false, () => UnityEditorMenu.Edit_Project_Settings_Graphics());
                m.AddItem(new GUIContent(S._Network), false, () => UnityEditorMenu.Edit_Project_Settings_Network());
                m.AddItem(new GUIContent(S._Editor), false, () => UnityEditorMenu.Edit_Project_Settings_Editor());
                m.AddItem(new GUIContent(S._ScriptExecutionOrder), false, () => UnityEditorMenu.Edit_Project_Settings_Script_Execution_Order());
#endif
                m.DropDown(EditorHelper.PopupRect(GUILayoutUtility.GetLastRect()));
                Event.current.Use();
            }

            GUI.Button(ssr, cont, s_styles.DropDown2);
        }

        private static void CreateOpenCSProjectMenus()
        {
            if (GUILayout.Button(EditorHelper.TempContent(s_styles.IconCS, S._OpenCSharpProject), s_styles.Button2, GUILayout.Width(s_styles.IconButtonSize)))
            {
                EditorApplication.ExecuteMenuItem("Assets/Open C# Project");
            }
        }

        private static void CreateScreenShotMenus()
        {
            var cont = EditorHelper.TempContent(Hananoki.Shared.Icon.Get("SceneViewFx"), S._GameViewScreenShot);
            Rect r = GUILayoutUtility.GetRect(cont, s_styles.DropDownButton, GUILayout.Width(50));

            Rect rr = r;
            rr.width = 20;
            rr.x += 30;
            if (EditorHelper.HasMouseClick(rr))
            {
                var m = new GenericMenu();
                var dname = Directory.GetCurrentDirectory() + "/ScreenShot";
                if (Directory.Exists(dname))
                {
                    m.AddItem(S._OpenOutputFolder, false, () =>
                    {
                        EditorUtils.ShellOpenDirectory(dname);
                    });
                }
                else
                {
                    m.AddDisabledItem(S._OpenOutputFolder);
                }
                m.DropDown(EditorHelper.PopupRect(r));
                Event.current.Use();
            }

            if (GUI.Button(r, cont, s_styles.DropDownButton))
            {
                EditorUtils.SaveScreenCapture();
            }

#if UNITY_2019_3_OR_NEWER
			rr.y += 3;
			rr.height -= 6;
			GUI.Label( rr, GUIContent.none, "DopesheetBackground" );
#endif
        }

        private static void CreateAssetStoreMenus()
        {
            if (GUILayout.Button(EditorHelper.TempContent(s_styles.IconAssetStore, S._OpenAssetStore), s_styles.Button, GUILayout.Width(s_styles.IconButtonSize)))
            {
                EditorUtils.AssetStoreWindow();
            }
        }
    }
}