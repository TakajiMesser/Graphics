using Graphics.GameObjects;
using Graphics.Rendering.Matrices;
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
        public ProjectionTypes Type { get; set; }

        /// <summary>
        /// Only relevant for orthographic cameras
        /// </summary>
        public float StartingWidth { get; set; }

        /// <summary>
        /// Only relevant for perspective cameras
        /// </summary>
        public float FieldOfViewY { get; set; }

        public Camera ToCamera(int width, int height)
        {
            var camera = Type == ProjectionTypes.Orthographic
                ? (Camera)new OrthographicCamera(Name, width, height, StartingWidth)
                : new PerspectiveCamera(Name, width, height, FieldOfViewY);

            if (Position != null)
            {
                camera.Position = Position;
            }

            return camera;
        }
    }
}
