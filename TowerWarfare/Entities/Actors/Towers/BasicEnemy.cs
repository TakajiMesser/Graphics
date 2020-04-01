using OpenTK;
using OpenTK.Graphics;
using SpiceEngine.Maps;
using SpiceEngineCore.Helpers;
using SpiceEngineCore.Maps;
using SweetGraphicsCore.Rendering.Models;
using SweetGraphicsCore.Vertices;
using System.Linq;

namespace TowerWarfare.Entities.Actors.Towers
{
    public class BasicEnemy : Enemy
    {
        public BasicEnemy(Vector3 position) : base(position) => Name = "BasicEnemy01";

        protected override ModelMesh GetShape() => ModelMesh.Box(2.0f, 2.0f, 2.0f);

        protected override Color4 GetColor() => Color4.ForestGreen;
    }
}
 