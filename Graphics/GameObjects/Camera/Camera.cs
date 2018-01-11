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

namespace Graphics.GameObjects
{
    public abstract class Camera
    {
        private string _name;
        protected ViewMatrix _viewMatrix = new ViewMatrix();
        protected ProjectionMatrix _projectionMatrix = new ProjectionMatrix();

        public GameObject AttachedObject { get; private set; }
        public Vector3 AttachedTranslation { get; private set; }

        public Vector3 Position
        {
            get => _viewMatrix.Translation;
            set => _viewMatrix.Translation = value;
        }
        public Matrix4 ViewProjectionMatrix => _projectionMatrix.Projection * _viewMatrix.View;

        public Camera(string name, int width, int height)
        {
            _name = name;
            _projectionMatrix.AspectRatio = (float)width / height;
        }

        public void AttachToGameObject(GameObject gameObject, bool attachTranslation, bool attachRotation)
        {
            AttachedObject = gameObject;

            // Determine the original distance from the attached object, based on the current camera position
            AttachedTranslation = gameObject.Position - Position;
        }

        public void UpdateAspectRatio(int width, int height) => _projectionMatrix.AspectRatio = (float)width / height;

        public void OnUpdateFrame()
        {
            if (AttachedObject != null)
            {
                Position = AttachedObject.Position - AttachedTranslation;
            }
        }

        public abstract void OnHandleInput(InputState inputState);

        public void Draw(ShaderProgram program)
        {
            _viewMatrix.Set(program);
            _projectionMatrix.Set(program);
        }
    }
}
