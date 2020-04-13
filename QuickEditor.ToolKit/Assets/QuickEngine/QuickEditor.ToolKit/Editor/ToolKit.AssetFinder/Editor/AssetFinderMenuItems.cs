namespace QuickEditor.ToolKit
{
    using UnityEditor;

    public class AssetFinderMenuItems
    {
        private const string ToolsMenuEntry = "Tools/QuickEditor.ToolKit/Asset Finder/";
        private const int MenuEntryPriority = 1000;

        [MenuItem(ToolsMenuEntry + "Asset Finder", false, priority = MenuEntryPriority)]
        public static void Tests()
        {
        }
    }
}