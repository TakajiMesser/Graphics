using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using TakoEngine.Entities.Lights;
using TakoEngine.Inputs;
using TakoEngine.Outputs;
using TakoEngine.Rendering.Matrices;
using TakoEngine.Rendering.Shaders;

namespace TakoEngine.Entities.Cameras
{
    public abstract class Camera : IEntity
    {
        public int ID { get; set; }

        public Vector3 Position
        {
            get => _viewMatrix.Translation;
            set => _viewMatrix.Translation = value;
        }

        public Vector3 OriginalRotation { get; set; }

        public Quaternion Rotation
        {
            get => throw new NotImplementedException();// _viewMatrix.;
            set => throw new NotImplementedException(); //_viewMatrix.Rotation = value;
        }

        public Vector3 Scale
        {
            get => throw new NotImplementedException(); //_viewMatrix.Scale;
            set => throw new NotImplementedException(); //_viewMatrix.Scale = value;
        }

        public IEntity AttachedEntity { get; private set; }
        public Vector3 AttachedTranslation { get; protected set; }

        private string _name;
        public ViewMatrix _viewMatrix = new ViewMatrix();
        internal ProjectionMatrix _projectionMatrix = new ProjectionMatrix();

        protected float _distance;
        public Matrix4 ViewProjectionMatrix => _viewMatrix.Matrix * _projectionMatrix.Matrix;

        public Camera(string name, Resolution resolution)
        {
            _name = name;
            _projectionMatrix.Resolution = resolution;
        }

        public void AttachToEntity(IEntity entity, bool attachTranslation, bool attachRotation)
        {
            AttachedEntity = entity;

            // Determine the original distance from the attached object, based on the current camera position
            AttachedTranslation = entity.Position - Position;
            _distance = AttachedTranslation.Length;
        }

        public void DetachFromEntity()
        {
            AttachedEntity = null;
            AttachedTranslation = Vector3.Zero;
            _distance = 0.0f;
        }

        public void OnUpdateFrame()
        {
            if (AttachedEntity != null)
            {
                Position = AttachedEntity.Position - AttachedTranslation;
                _viewMatrix.LookAt = AttachedEntity.Position;
            }
        }

        public abstract void OnHandleInput(InputState inputState);

        public void Draw(ShaderProgram program)
        {
            _viewMatrix.Set(program);
            _projectionMatrix.Set(program);
        }

        public void DrawFromLight(ShaderProgram program, PointLight light)
        {
            var shadowViews = new List<Matrix4>();
            for (var i = 0; i < 6; i++)
            {
                shadowViews.Add(light.GetView(TextureTarget.TextureCubeMapPositiveX + i) * light.GetProjection(_projectionMatrix.Resolution));
            }
            program.SetUniform(ViewMatrix.SHADOW_NAME, shadowViews.ToArray());
        }

        public void DrawFromLight(ShaderProgram program, PointLight light, TextureTarget target)
        {
            program.SetUniform(ProjectionMatrix.NAME, light.GetProjection(_projectionMatrix.Resolution));
            program.SetUniform(ViewMatrix.NAME, light.GetView(target));
        }

        public void DrawFromLight(ShaderProgram program, SpotLight light)
        {
            program.SetUniform(ProjectionMatrix.NAME, light.GetProjection(_projectionMatrix.Resolution));
            program.SetUniform(ViewMatrix.NAME, light.View);
        }
    }
}
