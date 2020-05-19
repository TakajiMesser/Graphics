using System;

namespace SampleGameProject.Helpers
{
    /// <summary>
    /// For now, this is a helper class for accessing hard-coded paths for game files
    /// </summary>
    public static class FilePathHelper
    {
        private static readonly string SOLUTION_DIRECTORY = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..";

        #region Projects
        public static string PROJECT_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Projects\TestProject.pro";
        #endregion

        #region Maps
        public static string MAP_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Maps\TestMap.map";
        #endregion

        #region Animations
        public static string BOB_LAMP_MESH_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Animations\boblampclean.md5mesh";
        public static string BOB_LAMP_ANIMATION_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Animations\boblampclean.md5anim";
        #endregion

        #region Meshes
        public static string PLAYER_MESH_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Meshes\Player.obj";
        public static string FLOOR_MESH_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Meshes\Floor.obj";
        public static string ENEMY_MESH_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Meshes\Enemy.obj";
        #endregion

        #region Materials
        public static string SHINY_MATERIAL_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Meshes\ShinyMaterial.mtl";
        #endregion

        #region Fonts
        public static string ROBOTO_BLACK_FONT_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Fonts\Roboto-Black.ttf";
        public static string ROBOTO_BLACK_ITALIC_FONT_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Fonts\Roboto-BlackItalic.ttf";
        public static string ROBOTO_BOLD_FONT_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Fonts\Roboto-Bold.ttf";
        public static string ROBOTO_BOLD_ITALIC_FONT_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Fonts\Roboto-BoldItalic.ttf";
        public static string ROBOTO_ITALIC_FONT_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Fonts\Roboto-Italic.ttf";
        public static string ROBOTO_LIGHT_FONT_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Fonts\Roboto-Light.ttf";
        public static string ROBOTO_LIGHT_ITALIC_FONT_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Fonts\Roboto-LightItalic.ttf";
        public static string ROBOTO_MEDIUM_FONT_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Fonts\Roboto-Medium.ttf";
        public static string ROBOTO_MEDIUM_ITALIC_FONT_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Fonts\Roboto-MediumItalic.ttf";
        public static string ROBOTO_REGULAR_FONT_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Fonts\Roboto-Regular.ttf";
        public static string ROBOTO_THIN_FONT_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Fonts\Roboto-Thin.ttf";
        public static string ROBOTO_THIN_ITALIC_FONT_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Fonts\Roboto-ThinItalic.ttf";
        #endregion

        #region Textures
        public static string BRICK_01_D_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Textures\brick1-d.jpg";
        public static string BRICK_01_H_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Textures\brick1-h.jpg";
        public static string BRICK_01_N_NORMAL_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Textures\brick1-n.jpg";
        public static string BRICK_01_S_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Textures\brick1-s.jpg";

        public static string BRICK_02_D_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Textures\brick2-d.jpg";
        public static string BRICK_02_H_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Textures\brick2-h.jpg";
        public static string BRICK_02_N_NORMAL_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Textures\brick2-n.jpg";
        public static string BRICK_02_S_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Textures\brick2-s.jpg";
        public static string BRICK_02_B_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Textures\brick2-b.jpg";

        public static string GRASS_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Textures\Grass01.png";
        public static string GRASS_N_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Textures\Grass01_normal.png";

        public static string SPACE_01_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Textures\Skybox\space_right1.png";
        public static string SPACE_02_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Textures\Skybox\space_left2.png";
        public static string SPACE_03_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Textures\Skybox\space_top3.png";
        public static string SPACE_04_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Textures\Skybox\space_bottom4.png";
        public static string SPACE_05_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Textures\Skybox\space_front5.png";
        public static string SPACE_06_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Textures\Skybox\space_back6.png";
        #endregion

        #region Behaviors
        public static string CAMERA_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Behaviors\Camera.btt";

        public static string ENEMY_PATROL_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Behaviors\EnemyPatrol.btt";
        public static string ENEMY_SEARCH_PLAYER_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Behaviors\SearchForPlayer.btt";
        public static string ENEMY_TURN_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Behaviors\EnemyTurn.btt";

        public static string PLAYER_INPUT_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Behaviors\PlayerInput.btt";
        public static string PLAYER_MOVEMENT_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Behaviors\PlayerMovement.btt";
        public static string PLAYER_TURN_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Behaviors\PlayerTurn.btt";
        public static string PLAYER_EVADE_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Behaviors\PlayerEvade.btt";
        public static string PLAYER_COVER_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Behaviors\PlayerCover.btt";

        public static string CAMERA_NODE_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Behaviors\Nodes\CameraNode.cs";
        public static string BLOCK_NODE_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Behaviors\Nodes\BlockNode.cs";
        public static string MOVE_NODE_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Behaviors\Nodes\MoveNode.cs";
        public static string MOVE_TO_NODE_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Behaviors\Nodes\MoveToNode.cs";
        public static string TURN_NODE_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Behaviors\Nodes\TurnNode.cs";
        public static string TURN_TOWARDS_NODE_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Behaviors\Nodes\TurnTowardsNode.cs";
        #endregion
    }
}
