using SweetGraphicsCore.Rendering.Models;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace TowerWarfare.Entities.Actors.Troops
{
    public class BasicEnemy : Enemy
    {
        public BasicEnemy(Vector3 position) : base(position) => Name = "BasicEnemy";

        protected override ModelMesh GetShape() => ModelMesh.Box(1.0f, 1.0f, 1.0f);

        protected override Color4 GetColor() => Color4.ForestGreen;
    }
}
 