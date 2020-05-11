using System;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class ChangeEditorTitle
{
#if UNITY_EDITOR_WIN

    [DllImport("user32.dll", EntryPoint = "SetWindowText")]
    public static extern bool SetWindowText(System.IntPtr hwnd, System.String lpString);

    [DllImport("user32.dll")]
    private static extern System.IntPtr GetActiveWindow();

    // unity editor will auto reset editor title in many cases
    static ChangeEditorTitle()
    {
        EditorApplication.update += ChangeTitleWhenSceneOnRuntime;
    }

    private static DateTime lastTime;

    private static void ChangeTitleWhenSceneOnRuntime()
    {
        if ((DateTime.Now - lastTime).TotalSeconds > 1)
        {
            lastTime = DateTime.Now;
            ChangeTitle();
        }
    }

    private static void ChangeTitle()
    {
        IntPtr windowPtr = GetActiveWindow();
        var path = Application.dataPath.Substring(0, Application.dataPath.Length - 7);

        SetWindowText(windowPtr, string.Format("{0} [{1}]  [{2}]  [{3}]",
            path, Application.unityVersion, EditorUserBuildSettings.activeBuildTarget.ToString(), SystemInfo.graphicsDeviceType.ToString()));
    }

#endif
}