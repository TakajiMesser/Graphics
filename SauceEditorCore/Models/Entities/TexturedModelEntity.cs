﻿using SpiceEngine.Entities.Selection;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Rendering.Materials;
using SpiceEngineCore.Rendering.Textures;
using SweetGraphicsCore.Rendering.Models;
using SweetGraphicsCore.Rendering.Textures;
using System;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

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
        }*/
    }
}
