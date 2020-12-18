using OpenTK;
using OpenTK.Graphics;
using SpiceEngine.Maps;
using SpiceEngineCore.Helpers;
using SweetGraphicsCore.Rendering.Models;
using SweetGraphicsCore.Vertices;
using System.Linq;

namespace TowerWarfare.Entities.Actors.Brushes
{
    public class Tile : MapBrush
    {
        public const float TILE_OFFSET = 2.0f;
        public const float TILE_GAP = 0.1f;
        public const float TILE_Z_POSITION = 1.0f;
        public const float TILE_HEIGHT = 0.5f;

        public Tile(int xOffset, int yOffset)
        {
            Position = new Vector3(xOffset * TILE_OFFSET, yOffset * TILE_OFFSET, TILE_Z_POSITION);
            IsPhysical = true;
            
            //Color = GetColor();
            BuildShape();
        }

        protected virtual ModelMesh GetShape() => ModelMesh.Cylinder(TILE_OFFSET / 2 - TILE_GAP, TILE_HEIGHT, 6);
        protected virtual Color4 GetColor() => Color4.RosyBrown;

        private void BuildShape()
        {
            var modelMesh = GetShape();
            var color = GetColor();

            if (modelMesh != null)
            {
                var modelBuilder = new ModelBuilder(modelMesh);

                Vertices.AddRange(modelBuilder.GetVertices().Select(v => new Vertex3D(v.Position, v.Normal, v.Tangent, v.UV, color)));
                TriangleIndices.AddRange(modelBuilder.TriangleIndices);
                Material = SpiceEngineCore.Rendering.Materials.Material.LoadFromFile(FilePathHelper.GENERIC_MATERIAL_PATH).First().Item2;
            }
        }
    }
}
 