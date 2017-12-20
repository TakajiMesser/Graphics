using Graphics.Inputs;
using Graphics.Meshes;
using Graphics.Physics.Collision;
using Graphics.Rendering.Shaders;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.GameObjects
{
    public class GameObject
    {
        internal ShaderProgram _program;
        private Matrix4Uniform _modelMatrix;

        public string Name { get; private set; }
        public Mesh Mesh { get; set; }
        public Transform Transform { get; set; } = new Transform();
        public ICollider Collider { get; set; }
        public Vector3 Position
        {
            get => new Vector3(_modelMatrix.Matrix.M41, _modelMatrix.Matrix.M42, _modelMatrix.Matrix.M43);
            set
            {
                _modelMatrix.Matrix = Transform.FromTranslation(value).ToModelMatrix();

                if (Collider != null)
                {
                    Collider.Center = value;
                }
            }
        }
        public Quaternion Rotation
        {
            get => new Quaternion();
        }

        public GameObject(string name)
        {
            Name = name;

            _modelMatrix = new Matrix4Uniform("modelMatrix")
            {
                Matrix = Matrix4.Identity
            };
        }

        public GameObject(string name, Vector3 position)
        {
            Name = name;

            _modelMatrix = new Matrix4Uniform("modelMatrix")
            {
                Matrix = Transform.FromTranslation(position).ToModelMatrix()
            };
        }

        public virtual void OnHandleInput(InputState inputState)
        {

        }

        public virtual void OnUpdateFrame()
        {
            if (Transform != null && (Transform.Translation != Vector3.Zero || Transform.Rotation != Quaternion.Identity || Transform.Scale != Vector3.One))
            {
                _modelMatrix.Matrix *= Transform.ToModelMatrix();
                Collider.Center = Position;
            }
        }

        public virtual void OnUpdateFrame(IEnumerable<ICollider> colliders)
        {
            if (Transform != null && (Transform.Translation != Vector3.Zero || Transform.Rotation != Quaternion.Identity || Transform.Scale != Vector3.One))
            {
                // Translate back to the origin, in order to first perform the rotation, then translate back into place
                var position = new Vector3(_modelMatrix.Matrix.M41, _modelMatrix.Matrix.M42, _modelMatrix.Matrix.M43);

                _modelMatrix.Matrix *= Matrix4.CreateTranslation(-position);
                _modelMatrix.Matrix *= Matrix4.CreateFromQuaternion(Transform.Rotation);
                _modelMatrix.Matrix *= Matrix4.CreateTranslation(position);

                _modelMatrix.Matrix *= Matrix4.CreateScale(Transform.Scale);

                var translatedModel = _modelMatrix.Matrix * Matrix4.CreateTranslation(Transform.Translation);

                if (Collider != null)
                {
                    Collider.Center = new Vector3(translatedModel.M41, translatedModel.M42, translatedModel.M43);

                    foreach (var collider in colliders)
                    {
                        if (Collider.CollidesWith((BoundingSphere)collider))
                        {
                            Collider.Center = Position;
                            return;
                        }
                    }
                }

                _modelMatrix.Matrix = translatedModel;
            }
        }

        public virtual void OnRenderFrame()
        {
            _modelMatrix.Set(_program);
            Mesh?.Draw();
        }

        // Define how this object's state will be saved, if desired
        public virtual void OnSaveState() { }
    }
}
