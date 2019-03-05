using OpenTK;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using System.Collections.Generic;

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
