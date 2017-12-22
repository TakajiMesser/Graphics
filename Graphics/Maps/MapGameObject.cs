using Graphics.Brushes;
using Graphics.GameObjects;
using Graphics.Meshes;
using Graphics.Rendering.Shaders;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Graphics.Maps
{
    public class MapGameObject
    {
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; }
        public string MeshFilePath { get; set; }
        //public ICollider Collider { get; set; }

        public GameObject ToGameObject(ShaderProgram program)
        {
            var gameObject = new GameObject(Name)
            {
                Mesh = Mesh.LoadFromFile(MeshFilePath, program)
            };

            if (Position != null)
            {
                gameObject.Position = Position;
            }

            if (Rotation != null)
            {
                gameObject.Rotation = Rotation;
            }

            if (Scale != null)
            {
                gameObject.Scale = Scale;
            }

            return gameObject;
        }
    }
}
