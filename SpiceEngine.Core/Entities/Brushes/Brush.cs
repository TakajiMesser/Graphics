﻿using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SpiceEngineCore.Entities.Brushes
{
    /// <summary>
    /// Brushes are static geometric shapes that are baked into a scene.
    /// Unlike meshes, brushes cannot be deformed.
    /// </summary>
    public class Brush : Entity, IBrush
    {
        //private Material _material;
        //private TextureMapping? _textureMapping;

        public Quaternion Rotation
        {
            get => _modelMatrix.Rotation;
            set => _modelMatrix.Rotation = value;
        }

        public Vector3 Scale
        {
            get => _modelMatrix.Scale;
            set => _modelMatrix.Scale = value;
        }

        /*public override IEnumerable<Material> Materials => _material.Yield();
        public override IEnumerable<TextureMapping?> TextureMappings => _textureMapping.Yield();

        public override Material CurrentMaterial => _material;
        public override TextureMapping? CurrentTextureMapping => _textureMapping;

        public override void AddMaterial(Material material) => _material = material;
        public override void AddTextureMapping(TextureMapping? textureMapping) => _textureMapping = textureMapping;

        public override void SetUniforms(ShaderProgram program)
        {
            base.SetUniforms(program);
            program.SetUniform(ModelMatrix.NAME, Matrix4.Identity);
            program.SetUniform(ModelMatrix.PREVIOUS_NAME, Matrix4.Identity);
        }*/
    }
}