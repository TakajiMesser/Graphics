using System;

namespace SpiceEngineCore.Helpers
{
    /// <summary>
    /// For now, this is a helper class for accessing hard-coded paths for game files
    /// </summary>
    public static class FilePathHelper
    {
        private static readonly string SOLUTION_DIRECTORY = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\..";

        public static string SCREENSHOT_PATH = SOLUTION_DIRECTORY + @"\SampleGameProject\Resources\Screenshots\";

        #region Meshes
        public static string SQUARE_MESH_PATH = SOLUTION_DIRECTORY + @"\SpiceEngine\Resources\Meshes\Square.obj";
        public static string CUBE_MESH_PATH = SOLUTION_DIRECTORY + @"\SpiceEngine\Resources\Meshes\Cube.obj";
        public static string SPHERE_MESH_PATH = SOLUTION_DIRECTORY + @"\SpiceEngine\Resources\Meshes\Sphere.obj";
        public static string CONE_MESH_PATH = SOLUTION_DIRECTORY + @"\SpiceEngine\Resources\Meshes\Cone.obj";
        public static string SPHERE_OUTPUT_PATH = SOLUTION_DIRECTORY + @"\SpiceEngine\Resources\Meshes\SphereOutput.txt";
        #endregion

        #region Materials
        public static string GENERIC_MATERIAL_PATH = SOLUTION_DIRECTORY + @"\SpiceEngine\Resources\Meshes\GenericMaterial.mtl";
        #endregion
    }
}
