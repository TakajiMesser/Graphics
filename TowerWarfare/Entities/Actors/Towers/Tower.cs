using SpiceEngineCore.Helpers;
using SpiceEngineCore.Maps;
using SweetGraphicsCore.Rendering.Models;
using SweetGraphicsCore.Vertices;
using System.Linq;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace TowerWarfare.Entities.Actors.Towers
{
    public abstract class Tower : MapActor
    {
        public Tower(Vector3 position)
        {
            Position = position;
            Scale = Vector3.One;
            Rotation = Vector3.Zero;
            Orientation = Vector3.Zero;
            IsPhysical = true;
            Color = GetColor();

            BuildShape();
        }

        protected abstract ModelMesh GetShape();// => ModelMesh.Box(2.0f, 2.0f, 2.0f);
        protected abstract Color4 GetColor();

        private void BuildShape()
        {
            var modelMesh = GetShape();
            var modelBuilder = new ModelBuilder(modelMesh);

            Vertices.AddRange(modelBuilder.GetVertices().Select(v => new Vertex3D(v.Position, v.Normal, v.Tangent, v.UV, Color)));
            TriangleIndices.AddRange(modelBuilder.TriangleIndices);
            Material = SpiceEngineCore.Rendering.Materials.Material.LoadFromFile(FilePathHelper.GENERIC_MATERIAL_PATH).First().Item2;
        }
    }
}
 