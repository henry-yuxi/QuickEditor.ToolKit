namespace QuickEditor.ToolKit.SVN
{
    using QuickEditor.Common;
    using UnityEditor;

    public class SVNControlSettings : QuickScriptableObject<SVNControlSettings>
    {
        public string externalVersionControl = "Visible Meta Files";
        public SerializationMode serializationMode = SerializationMode.ForceText;
        public string SVNRemoteUrl;
        public string SVNProjectPath;

        public void ApplySettings()
        {
            EditorSettings.externalVersionControl = externalVersionControl;
            EditorSettings.serializationMode = serializationMode;
        }
    }
}