using System;

namespace SauceEditorCore.Helpers
{
    public static class FilePathHelper
    {
        private static readonly string SOLUTION_DIRECTORY = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..";

        public static string VERTEX_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\SpiceEngine\Resources\Textures\vertex.png";
    }
}
