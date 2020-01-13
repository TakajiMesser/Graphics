using OpenTK;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.UserInterfaces;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Helpers;
using SpiceEngineCore.Maps;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Materials;
using SpiceEngineCore.Rendering.Models;
using SpiceEngineCore.Rendering.Textures;
using SpiceEngineCore.Rendering.Vertices;
using StarchUICore.Views.Controls.Buttons;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StarchUICore.Maps
{
    public class MapUIElement : MapEntity<IUIElement>, IMapUIElement, ITexturePather
    {
        public enum UITypes
        {
            Button,
            Label,
            Panel
        }

        public string Name { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public UITypes UIType { get; set; }
        public List<TexturePaths> TexturesPaths { get; set; } = new List<TexturePaths>();

        public override IEntity ToEntity() => new UIElement()
        {
            Name = Name,
            Position = new Vector3(X, Y, 0.0f)
        };

        IRenderable IComponentBuilder<IRenderable>.ToComponent()
        {
            if (!TexturesPaths.Any() && UIType == UITypes.Button)
            {
                return Button.CreateButton(X, Y, Width, Height);
            }

            throw new NotImplementedException();
        }

        /*public static MapBrush Rectangle(Vector3 center, float width, float height)
        {
            var meshShape = new ModelMesh();
            meshShape.Faces.Add(ModelFace.Rectangle(width, height));
            var meshBuild = new ModelBuilder(meshShape);

            return new MapBrush()
            {
                Position = center,
                Vertices = meshBuild.GetVertices().Select(v => new Vertex3D(v.Position, v.Normal, v.Tangent, v.UV)).ToList(),
                TriangleIndices = meshBuild.TriangleIndices,
                Material = Material.LoadFromFile(FilePathHelper.GENERIC_MATERIAL_PATH).First().Item2
            };
        }*/
    }
}
