using OpenTK;
using OpenTK.Graphics.OpenGL;
using SpiceEngineCore.Entities.Lights;
using SpiceEngineCore.Inputs;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Shaders;
using System;
using System.Collections.Generic;

namespace SpiceEngineCore.Entities.Cameras
{
    public abstract class Camera : ICamera
    {
        public int ID { get; set; }
        public string Name { get; }

        // TODO - Fix this...
        public ModelMatrix WorldMatrix => throw new NotImplementedException();

        public Vector3 Position
        {
            get => _viewMatrix.Translation;
            set
            {
                _viewMatrix.Translation = value;
                //Transformed?.Invoke(this, new EntityTransformEventArgs(ID, _viewMatrix.Matrix));
            }
        }

        public IEntity AttachedEntity { get; private set; }
        public Vector3 AttachedTranslation { get; protected set; }

        public ViewMatrix _viewMatrix = new ViewMatrix();
        internal ProjectionMatrix _projectionMatrix = new ProjectionMatrix();

        protected float _distance;
        public Matrix4 ViewMatrix => _viewMatrix.Matrix;
        public Matrix4 ProjectionMatrix => _projectionMatrix.Matrix;
        public Matrix4 ViewProjectionMatrix => _viewMatrix.Matrix * _projectionMatrix.Matrix;

        public event EventHandler<EntityTransformEventArgs> Transformed;

        public Camera(string name) => Name = name;

        public void Transform(Transform transform) => throw new NotImplementedException();

        public void Translate(Vector3 translation)
        {
            _viewMatrix.Translation *= translation;
            //Transformed?.Invoke(this, new EntityTransformEventArgs(ID, _viewMatrix.Matrix));
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

        public abstract void OnHandleInput(InputManager inputManager);

        public void SetUniforms(ShaderProgram program)
        {
            _viewMatrix.Set(program);
            _projectionMatrix.Set(program);
        }

        public void SetUniforms(ShaderProgram program, ILight light)
        {
            if (light is PointLight pointLight)
            {
                var shadowViews = new List<Matrix4>();

                for (var i = 0; i < 6; i++)
                {
                    shadowViews.Add(pointLight.GetView(TextureTarget.TextureCubeMapPositiveX + i) * pointLight.Projection);
                }

                program.SetUniform(SpiceEngineCore.Rendering.Matrices.ViewMatrix.SHADOW_NAME, shadowViews.ToArray());
            }
            else if (light is SpotLight spotLight)
            {
                program.SetUniform(SpiceEngineCore.Rendering.Matrices.ProjectionMatrix.NAME, spotLight.Projection);
                program.SetUniform(SpiceEngineCore.Rendering.Matrices.ViewMatrix.NAME, spotLight.View);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public bool CompareUniforms(IEntity entity) => false;
    }
}
