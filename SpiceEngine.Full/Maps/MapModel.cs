using CitrusAnimationCore;
using CitrusAnimationCore.Animations;
using SpiceEngine.Rendering.Models;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Materials;
using SweetGraphicsCore.Rendering.Meshes;
using SweetGraphicsCore.Rendering.Textures;
using SweetGraphicsCore.Vertices;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngineCore.Maps
{
    public class MapModel : IMapModel, ITexturePather
    {
        public List<Vertex3D> Vertices { get; set; } = new List<Vertex3D>();
        public Material Material { get; set; }
        public List<int> TriangleIndices { get; set; } = new List<int>();
        public Color4 Color { get; set; }
        public List<TexturePaths> TexturesPaths { get; set; } = new List<TexturePaths>();

        public IRenderable LoadRenderable()
        {
            // TODO - Determine if Actor's should always be Models, or if they can also just be singular Meshes...
            var model = new Model();

            if (TexturesPaths.Any())
            {
                model.Add(new TexturedMesh<Vertex3D>(new Vertex3DSet<Vertex3D>(Vertices, TriangleIndices))
                {
                    Material = Material
                });
            }
            else
            {
                model.Add(new ColoredMesh<Vertex3D>(new Vertex3DSet<Vertex3D>(Vertices.Select(v => (Vertex3D)v.Colored(Color)).ToList(), TriangleIndices)));
            }

            return model;
        }

        // TODO - Animations are only supported via model file loading with Assimp
        public IAnimator LoadAnimator(int entityID) => null;
        public IEnumerable<IAnimation> LoadAnimations() => Enumerable.Empty<IAnimation>();
    }
}
