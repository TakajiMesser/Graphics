using OpenTK;
using SpiceEngine.Entities.Volumes;
using SpiceEngine.Helpers;
using SpiceEngine.Physics.Shapes;
using SpiceEngine.Rendering.Materials;
using SpiceEngine.Rendering.Vertices;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Maps
{
    public class MapVolume : MapEntity3D<Volume>
    {
        public enum VolumeTypes
        {
            Blocking,
            Physics,
            Trigger
        }

        public VolumeTypes VolumeType { get; set; }
        public Vector3 Gravity { get; set; }

        public List<Vector3> Vertices { get; set; } = new List<Vector3>();
        public List<int> TriangleIndices { get; set; } = new List<int>();

        /*public Mesh3D<Vertex3D> ToMesh()
        {
            var vertices = Vertices.Select(v => new Vertex3D(v, v, v, Vector2.Zero, Color4.Blue)).ToList();
            return new Mesh3D<Vertex3D>(vertices, TriangleIndices);
        }*/

        public override Volume ToEntity()
        {
            switch (VolumeType)
            {
                case VolumeTypes.Blocking:
                    return new BlockingVolume()
                    {
                        Position = Position,
                        OriginalRotation = Rotation,
                        Scale = Scale
                    };
                case VolumeTypes.Physics:
                    return new PhysicsVolume()
                    {
                        Position = Position,
                        OriginalRotation = Rotation,
                        Scale = Scale,
                        Gravity = Gravity
                    };
                case VolumeTypes.Trigger:
                    return new TriggerVolume()
                    {
                        Position = Position,
                        OriginalRotation = Rotation,
                        Scale = Scale
                    };
            }

            throw new NotImplementedException();
        }

        public override Shape3D ToShape() => new Box(Vertices);

        public static MapVolume Box(Vector3 center, float width, float height, float depth)
        {
            var meshShape = MeshShape.Box(width, height, depth);

            return new MapVolume()
            {
                Position = center,
                Vertices = meshShape.Vertices,
                TriangleIndices = meshShape.TriangleIndices
            };
        }
    }
}
