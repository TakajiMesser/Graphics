using Graphics.Meshes;
using OpenTK;
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
        public Transform Transform { get; set; }
        public Mesh Mesh { get; set; }
        public Matrix4Uniform ModelMatrix { get => _modelMatrix; set => _modelMatrix = value; }

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

        public virtual void OnUpdateFrame()
        {
            if (Transform != null)
            {
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
