﻿using OpenTK;
using TakoEngine.Entities.Cameras;
using TakoEngine.Outputs;
using TakoEngine.Rendering.Matrices;

namespace TakoEngine.Maps
{
    public class MapCamera
    {
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public string AttachedActorName { get; set; }
        public ProjectionTypes Type { get; set; }

        /// <summary>
        /// Only relevant for orthographic cameras
        /// </summary>
        public float StartingWidth { get; set; }

        /// <summary>
        /// Only relevant for perspective cameras
        /// </summary>
        public float FieldOfViewY { get; set; }

        public Camera ToCamera(Resolution resolution)
        {
            var camera = Type == ProjectionTypes.Orthographic
                ? (Camera)new OrthographicCamera(Name, resolution, StartingWidth)
                : new PerspectiveCamera(Name, resolution, FieldOfViewY);

            if (Position != null)
            {
                camera.Position = Position;
            }

            return camera;
        }
    }
}