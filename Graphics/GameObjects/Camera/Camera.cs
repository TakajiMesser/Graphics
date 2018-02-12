using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graphics.Rendering.Shaders;
using Graphics.Inputs;
using Graphics.Rendering.Matrices;
using Graphics.Outputs;
using Graphics.Lighting;

namespace Graphics.GameObjects
{
    public abstract class Camera
    {
        private string _name;
        public ViewMatrix _viewMatrix = new ViewMatrix();
        internal ProjectionMatrix _projectionMatrix = new ProjectionMatrix();

        protected float _distance;

        public GameObject AttachedObject { get; private set; }
        public Vector3 AttachedTranslation { get; protected set; }

        public Vector3 Position
        {
            get => _viewMatrix.Translation;
            set => _viewMatrix.Translation = value;
        }
        public Matrix4 ViewProjectionMatrix => _viewMatrix.Matrix * _projectionMatrix.Matrix;

        public Camera(string name, Resolution resolution)
        {
            _name = name;
            _projectionMatrix.Resolution = resolution;
        }

        public void AttachToGameObject(GameObject gameObject, bool attachTranslation, bool attachRotation)
        {
            AttachedObject = gameObject;

            // Determine the original distance from the attached object, based on the current camera position
            AttachedTranslation = gameObject.Model.Position - Position;
            _distance = AttachedTranslation.Length;
        }

        public void OnUpdateFrame()
        {
            if (AttachedObject != null)
            {
                Position = AttachedObject.Model.Position - AttachedTranslation;
                _viewMatrix.LookAt = AttachedObject.Model.Position;
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
