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

namespace Graphics.GameObjects
{
    public abstract class Camera
    {
        private string _name;
        internal ViewMatrix _viewMatrix = new ViewMatrix();
        internal ProjectionMatrix _projectionMatrix = new ProjectionMatrix();
        internal Matrix4 _previousViewMatrix;
        internal Matrix4 _previousProjectionMatrix;

        protected float _distance;

        public GameObject AttachedObject { get; private set; }
        public Vector3 AttachedTranslation { get; protected set; }

        public Vector3 Position
        {
            get => _viewMatrix.Translation;
            set => _viewMatrix.Translation = value;
        }
        public Matrix4 ViewProjectionMatrix => _projectionMatrix.Projection * _viewMatrix.View;

        public Camera(string name, Resolution resolution)
        {
            _name = name;
            _projectionMatrix.Resolution = resolution;
        }

        public void AttachToGameObject(GameObject gameObject, bool attachTranslation, bool attachRotation)
        {
            AttachedObject = gameObject;

            // Determine the original distance from the attached object, based on the current camera position
            AttachedTranslation = gameObject.Position - Position;
            _distance = AttachedTranslation.Length;
        }

        public void OnUpdateFrame()
        {
            if (AttachedObject != null)
            {
                Position = AttachedObject.Position - AttachedTranslation;
                _viewMatrix.LookAt = AttachedObject.Position;
            }
        }

        public abstract void OnHandleInput(InputState inputState);

        public void Draw(ShaderProgram program)
        {
            _viewMatrix.Set(program);

            int location = program.GetUniformLocation("previousViewMatrix");
            GL.UniformMatrix4(location, false, ref _previousViewMatrix);

            _previousViewMatrix = _viewMatrix.View;

            _projectionMatrix.Set(program);

            int location2 = program.GetUniformLocation("previousProjectionMatrix");
            GL.UniformMatrix4(location2, false, ref _previousProjectionMatrix);

            _previousProjectionMatrix = _projectionMatrix.Projection;

            /*_modelMatrix.Set(program);

            var model = _modelMatrix.Model;
            int location = program.GetUniformLocation("previousModelMatrix");
            GL.UniformMatrix4(location, false, ref model);

            _mesh.Draw();*/
        }
    }
}
