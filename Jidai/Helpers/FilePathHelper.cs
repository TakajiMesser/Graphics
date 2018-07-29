using System;

namespace Jidai.Helpers
{
    /// <summary>
    /// For now, this is a helper class for accessing hard-coded paths for game files
    /// </summary>
    public static class FilePathHelper
    {
        private static string SOLUTION_DIRECTORY = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..";//@"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\SpiceEngine";

        #region Maps
        public static string MAP_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Maps\TestMap.map";
        #endregion

        #region Animations
        public static string BOB_LAMP_MESH_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Animations\boblampclean.md5mesh";
        public static string BOB_LAMP_ANIMATION_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Animations\boblampclean.md5anim";
        #endregion

        #region Meshes
        public static string PLAYER_MESH_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Meshes\Player.obj";
        public static string FLOOR_MESH_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Meshes\Floor.obj";
        public static string ENEMY_MESH_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Meshes\Enemy.obj";
        #endregion

        #region Materials
        public static string SHINY_MATERIAL_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Meshes\ShinyMaterial.mtl";
        #endregion

        #region Fonts
        public static string ROBOTO_BLACK_FONT_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Fonts\Roboto-Black.ttf";
        public static string ROBOTO_BLACK_ITALIC_FONT_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Fonts\Roboto-BlackItalic.ttf";
        public static string ROBOTO_BOLD_FONT_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Fonts\Roboto-Bold.ttf";
        public static string ROBOTO_BOLD_ITALIC_FONT_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Fonts\Roboto-BoldItalic.ttf";
        public static string ROBOTO_ITALIC_FONT_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Fonts\Roboto-Italic.ttf";
        public static string ROBOTO_LIGHT_FONT_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Fonts\Roboto-Light.ttf";
        public static string ROBOTO_LIGHT_ITALIC_FONT_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Fonts\Roboto-LightItalic.ttf";
        public static string ROBOTO_MEDIUM_FONT_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Fonts\Roboto-Medium.ttf";
        public static string ROBOTO_MEDIUM_ITALIC_FONT_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Fonts\Roboto-MediumItalic.ttf";
        public static string ROBOTO_REGULAR_FONT_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Fonts\Roboto-Regular.ttf";
        public static string ROBOTO_THIN_FONT_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Fonts\Roboto-Thin.ttf";
        public static string ROBOTO_THIN_ITALIC_FONT_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Fonts\Roboto-ThinItalic.ttf";
        #endregion

        #region Textures
        public static string BRICK_01_D_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Textures\brick1-d.jpg";
        public static string BRICK_01_H_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Textures\brick1-h.jpg";
        public static string BRICK_01_N_NORMAL_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Textures\brick1-n.jpg";
        public static string BRICK_01_S_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Textures\brick1-s.jpg";

        public static string BRICK_02_D_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Textures\brick2-d.jpg";
        public static string BRICK_02_H_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Textures\brick2-h.jpg";
        public static string BRICK_02_N_NORMAL_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Textures\brick2-n.jpg";
        public static string BRICK_02_S_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Textures\brick2-s.jpg";
        public static string BRICK_02_B_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Textures\brick2-b.jpg";

        public static string GRASS_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Textures\Grass01.png";
        public static string GRASS_N_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Textures\Grass01_normal.png";

        public static string SPACE_01_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Textures\Skybox\space_right1.png";
        public static string SPACE_02_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Textures\Skybox\space_left2.png";
        public static string SPACE_03_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Textures\Skybox\space_top3.png";
        public static string SPACE_04_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Textures\Skybox\space_bottom4.png";
        public static string SPACE_05_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Textures\Skybox\space_front5.png";
        public static string SPACE_06_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Textures\Skybox\space_back6.png";
        #endregion

        #region Behaviors
        public static string ENEMY_PATROL_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Behaviors\EnemyPatrol.btt";
        public static string ENEMY_SEARCH_PLAYER_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Behaviors\SearchForPlayer.btt";
        public static string ENEMY_TURN_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Behaviors\EnemyTurn.btt";

        public static string PLAYER_INPUT_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Behaviors\PlayerInput.btt";
        public static string PLAYER_MOVEMENT_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Behaviors\PlayerMovement.btt";
        public static string PLAYER_TURN_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Behaviors\PlayerTurn.btt";
        public static string PLAYER_EVADE_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Behaviors\PlayerEvade.btt";
        public static string PLAYER_COVER_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\Jidai\Resources\Behaviors\PlayerCover.btt";
        #endregion
    }
}
