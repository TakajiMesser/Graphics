using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.GameObjects
{
    public class Camera
    {
        public const float MAX_FIELD_OF_VIEW = 2.0f;
        public const float MIN_FIELD_OF_VIEW = 1.0f;

        private string _name;
        private ShaderProgram _program;
        private Matrix4Uniform _viewMatrix;
        private Matrix4Uniform _projectionMatrix;
        private float _fieldOfView = (float)Math.PI / 2.0f;

        public Matrix4Uniform View => _viewMatrix;
        public Matrix4Uniform Projection => _projectionMatrix;
        public float FieldOfView => _fieldOfView;
        public Transform Transform { get; set; }

        public Camera(string name, ShaderProgram program, float width, float height)
        {
            _name = name;
            _program = program;

            _viewMatrix = new Matrix4Uniform("viewMatrix")
            {
                Matrix = Matrix4.Identity//Matrix4.LookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 3) + new Vector3(0, 0, -1.0f), new Vector3(0, 1, 0))
            };

            // Specify the FOV
            _projectionMatrix = new Matrix4Uniform("projectionMatrix")
            {
                Matrix = Matrix4.CreatePerspectiveFieldOfView(FieldOfView, width / height, 1.0f, 100.0f)
            };
        }

        public void OnUpdateFrame()
        {
            /*if (Transform != null)
            {
                ModelMatrix.Matrix *= Transform.ToModelMatrix();
            }*/

            if (Transform != null)
            {
                _viewMatrix.Matrix *= Matrix4.CreateTranslation(Transform.Translation);// Transform.ToModelMatrix();
            }
        }

        public void AdjustFieldOfView(float amount, int width, int height)
        {
            float fieldOfView = _fieldOfView + amount;

            if (fieldOfView < MAX_FIELD_OF_VIEW && fieldOfView > MIN_FIELD_OF_VIEW)
            {
                _fieldOfView = fieldOfView;
                _projectionMatrix.Matrix = Matrix4.CreatePerspectiveFieldOfView(_fieldOfView, width / height, 1.0f, 100.0f);
            }
        }

        public void OnRenderFrame()
        {
            _viewMatrix.Set(_program);
            _projectionMatrix.Set(_program);
        }
    }
}
