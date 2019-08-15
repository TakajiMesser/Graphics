using OpenTK;
using SpiceEngine.Rendering.Materials;
using SpiceEngine.Rendering.Matrices;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using System;

namespace SpiceEngine.Entities.Brushes
{
    /// <summary>
    /// Brushes are static geometric shapes that are baked into a scene.
    /// Unlike meshes, brushes cannot be deformed.
    /// </summary>
    public class Brush : TexturedEntity, IRotate, IScale, ITextureBinder
    {
        private Material _material;
        private TextureMapping? _textureMapping;

        public Quaternion Rotation
        {
            get => _modelMatrix.Rotation;
            set
            {
                _modelMatrix.Rotation = value;
                //Transformed?.Invoke(this, new EntityTransformEventArgs(ID, _modelMatrix.Matrix));
            }
        }

        public Vector3 Scale
        {
            get => _modelMatrix.Scale;
            set
            {
                _modelMatrix.Scale = value;
                //Transformed?.Invoke(this, new EntityTransformEventArgs(ID, _modelMatrix.Matrix));
            }
        }

        public void Rotate(Quaternion rotation)
        {
            _modelMatrix.Rotation *= rotation;
            // Call Transformed
        }

        public void ScaleBy(Vector3 scale)
        {
            _modelMatrix.Scale *= scale;
            // Call Transformed
        }

        //public List<Vector3> Vertices => Mesh.Vertices.Select(v => v.Position).Distinct().ToList();
        //public Matrix4 GetModelMatrix() => _modelMatrix.Matrix;

        public override Material Material => _material;
        public override TextureMapping? TextureMapping => _textureMapping;

        public override void AddMaterial(Material material) => _material = material;
        public override void AddTextureMapping(TextureMapping? textureMapping) => _textureMapping = textureMapping;

        public override void SetUniforms(ShaderProgram program)
        {
            base.SetUniforms(program);
            program.SetUniform(ModelMatrix.NAME, Matrix4.Identity);
            program.SetUniform(ModelMatrix.PREVIOUS_NAME, Matrix4.Identity);
        }

        public Brush Duplicate()
        {
            var brush = new Brush()
            {
                Position = Position,
                Rotation = Rotation,
                Scale = Scale,
                _material = Material
            };

            if (TextureMapping.HasValue)
            {
                var textureMapping = TextureMapping.Value;
                brush.AddTextureMapping(new TextureMapping(textureMapping.DiffuseMapID, textureMapping.NormalMapID, textureMapping.ParallaxMapID, textureMapping.SpecularMapID));
            }

            return brush;
        }
    }
}
