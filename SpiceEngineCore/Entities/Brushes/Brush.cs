using OpenTK;
using SpiceEngineCore.Rendering.Materials;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Shaders;
using SpiceEngineCore.Rendering.Textures;
using SpiceEngineCore.Utilities;
using System.Collections.Generic;

namespace SpiceEngineCore.Entities.Brushes
{
    /// <summary>
    /// Brushes are static geometric shapes that are baked into a scene.
    /// Unlike meshes, brushes cannot be deformed.
    /// </summary>
    public class Brush : TexturedEntity, IBrush
    {
        private Material _material;
        private TextureMapping? _textureMapping;

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

        public override IEnumerable<Material> Materials => _material.Yield();
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
        }
    }
}
