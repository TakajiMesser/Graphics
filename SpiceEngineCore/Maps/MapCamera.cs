using Newtonsoft.Json;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Outputs;
using SpiceEngineCore.Rendering.Matrices;

namespace SpiceEngineCore.Maps
{
    public class MapCamera : MapEntity3D<ICamera>, IMapCamera
    {
        public string Name { get; set; }

        public string AttachedActorName { get; set; }
        public ProjectionTypes Type { get; set; }
        public float ZNear { get; set; }
        public float ZFar { get; set; }

        /// <summary>
        /// Only relevant for orthographic cameras
        /// </summary>
        public float StartingWidth { get; set; }

        /// <summary>
        /// Only relevant for perspective cameras
        /// </summary>
        public float FieldOfViewY { get; set; }

        [JsonIgnore]
        public Resolution Resolution { get; set; }

        public override IEntity ToEntity()
        {
            var camera = Type == ProjectionTypes.Orthographic
                ? (ICamera)new OrthographicCamera(Name, Resolution, ZNear, ZFar, StartingWidth)
                : new PerspectiveCamera(Name, Resolution, ZNear, ZFar, FieldOfViewY);

            if (Position != null)
            {
                camera.Position = Position;
            }

            return camera;
        }
    }
}
