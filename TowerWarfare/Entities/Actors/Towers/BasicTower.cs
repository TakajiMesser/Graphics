using OpenTK;
using OpenTK.Graphics;
using SweetGraphicsCore.Rendering.Models;

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
 