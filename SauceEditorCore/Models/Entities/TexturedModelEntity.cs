using OpenTK;
using SpiceEngine.Entities.Selection;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Rendering.Materials;
using SpiceEngineCore.Rendering.Models;
using SpiceEngineCore.Rendering.Shaders;
using SpiceEngineCore.Rendering.Textures;
using SpiceEngineCore.Utilities;
using System;
using System.Collections.Generic;

namespace SauceEditorCore.Models.Entities
{
    public abstract class TexturedModelEntity<T> : ModelEntity<T>, IRotate, IScale, /*ITextureBinder, ITexturePath,*/ ITexturedEntity, IDirectional where T : IModelShape, ITexturedShape
    {
        private Vector2 _texturePosition;
        private float _textureRotation;
        private Vector2 _textureScale;

        public bool IsInTextureMode { get; set; }

        // TODO - Determine if quaternion multiplication order matters here
        //var rotationChange = value * _modelMatrix.Rotation.Inverted();
        //var transform = Matrix4.CreateTranslation(-Position) * Matrix4.CreateFromQuaternion(rotationChange) * Matrix4.CreateTranslation(Position);
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

        protected TexturePaths _texturePaths;
        protected Material _material;
        protected TextureMapping _textureMapping;

        /*public IEnumerable<Material> Materials => CurrentMaterial.Yield();
        public IEnumerable<TextureMapping?> TextureMappings => CurrentTextureMapping.Yield();

        public Material CurrentMaterial { get; private set; }
        public TextureMapping? CurrentTextureMapping { get; private set; }*/

        public abstract Vector3 XDirection { get; }
        public abstract Vector3 YDirection { get; }
        public abstract Vector3 ZDirection { get; }

        //public override event EventHandler<EntityTransformEventArgs> Transformed;
        public event EventHandler<TextureTransformEventArgs> TextureTransformed;

        public TexturedModelEntity(T modelShape, TexturePaths texturePaths) : base(modelShape) => _texturePaths = texturePaths;

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

        /*public void AddMaterial(Material material) => CurrentMaterial = material;

        public void AddTextureMapping(TextureMapping? textureMapping) => CurrentTextureMapping = textureMapping;

        public void BindTextures(ShaderProgram program, ITextureProvider textureProvider)
        {
            if (CurrentTextureMapping.HasValue)
            {
                program.BindTextures(textureProvider, CurrentTextureMapping.Value);
            }
            else
            {
                program.UnbindTextures();
            }
        }

        public override void SetUniforms(ShaderProgram program)
        {
            base.SetUniforms(program);
            CurrentMaterial.SetUniforms(program);
        }

        public override bool CompareUniforms(IEntity entity) => entity is ITextureBinder textureBinder
            && CurrentMaterial.Equals(textureBinder.CurrentMaterial)
            && CurrentTextureMapping.Equals(textureBinder.CurrentTextureMapping);*/
    }
}
