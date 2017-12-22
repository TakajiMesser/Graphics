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
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; }
        public string MeshFilePath { get; set; }

        public Player ToPlayer(ShaderProgram program)
        {
            var player = new Player()
            {
                Mesh = Mesh.LoadFromFile(MeshFilePath, program)
            };

            if (Position != null)
            {
                player.Position = Position;
            }

            if (Rotation != null)
            {
                player.Rotation = Rotation;
            }

            if (Scale != null)
            {
                player.Scale = Scale;
            }

            return player;
        }
    }
}
