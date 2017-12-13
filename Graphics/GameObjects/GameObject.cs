using Graphics.Meshes;
using Graphics.Physics.Collision;
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
        public string Name { get; private set; }
        public Mesh Mesh { get; set; }
        public Transform Transform { get; set; }
        public Vector3 Position
        {
            get => new Vector3(_modelMatrix.Matrix.M41, _modelMatrix.Matrix.M42, _modelMatrix.Matrix.M43);
            set
            {
                _modelMatrix.Matrix = Transform.FromTranslation(value).ToModelMatrix();
                Collider.Center = value;
            }
        }
        public ICollider Collider { get; set; }

        internal ShaderProgram _program;
        private Matrix4Uniform _modelMatrix;

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

        public virtual void OnHandleInput(KeyboardState keyState, MouseState mouseState, KeyboardState previousKeyState, MouseState previousMouseState)
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
                var newModel = _modelMatrix.Matrix * Transform.ToModelMatrix();
                Collider.Center = new Vector3(newModel.M41, newModel.M42, newModel.M43);

                foreach (var collider in colliders)
                {
                    if (Collider.CollidesWith((BoundingSphere)collider))
                    {
                        //Collider.Center = Position;
                        //return;
                    }
                }

                _modelMatrix.Matrix *= Transform.ToModelMatrix();
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
