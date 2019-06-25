using OpenTK;
using OpenTK.Graphics.OpenGL;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Inputs;
using SpiceEngine.Rendering.Matrices;
using SpiceEngine.Rendering.Shaders;
using System.Collections.Generic;

namespace SpiceEngine.Entities.Cameras
{
    public abstract class Camera : ICamera
    {
        public int ID { get; set; }
        public string Name { get; }

        public Vector3 Position
        {
            get => _viewMatrix.Translation;
            set => _viewMatrix.Translation = value;
        }

        public IEntity AttachedEntity { get; private set; }
        public Vector3 AttachedTranslation { get; protected set; }

        public ViewMatrix _viewMatrix = new ViewMatrix();
        internal ProjectionMatrix _projectionMatrix = new ProjectionMatrix();

        protected float _distance;
        public Matrix4 ViewMatrix => _viewMatrix.Matrix;
        public Matrix4 ProjectionMatrix => _projectionMatrix.Matrix;
        public Matrix4 ViewProjectionMatrix => _viewMatrix.Matrix * _projectionMatrix.Matrix;

        public Camera(string name) => Name = name;

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

        public abstract void OnHandleInput(InputManager inputManager);

        public void SetUniforms(ShaderProgram program)
        {
            _viewMatrix.Set(program);
            _projectionMatrix.Set(program);
        }

        public void SetUniforms(ShaderProgram program, PointLight light)
        {
            var shadowViews = new List<Matrix4>();
            for (var i = 0; i < 6; i++)
            {
                shadowViews.Add(light.GetView(TextureTarget.TextureCubeMapPositiveX + i) * light.Projection);
            }
            program.SetUniform(Rendering.Matrices.ViewMatrix.SHADOW_NAME, shadowViews.ToArray());
        }

        public void SetUniforms(ShaderProgram program, SpotLight light)
        {
            program.SetUniform(Rendering.Matrices.ProjectionMatrix.NAME, light.Projection);
            program.SetUniform(Rendering.Matrices.ViewMatrix.NAME, light.View);
        }
    }
}
