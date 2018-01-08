using Graphics.GameObjects;
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
    public class MapCamera
    {
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public string AttachedGameObjectName { get; set; }

        public Camera ToCamera(int width, int height)
        {
            var camera = new Camera(Name, width, height);

            if (Position != null)
            {
                camera.Position = Position;
            }

            return camera;
        }
    }
}
