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
    public class MapPlayer
    {
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public Transform Transform { get; set; } = new Transform();
        public string MeshFilePath { get; set; }

        public Player ToPlayer(ShaderProgram program)
        {
            return new Player()
            {
                Position = Position,
                Transform = Transform,
                Mesh = Mesh.LoadFromFile(MeshFilePath, program)
            };
        }
    }
}
