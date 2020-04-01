using OpenTK;
using OpenTK.Graphics;
using SweetGraphicsCore.Rendering.Models;

namespace TowerWarfare.Entities.Actors.Towers
{
    public class BasicTower : Tower
    {
        public BasicTower(Vector3 position) : base(position) => Name = "BasicTower01";

        protected override ModelMesh GetShape() => ModelMesh.Box(2.0f, 2.0f, 2.0f);

        protected override Color4 GetColor() => Color4.ForestGreen;
    }
}
 