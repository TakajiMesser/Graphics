using OpenTK;
using OpenTK.Graphics;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Volumes;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Physics.Shapes;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Meshes;
using SpiceEngineCore.Rendering.Models;
using SpiceEngineCore.Rendering.Vertices;
using SpiceEngineCore.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngineCore.Maps
{
    public class MapVolume : MapEntity<IVolume>, IMapVolume
    {
        public enum VolumeTypes
        {
            Blocking,
            Physics,
            Trigger
        }

        public MapVolume() { }
        public MapVolume(ModelBuilder meshBuild)
        {
            Vertices.AddRange(meshBuild.GetVertices().Select(v => v.Position));
            TriangleIndices.AddRange(meshBuild.TriangleIndices);
        }

        public VolumeTypes VolumeType { get; set; }
        public Vector3 Gravity { get; set; }

        public List<Vector3> Vertices { get; set; } = new List<Vector3>();
        public List<int> TriangleIndices { get; set; } = new List<int>();
        public Color4 Color { get; set; }

        public bool IsPhysical => VolumeType == VolumeTypes.Blocking;

        /*public Mesh3D<Vertex3D> ToMesh()
        {
            var vertices = Vertices.Select(v => new Vertex3D(v, v, v, Vector2.Zero, Color4.Blue)).ToList();
            return new Mesh3D<Vertex3D>(vertices, TriangleIndices);
        }*/

        public override IEntity ToEntity()
        {
            switch (VolumeType)
            {
                case VolumeTypes.Blocking:
                    return new BlockingVolume()//Vertices, TriangleIndices, Color)
                    {
                        Position = Position,
                        Rotation = Quaternion.FromEulerAngles(Rotation.ToRadians()),
                        Scale = Scale
                    };
                case VolumeTypes.Physics:
                    return new PhysicsVolume()//Vertices, TriangleIndices, Color)
                    {
                        Position = Position,
                        Rotation = Quaternion.FromEulerAngles(Rotation.ToRadians()),
                        Scale = Scale,
                        Gravity = Gravity
                    };
                case VolumeTypes.Trigger:
                    return new TriggerVolume()//Vertices, TriangleIndices, Color)
                    {
                        Position = Position,
                        Rotation = Quaternion.FromEulerAngles(Rotation.ToRadians()),
                        Scale = Scale
                    };
            }

            return new Volume()//Vertices, TriangleIndices, Color)
            {
                Position = Position,
                Rotation = Quaternion.FromEulerAngles(Rotation.ToRadians()),
                Scale = Scale
            };
        }

        IShape IComponentBuilder<IShape>.ToComponent() => new Box(Vertices);

        IRenderable IComponentBuilder<IRenderable>.ToComponent() => new ColoredMesh<ColorVertex3D>(Vertices.Select(v => new ColorVertex3D(v, Color4.White)).ToList(), TriangleIndices);

        public static MapVolume Rectangle(Vector3 center, float width, float height)
        {
            var meshShape = new ModelMesh();
            meshShape.Faces.Add(ModelFace.Rectangle(width, height));
            var meshBuild = new ModelBuilder(meshShape);

            return new MapVolume()
            {
                Position = center,
                Vertices = meshBuild.Positions.ToList(),
                TriangleIndices = meshBuild.TriangleIndices
            };
        }

        public static MapVolume Box(Vector3 center, float width, float height, float depth)
        {
            var meshShape = ModelMesh.Box(width, height, depth);
            var meshBuild = new ModelBuilder(meshShape);

            return new MapVolume()
            {
                Position = center,
                Vertices = meshBuild.Positions.ToList(),
                TriangleIndices = meshBuild.TriangleIndices
            };
        }
    }
}
