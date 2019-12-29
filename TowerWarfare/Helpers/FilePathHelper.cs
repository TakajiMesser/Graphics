using System;

namespace TowerWarfare.Helpers
{
    /// <summary>
    /// For now, this is a helper class for accessing hard-coded paths for game files
    /// </summary>
    public static class FilePathHelper
    {
        private static readonly string SOLUTION_DIRECTORY = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..";

        #region Projects
        public static string PROJECT_PATH = SOLUTION_DIRECTORY + @"\TowerWarfare\Resources\Projects\TestProject.pro";
        #endregion

        #region Maps
        public static string MAP_PATH = SOLUTION_DIRECTORY + @"\TowerWarfare\Resources\Maps\TestMap.map";
        #endregion

        #region Animations
        #endregion

        #region Meshes
        #endregion

        #region Materials
        public static string SHINY_MATERIAL_PATH = SOLUTION_DIRECTORY + @"\TowerWarfare\Resources\Meshes\ShinyMaterial.mtl";
        #endregion

        #region Fonts
        public static string ROBOTO_BLACK_FONT_PATH = SOLUTION_DIRECTORY + @"\TowerWarfare\Resources\Fonts\Roboto-Black.ttf";
        public static string ROBOTO_BLACK_ITALIC_FONT_PATH = SOLUTION_DIRECTORY + @"\TowerWarfare\Resources\Fonts\Roboto-BlackItalic.ttf";
        public static string ROBOTO_BOLD_FONT_PATH = SOLUTION_DIRECTORY + @"\TowerWarfare\Resources\Fonts\Roboto-Bold.ttf";
        public static string ROBOTO_BOLD_ITALIC_FONT_PATH = SOLUTION_DIRECTORY + @"\TowerWarfare\Resources\Fonts\Roboto-BoldItalic.ttf";
        public static string ROBOTO_ITALIC_FONT_PATH = SOLUTION_DIRECTORY + @"\TowerWarfare\Resources\Fonts\Roboto-Italic.ttf";
        public static string ROBOTO_LIGHT_FONT_PATH = SOLUTION_DIRECTORY + @"\TowerWarfare\Resources\Fonts\Roboto-Light.ttf";
        public static string ROBOTO_LIGHT_ITALIC_FONT_PATH = SOLUTION_DIRECTORY + @"\TowerWarfare\Resources\Fonts\Roboto-LightItalic.ttf";
        public static string ROBOTO_MEDIUM_FONT_PATH = SOLUTION_DIRECTORY + @"\TowerWarfare\Resources\Fonts\Roboto-Medium.ttf";
        public static string ROBOTO_MEDIUM_ITALIC_FONT_PATH = SOLUTION_DIRECTORY + @"\TowerWarfare\Resources\Fonts\Roboto-MediumItalic.ttf";
        public static string ROBOTO_REGULAR_FONT_PATH = SOLUTION_DIRECTORY + @"\TowerWarfare\Resources\Fonts\Roboto-Regular.ttf";
        public static string ROBOTO_THIN_FONT_PATH = SOLUTION_DIRECTORY + @"\TowerWarfare\Resources\Fonts\Roboto-Thin.ttf";
        public static string ROBOTO_THIN_ITALIC_FONT_PATH = SOLUTION_DIRECTORY + @"\TowerWarfare\Resources\Fonts\Roboto-ThinItalic.ttf";
        #endregion

        #region Textures
        #endregion

        #region Behaviors
        public static string CAMERA_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\TowerWarfare\Resources\Behaviors\Camera.btt";
        public static string CAMERA_NODE_PATH = SOLUTION_DIRECTORY + @"\TowerWarfare\Resources\Behaviors\Nodes\CameraNode.cs";
        #endregion
    }
}
