using SauceEditor.Models;
using System;

namespace SauceEditor.Helpers
{
    public static class FilePathHelper
    {
        internal static readonly string SOLUTION_DIRECTORY = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..";

        public static string SETTINGS_PATH = SOLUTION_DIRECTORY + @"\SauceEditor\EditorSettings" + EditorSettings.FILE_EXTENSION;
        public static string INITIAL_FILE_DIRECTORY = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Maps";

        public static string RESOURCES_DIRECTORY = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources";

        public static string INITIAL_PROJECT_DIRECTORY = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Projects";
        public static string INITIAL_MAP_DIRECTORY = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Maps";
        public static string INITIAL_MODEL_DIRECTORY = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Models";
        public static string INITIAL_BEHAVIOR_DIRECTORY = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Behaviors";
        public static string INITIAL_TEXTURE_DIRECTORY = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Textures";
        public static string INITIAL_SOUND_DIRECTORY = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Sounds";
        public static string INITIAL_MATERIAL_DIRECTORY = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Materials";
        public static string INITIAL_ARCHETYPE_DIRECTORY = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Archetypes";
        public static string INITIAL_SCRIPT_DIRECTORY = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Scripts";
    }
}
