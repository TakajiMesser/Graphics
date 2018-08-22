using OpenTK;
using System.Collections.Generic;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Rendering.Matrices;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Vertices;
using SpiceEngine.Rendering.Meshes;
using System.Linq;

namespace SpiceEngine.Entities.Models
{
    public interface IModel3D
    {
        List<Vector3> Vertices { get; }

        Matrix4 ModelMatrix { get; }
        Vector3 Position { get; set; }
        Quaternion Rotation { get; set; }
        Quaternion Orientation { get; set; }
        Vector3 OriginalRotation { get; set; }
        Vector3 Scale { get; set; }

        void Load();
        void Draw();
        void SetUniforms(ShaderProgram program, TextureManager textureManager);
        void SetUniformsAndDraw(ShaderProgram program, TextureManager textureManager);
    }
}
