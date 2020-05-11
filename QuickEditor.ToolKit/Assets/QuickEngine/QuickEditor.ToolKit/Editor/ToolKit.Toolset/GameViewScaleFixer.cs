using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
[ExecuteInEditMode]
internal static class GameViewScaleFixer
{
    private static readonly Assembly GAME_VIEW_ASSEMBLY = typeof(EditorWindow).Assembly;
    private static readonly Type GAME_VIEW_TYPE = GAME_VIEW_ASSEMBLY.GetType("UnityEditor.GameView");
    private static readonly BindingFlags BINDING_ATTRS = BindingFlags.Instance | BindingFlags.NonPublic;
    private static readonly Vector2 DEFAULT_SCALE = new Vector2(0.1f, 0.1f);

    private static bool m_CanUpdate;

    static GameViewScaleFixer()
    {
        EditorApplication.update += OnUpdate;
        EditorApplication.delayCall += OnDelayCall;
        //EditorApplication.playModeStateChanged += OnPlayStateChanged;
        SetGameViewScale();
    }

    private static void OnPlayStateChanged(PlayModeStateChange playModeStateChange)
    {
        SetGameViewScale();
    }

    private static void OnDelayCall()
    {
        m_CanUpdate = true;
    }

    private static void OnUpdate()
    {
        if (!m_CanUpdate) return;

        SetGameViewScale();
        m_CanUpdate = false;
    }

    private static void SetGameViewScale()
    {
        var objects = Resources.FindObjectsOfTypeAll(GAME_VIEW_TYPE);
        var gameViewWindow = objects.FirstOrDefault() as EditorWindow;

        if (gameViewWindow == null)
        {
            Debug.LogWarning("GameView is null!");
            return;
        }

        var defScaleField = GAME_VIEW_TYPE.GetField("m_defaultScale", BINDING_ATTRS);

        var zoomAreaField = GAME_VIEW_TYPE.GetField("m_ZoomArea", BINDING_ATTRS);

        if (zoomAreaField == null) return;

        var zoomArea = zoomAreaField.GetValue(gameViewWindow);
        var scaleField = zoomArea.GetType().GetField("m_Scale", BINDING_ATTRS);

        if (scaleField == null) return;

        scaleField.SetValue(zoomArea, DEFAULT_SCALE);
    }
}