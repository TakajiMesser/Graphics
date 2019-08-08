using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Entities.Selection;
using SpiceEngine.Rendering.Materials;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.PostProcessing;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Utilities;
using System;

namespace SauceEditorCore.Models.Entities
{
    public abstract class TexturedModelEntity<T> : ModelEntity<T>, IRotate, IScale, ITextureBinder, ITexturePath, ITexturedEntity, IDirectional where T : IModelShape, ITexturedShape
    {
        private Vector2 _texturePosition;
        private float _textureRotation;
        private Vector2 _textureScale;

        public bool IsInTextureMode { get; set; }

        /*public override Vector3 Position
        {
            get => base.Position;
            set
            {
                var translation = value - Position;
                if (translation.IsSignificant())
                {
                    ModelShape.Translate(translation);
                }

                base.Position = value;
            }
        }*/

        public Quaternion Rotation
        {
            // TODO - Determine if quaternion multiplication order matters here
            get => _modelMatrix.Rotation;
            set
            {
                var rotationChange = value * _modelMatrix.Rotation.Inverted();
                _modelMatrix.Rotation = value;

                if (rotationChange.IsSignificant())
                {
                    ModelShape.Rotate(rotationChange);
                    var transform = Matrix4.CreateTranslation(-Position) * Matrix4.CreateFromQuaternion(rotationChange) * Matrix4.CreateTranslation(Position);
                    OnTransformed(this, new EntityTransformEventArgs(ID, transform));
                }
            }
        }

        public Vector3 Scale
        {
            get => _modelMatrix.Scale;
            set
            {
                var scaleChange = value - _modelMatrix.Scale;
                _modelMatrix.Scale = value;

                if (scaleChange.IsSignificant())
                {
                    OnTransformed(this, new EntityTransformEventArgs(ID, Matrix4.CreateScale(scaleChange)));
                    //Transformed?.Invoke(this, new EntityTransformEventArgs(ID, Matrix4.CreateScale(scaleChange)));
                }
            }
        }

        public TexturePaths TexturePaths { get; }
        public Material Material { get; private set; }
        public TextureMapping? TextureMapping { get; private set; }

        public abstract Vector3 XDirection { get; }
        public abstract Vector3 YDirection { get; }
        public abstract Vector3 ZDirection { get; }

        //public override event EventHandler<EntityTransformEventArgs> Transformed;
        public event EventHandler<TextureTransformEventArgs> TextureTransformed;

        public TexturedModelEntity(T modelShape, TexturePaths texturePaths) : base(modelShape) => TexturePaths = texturePaths;

        public void TranslateTexture(float x, float y)
        {
            ModelShape.TranslateTexture(x, y);
            TextureTransformed?.Invoke(this, new TextureTransformEventArgs(ID, new Vector2(x, y), 0.0f, Vector2.One));
        }

        public void RotateTexture(float angle)
        {
            ModelShape.RotateTexture(angle);
            TextureTransformed?.Invoke(this, new TextureTransformEventArgs(ID, Vector2.Zero, angle, Vector2.One));
        }

        public void ScaleTexture(float x, float y)
        {
            ModelShape.ScaleTexture(x, y);
            TextureTransformed?.Invoke(this, new TextureTransformEventArgs(ID, Vector2.Zero, 0.0f, new Vector2(x, y)));
        }

        public void AddMaterial(Material material) => Material = material;

        public void AddTextureMapping(TextureMapping? textureMapping) => TextureMapping = textureMapping;

        public void BindTextures(ShaderProgram program, ITextureProvider textureProvider)
        {
            if (TextureMapping.HasValue)
            {
                program.BindTextures(textureProvider, TextureMapping.Value);
            }
            else
            {
                program.UnbindTextures();
            }
        }

        public override void SetUniforms(ShaderProgram program)
        {
            base.SetUniforms(program);
            Material.SetUniforms(program);
        }

        public override bool CompareUniforms(IEntity entity) => entity is ITextureBinder textureBinder
            && Material.Equals(textureBinder.Material)
            && TextureMapping.Equals(textureBinder.TextureMapping);
    }
}
