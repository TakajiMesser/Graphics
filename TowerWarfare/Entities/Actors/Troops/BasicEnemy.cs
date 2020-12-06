using SpiceEngineCore.Geometry.Colors;
using SpiceEngineCore.Geometry.Vectors;
using SweetGraphicsCore.Rendering.Models;

namespace TowerWarfare.Entities.Actors.Troops
{
    public class BasicEnemy : Enemy
    {
        public BasicEnemy(Vector3 position) : base(position) => Name = "BasicEnemy";

        protected override ModelMesh GetShape() => ModelMesh.Box(1.0f, 1.0f, 1.0f);

        protected override Color4 GetColor() => Color4.ForestGreen;
    }
}
 