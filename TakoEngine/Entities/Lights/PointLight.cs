﻿using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using TakoEngine.Outputs;
using TakoEngine.Rendering.Shaders;
using TakoEngine.Utilities;

namespace TakoEngine.Entities.Lights
{
    public class PointLight : Light
    {
        public float Radius { get; set; }

        public Matrix4 Projection => Matrix4.CreatePerspectiveFieldOfView(UnitConversions.ToRadians(90.0f), 1.0f, 0.1f, Radius);

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

        public override void DrawForStencilPass(ShaderProgram program)
        {
            var model = Matrix4.Identity * Matrix4.CreateScale(Radius) * Matrix4.CreateTranslation(Position);
            program.SetUniform("modelMatrix", model);

            program.SetUniform("lightPosition", Position);
            program.SetUniform("lightRadius", Radius);
            program.SetUniform("lightColor", Color);
            program.SetUniform("lightIntensity", Intensity);
        }

        public override void DrawForLightPass(ShaderProgram program)
        {
            var model = Matrix4.Identity * Matrix4.CreateScale(Radius) * Matrix4.CreateTranslation(Position);
            program.SetUniform("modelMatrix", model);

            program.SetUniform("lightPosition", Position);
            program.SetUniform("lightRadius", Radius);
            program.SetUniform("lightColor", Color.Xyz);
            program.SetUniform("lightIntensity", Intensity);
        }

        public PLight ToStruct() => new PLight(Position, Radius, Color.Xyz, Intensity);
    }
}
