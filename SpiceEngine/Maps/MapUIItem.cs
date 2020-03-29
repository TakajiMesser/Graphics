using OpenTK.Graphics;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.UserInterfaces;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Maps;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.UserInterfaces;
using StarchUICore;
using StarchUICore.Attributes.Positions;
using StarchUICore.Attributes.Sizes;
using StarchUICore.Attributes.Styling;
using StarchUICore.Attributes.Units;
using StarchUICore.Groups;
using StarchUICore.Views;
using SweetGraphicsCore.Rendering.Textures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Maps
{
    public enum UITypes
    {
        View,
        RowGroup,
        ColumnGroup,
        Button,
        Label,
        Panel
    }

    public class MapUIItem : MapEntity<UIItem>, IMapUIItem, ITexturePather
    {
        public string Name { get; set; }
        public List<string> ChildElementNames { get; set; } = new List<string>();

        public string X { get; set; }
        public string Y { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }

        public string MinimumX { get; set; }
        public string MinimumY { get; set; }
        public string MinimumWidth { get; set; }
        public string MinimumHeight { get; set; }

        public string MaximumX { get; set; }
        public string MaximumY { get; set; }
        public string MaximumWidth { get; set; }
        public string MaximumHeight { get; set; }

        public AnchorTypes HorizontalAnchorSelfType { get; set; }
        public AnchorTypes HorizontalAnchorRelativeType { get; set; }
        public string RelativeHorizontalAnchorElementName { get; set; }
        public bool DoesHorizontalAnchorRespectChanges { get; set; }

        public AnchorTypes VerticalAnchorSelfType { get; set; }
        public AnchorTypes VerticalAnchorRelativeType { get; set; }
        public string RelativeVerticalAnchorElementName { get; set; }
        public bool DoesVerticalAnchorRespectChanges { get; set; }

        public string RelativeHorizontalDockElementName { get; set; }
        public bool DoesHorizontalDockRespectChanges { get; set; }

        public string RelativeVerticalDockElementName { get; set; }
        public bool DoesVerticalDockRespectChanges { get; set; }

        public string PaddingLeft { get; set; }
        public string PaddingTop { get; set; }
        public string PaddingRight { get; set; }
        public string PaddingBottom { get; set; }

        public Color4 Color { get; set; }
        public float Alpha { get; set; } = 1.0f;

        public float BorderThickness { get; set; }
        public Color4 BorderColor { get; set; }
        public float CornerXRadius { get; set; }
        public float CornerYRadius { get; set; }

        public UITypes UIType { get; set; }
        public List<TexturePaths> TexturesPaths { get; set; } = new List<TexturePaths>();

        public override IEntity ToEntity() => new UIItem()
        {
            Name = Name,
            //Position = new Vector3(X, Y, 0.0f)
        };

        public void UpdateFrom(IUIItem uiItem)
        {

        }

        private IElement _component;
        private object _componentLock = new object();

        IUIElement IComponentBuilder<IUIElement>.ToComponent()
        {
            lock (_componentLock)
            {
                if (_component == null)
                {
                    _component = CreateComponent();
                }

                return _component;
            }
        }

        IRenderable IComponentBuilder<IRenderable>.ToComponent()
        {
            lock (_componentLock)
            {
                if (_component == null)
                {
                    _component = CreateComponent();
                }

                return _component;
            }
        }

        private IElement CreateComponent()
        {
            if (TexturesPaths.Any()) throw new NotImplementedException();

            switch (UIType)
            {
                case UITypes.View:
                    var view = new View();
                    ApplyValues(view);
                    return view;
                case UITypes.RowGroup:
                    var rowGroup = new RowGroup();
                    ApplyValues(rowGroup);
                    return rowGroup;
                case UITypes.ColumnGroup:
                    var columnGroup = new ColumnGroup();
                    ApplyValues(columnGroup);
                    return columnGroup;
            }

            throw new NotImplementedException();
        }

        private void ApplyValues(Element element)
        {
            element.Name = Name;
            element.Position = ParsePosition();
            element.Size = ParseSize();
            element.HorizontalAnchor = new Anchor(HorizontalAnchorSelfType, HorizontalAnchorRelativeType, DoesHorizontalAnchorRespectChanges);
            element.VerticalAnchor = new Anchor(VerticalAnchorSelfType, VerticalAnchorRelativeType, DoesVerticalAnchorRespectChanges);
            element.HorizontalDock = new Dock(DoesHorizontalDockRespectChanges);
            element.VerticalDock = new Dock(DoesVerticalDockRespectChanges);
            element.Alpha = Alpha;
            element.Padding = ParsePadding();
            element.Border = new Border(BorderThickness, BorderColor, CornerXRadius, CornerYRadius);

            if (element is View view)
            {
                view.Background = new StarchUICore.Layers.Layer
                {
                    Color = Color
                };
            }

            if (element is Group group)
            {
                /*foreach (var child in _children)
                {
                    group.AddChild(child);
                }*/
            }
        }

        private Position ParsePosition()
        {
            var x = !string.IsNullOrEmpty(X) ? ParseUnits(X) : Unit.Auto();
            var y = !string.IsNullOrEmpty(Y) ? ParseUnits(Y) : Unit.Auto();
            var minimumX = !string.IsNullOrEmpty(MinimumX) ? ParseUnits(MinimumX) : Unit.Auto();
            var minimumY = !string.IsNullOrEmpty(MinimumY) ? ParseUnits(MinimumY) : Unit.Auto();
            var maximumX = !string.IsNullOrEmpty(MaximumX) ? ParseUnits(MaximumX) : Unit.Auto();
            var maximumY = !string.IsNullOrEmpty(MaximumY) ? ParseUnits(MaximumY) : Unit.Auto();

            return new Position(x, y, minimumX, minimumY, maximumX, maximumY);
        }

        private Size ParseSize()
        {
            var width = !string.IsNullOrEmpty(Width) ? ParseUnits(Width) : Unit.Auto();
            var height = !string.IsNullOrEmpty(Height) ? ParseUnits(Height) : Unit.Auto();
            var minimumWidth = !string.IsNullOrEmpty(MinimumWidth) ? ParseUnits(MinimumWidth) : Unit.Auto();
            var minimumHeight = !string.IsNullOrEmpty(MinimumHeight) ? ParseUnits(MinimumHeight) : Unit.Auto();
            var maximumWidth = !string.IsNullOrEmpty(MaximumWidth) ? ParseUnits(MaximumWidth) : Unit.Auto();
            var maximumHeight = !string.IsNullOrEmpty(MaximumHeight) ? ParseUnits(MaximumHeight) : Unit.Auto();

            return new Size(width, height, minimumWidth, minimumHeight, maximumWidth, maximumHeight);
        }

        /*private Anchor ParseAnchor()
        {
            
        }

        private Dock ParseDock()
        {

        }*/

        private Padding ParsePadding()
        {
            var left = !string.IsNullOrEmpty(PaddingLeft) ? ParseUnits(PaddingLeft) : Unit.Auto();
            var top = !string.IsNullOrEmpty(PaddingTop) ? ParseUnits(PaddingTop) : Unit.Auto();
            var right = !string.IsNullOrEmpty(PaddingRight) ? ParseUnits(PaddingRight) : Unit.Auto();
            var bottom = !string.IsNullOrEmpty(PaddingBottom) ? ParseUnits(PaddingBottom) : Unit.Auto();

            return new Padding(left, top, right, bottom);
        }

        private IUnits ParseUnits(string value)
        {
            if (value.ToLower() == "auto")
            {
                return Unit.Auto();
            }
            else if (value.EndsWith("%"))
            {
                if (float.TryParse(value.Substring(0, value.Length - 1), out float result))
                {
                    return Unit.Percents(result);
                }
                else
                {
                    throw new ArgumentException("Failed to correctly parse Percent Units");
                }
            }
            else
            {
                if (int.TryParse(value, out int result))
                {
                    return Unit.Pixels(result);
                }
                else
                {
                    throw new ArgumentException("Failed to correctly parse Pixel Units");
                }
            }
        }
    }
}
