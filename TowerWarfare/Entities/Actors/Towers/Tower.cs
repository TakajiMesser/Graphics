using SpiceEngineCore.Helpers;
using SpiceEngineCore.Maps;
using SweetGraphicsCore.Rendering.Models;
using SweetGraphicsCore.Vertices;
using System.Linq;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

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
 