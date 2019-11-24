using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Scripting;
using SpiceEngineCore.Scripting.Properties;
using SpiceEngineCore.Scripting.Scripts;
using SpiceEngineCore.Scripting.StimResponse;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngineCore.Maps
{
    public class MapCamera : MapEntity<ICamera>, IMapCamera
    {
        public string Name { get; set; }

        public string AttachedEntityName { get; set; }
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

        public MapBehavior Behavior { get; set; }

        public IEnumerable<Script> Scripts => Behavior != null ? Behavior.GetScripts() : Enumerable.Empty<Script>();
        public List<Stimulus> Stimuli { get; private set; } = new List<Stimulus>();
        public List<Property> Properties { get; set; } = new List<Property>();

        public override IEntity ToEntity()
        {
            var camera = Type == ProjectionTypes.Orthographic
                ? (ICamera)new OrthographicCamera(Name, ZNear, ZFar, StartingWidth)
                : new PerspectiveCamera(Name, ZNear, ZFar, FieldOfViewY);

            if (Position != null)
            {
                camera.Position = Position;
            }

            return camera;
        }

        IBehavior IComponentBuilder<IBehavior>.ToComponent() => Behavior?.ToBehavior();
    }
}
