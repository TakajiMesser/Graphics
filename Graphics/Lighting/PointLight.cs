using Graphics.GameObjects;
using Graphics.Meshes;
using Graphics.Outputs;
using Graphics.Rendering.Shaders;
using Graphics.Utilities;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Lighting
{
    /// <summary>
    /// This struct is used by the Forward Renderer, in a uniform buffer
    /// </summary>
    public struct PLight
    {
        public Vector3 Position { get; private set; }
        public float Radius { get; private set; }
        public Vector3 Color { get; private set; }
        public float Intensity { get; private set; }

        public PLight(Vector3 position, float radius, Vector3 color, float intensity)
        {
            Position = position;
            Radius = radius;
            Color = color;
            Intensity = intensity;
        }
    }

    public class PointLight : Light
    {
        public Vector3 Position { get; set; }
        public float Radius { get; set; }

        public Matrix4 GetView(TextureTarget target)
        {
            switch (target)
            {
                case TextureTarget.TextureCubeMapPositiveX:
                    return Matrix4.LookAt(Position, Position + Vector3.UnitX, -Vector3.UnitY);
                case TextureTarget.TextureCubeMapNegativeX:
                    return Matrix4.LookAt(Position, Position - Vector3.UnitX, -Vector3.UnitY);
                case TextureTarget.TextureCubeMapPositiveY:
                    return Matrix4.LookAt(Position, Position + Vector3.UnitY, Vector3.UnitZ);
                case TextureTarget.TextureCubeMapNegativeY:
                    return Matrix4.LookAt(Position, Position - Vector3.UnitY, -Vector3.UnitZ);
                case TextureTarget.TextureCubeMapPositiveZ:
                    return Matrix4.LookAt(Position, Position + Vector3.UnitZ, -Vector3.UnitY);
                case TextureTarget.TextureCubeMapNegativeZ:
                    return Matrix4.LookAt(Position, Position - Vector3.UnitZ, -Vector3.UnitY);
                default:
                    throw new NotImplementedException("Could not handle target " + target);
            }
        }

        public Matrix4 GetProjection(Resolution resolution)
        {
            return Matrix4.CreatePerspectiveFieldOfView(UnitConversions.ToRadians(90.0f), 1.0f, 0.1f, Radius);
        }

        public override void DrawForStencilPass(ShaderProgram program)
        {
            var model = Matrix4.Identity * Matrix4.CreateScale(Radius) * Matrix4.CreateTranslation(Position);
            program.SetUniform("modelMatrix", model);

            program.SetUniform("lightPosition", Position);
            program.SetUniform("lightRadius", Radius);
            program.SetUniform("lightColor", Color);
            program.SetUniform("lightIntensity", Intensity);
        }

        public override void DrawForLightPass(Resolution resolution, ShaderProgram program)
        {
            var model = Matrix4.Identity * Matrix4.CreateScale(Radius) * Matrix4.CreateTranslation(Position);
            program.SetUniform("modelMatrix", model);

            program.SetUniform("lightPosition", Position);
            program.SetUniform("lightRadius", Radius);
            program.SetUniform("lightColor", Color);
            program.SetUniform("lightIntensity", Intensity);
        }

        public PLight ToStruct() => new PLight(Position, Radius, Color, Intensity);
    }
}
