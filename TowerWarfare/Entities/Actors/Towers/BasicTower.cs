using SweetGraphicsCore.Rendering.Models;

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
    public class BasicTower : Tower
    {
        public BasicTower(Vector3 position) : base(position) => Name = "BasicTower";
        public BasicTower(Vector3 position, string name) : base(position) => Name = name;

        protected override ModelMesh GetShape() => ModelMesh.Box(0.6f, 0.6f, 5.0f);

        protected override Color4 GetColor() => Color4.ForestGreen;
    }
}
 