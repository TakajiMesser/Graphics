using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.UserInterfaces;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Maps;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Textures;
using StarchUICore.Attributes.Units;
using StarchUICore.Views.Controls.Buttons;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StarchUICore.Maps
{
    public class MapUIItem : MapEntity<UIItem>, IMapUIItem, ITexturePather
    {
        public enum UITypes
        {
            Button,
            Label,
            Panel
        }

        public enum UnitTypes
        {
            Auto,
            Pixels,
            Percents
        }

        public string Name { get; set; }

        /*public IUnits X { get; set; }
        public IUnits Y { get; set; }
        public IUnits Width { get; set; }
        public IUnits Height { get; set; }*/
        public UnitTypes XUnitType { get; set; }
        public UnitTypes YUnitType { get; set; }
        public UnitTypes WidthUnitType { get; set; }
        public UnitTypes HeightUnitType { get; set; }

        public string XUnits { get; set; }
        public string YUnits { get; set; }
        public string WidthUnits { get; set; }
        public string HeightUnits { get; set; }

        public UITypes UIType { get; set; }
        public List<TexturePaths> TexturesPaths { get; set; } = new List<TexturePaths>();

        public override IEntity ToEntity() => new UIItem()
        {
            Name = Name,
            //Position = new Vector3(X, Y, 0.0f)
        };

        private IUnits GetUnits(UnitTypes unitType, string units)
        {
            switch (unitType)
            {
                case UnitTypes.Auto:
                    return Unit.Auto();
                case UnitTypes.Pixels:
                    return Unit.Pixels(int.Parse(units));
                case UnitTypes.Percents:
                    return Unit.Percents(float.Parse(units));
            }

            throw new NotImplementedException("Could not handle unitType " + unitType);
        }

        IRenderable IComponentBuilder<IRenderable>.ToComponent()
        {
            if (!TexturesPaths.Any() && UIType == UITypes.Button)
            {
                var x = GetUnits(XUnitType, XUnits);
                var y = GetUnits(YUnitType, YUnits);
                var width = GetUnits(WidthUnitType, WidthUnits);
                var height = GetUnits(HeightUnitType, HeightUnits);

                return Button.CreateButton(x, y, width, height);
            }

            throw new NotImplementedException();
        }

        public void UpdateFrom(IUIItem uiItem)
        {

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
