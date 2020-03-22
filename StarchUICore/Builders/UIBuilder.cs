using OpenTK;
using OpenTK.Graphics;
using StarchUICore.Attributes.Positions;
using StarchUICore.Attributes.Sizes;
using StarchUICore.Attributes.Styling;
using StarchUICore.Attributes.Units;
using StarchUICore.Groups;
using StarchUICore.Rendering.Vertices;
using StarchUICore.Views;
using SweetGraphicsCore.Vertices;
using System;
using System.Collections.Generic;

namespace StarchUICore.Builders
{
    public class UIBuilder
    {
        private Position _position;
        private Size _size;
        private Anchor _horizontalAnchor;
        private Anchor _verticalAnchor;
        private Dock _horizontalDock;
        private Dock _verticalDock;
        private float _alpha;
        private Padding _padding;
        private Border _border;
        private Color4 _color;

        private List<IElement> _children = new List<IElement>();

        public UIBuilder WithPosition(IUnits x, IUnits y)
        {
            _position.Offset(x, y);
            return this;
        }

        public UIBuilder WithMinimumPosition(IUnits minimumX, IUnits minimumY)
        {
            _position.OffsetMinimums(minimumX, minimumY);
            return this;
        }

        public UIBuilder WithMaximumPosition(IUnits maximumX, IUnits maximumY)
        {
            _position.OffsetMaximums(maximumX, maximumY);
            return this;
        }

        public UIBuilder WithSize(IUnits width, IUnits height)
        {
            _size.Dimension(width, height);
            return this;
        }

        public UIBuilder WithMinimumSize(IUnits minimumWidth, IUnits minimumHeight)
        {
            _size.DimensionMinimums(minimumWidth, minimumHeight);
            return this;
        }

        public UIBuilder WithMaximumSize(IUnits maximumWidth, IUnits maximumHeight)
        {
            _size.DimensionMaximums(maximumWidth, maximumHeight);
            return this;
        }

        public UIBuilder WithAnchors(Anchor horizontalAnchor, Anchor verticalAnchor)
        {
            _horizontalAnchor = horizontalAnchor;
            _verticalAnchor = verticalAnchor;
            return this;
        }

        public UIBuilder WithDocks(Dock horizontalDock, Dock verticalDock)
        {
            _horizontalDock = horizontalDock;
            _verticalDock = verticalDock;
            return this;
        }

        public UIBuilder WithAlpha(float alpha)
        {
            _alpha = alpha;
            return this;
        }

        public UIBuilder WithPadding(Padding padding)
        {
            _padding = padding;
            return this;
        }

        public UIBuilder WithBorder(Border border)
        {
            _border = border;
            return this;
        }

        public UIBuilder WithColor(Color4 color)
        {
            _color = color;
            return this;
        }

        public UIBuilder WithChildren(params IElement[] elements)
        {
            _children.Clear();
            _children.AddRange(elements);
            return this;
        }

        public View CreateView()
        {
            // TODO - Views should NOT require an explicit vertex set!
            // They should manage their own vertices as their position, size, color, etc. change
            var view = new View();
            ApplyValues(view);

            return view;
        }

        public RowGroup CreateRowGroup()
        {
            var rowGroup = new RowGroup();
            ApplyValues(rowGroup);

            return rowGroup;
        }

        /*public Button CreateButton()
        {
            if (_size.Width is PixelUnits pixelWidth && _size.Height is PixelUnits pixelHeight)
            {
                // TODO - Views should NOT require an explicit vertex set!
                // They should manage their own vertices as their position, size, color, etc. change
                var vertexSet = Rectangle(pixelWidth.Value, pixelHeight.Value, _color);
                var button = new Button(vertexSet);

                ApplyValues(button);

                return button;
            }

            return null;
        }*/

        private void ApplyValues(IElement element)
        {
            element.Position = _position;
            element.Size = _size;
            element.HorizontalAnchor = _horizontalAnchor;
            element.VerticalAnchor = _verticalAnchor;
            element.HorizontalDock = _horizontalDock;
            element.VerticalDock = _verticalDock;
            element.Alpha = _alpha;
            element.Padding = _padding;
            element.Border = _border;

            if (element is View view)
            {
                view.Background.Color = _color;
            }

            if (element is Group group)
            {
                foreach (var child in _children)
                {
                    group.AddChild(child);
                }
            }
        }

        public static Vertex3DSet<ViewQuadVertex> Rectangle(float width, float height, Color4 color)
        {
            var vertices = new List<ViewQuadVertex>
            {
                new ViewQuadVertex(new Vector3(0.0f, 0.0f, 0.0f), 0.0f, new Vector2(width, height), new Vector2(), color, Color4.White, Color4.PaleVioletRed),
                new ViewQuadVertex(new Vector3(0.0f, height, 0.0f), 0.0f, new Vector2(width, height), new Vector2(), color, Color4.White, Color4.PaleVioletRed),
                new ViewQuadVertex(new Vector3(width, height, 0.0f), 0.0f, new Vector2(width, height), new Vector2(), color, Color4.White, Color4.PaleVioletRed),
                new ViewQuadVertex(new Vector3(width, 0.0f, 0.0f), 0.0f, new Vector2(width, height), new Vector2(), color, Color4.White, Color4.PaleVioletRed)
            };

            var triangleIndices = new List<int>{ 2, 1, 0, 3, 2, 0 };

            return new Vertex3DSet<ViewQuadVertex>(vertices, triangleIndices);
        }

        public static Vertex3DSet<ViewVertex> RoundedRectangle(float width, float height, float radius, int nSides, Color4 color)
        {
            // B A
            // C D
            var vertices = new List<ViewVertex>
            {
                new ViewVertex(new Vector3(0.0f, 0.0f, 0.0f), color, Color4.PaleVioletRed),
                new ViewVertex(new Vector3(0.0f, height, 0.0f), color, Color4.PaleVioletRed),
                new ViewVertex(new Vector3(width, height, 0.0f), color, Color4.PaleVioletRed),
                new ViewVertex(new Vector3(width, 0.0f, 0.0f), color, Color4.PaleVioletRed)
            };

            var triangleIndices = new List<int> { 2, 1, 0, 3, 2, 0 };

            var cornerPointA = new Vector2(width - radius, radius);
            var quadrantAPoints = GetRoundedRectangleQuadrant(cornerPointA, radius, nSides, 0);

            foreach (var point in quadrantAPoints)
            {
                vertices.Add(new ViewVertex(new Vector3(point.X, point.Y, 0.0f), color, Color4.PaleVioletRed));
            }

            var cornerPointB = new Vector2(radius, radius);
            var quadrantBPoints = GetRoundedRectangleQuadrant(cornerPointB, radius, nSides, 1);

            foreach (var point in quadrantBPoints)
            {
                vertices.Add(new ViewVertex(new Vector3(point.X, point.Y, 0.0f), color, Color4.PaleVioletRed));
            }

            var cornerPointC = new Vector2(radius, height - radius);
            var quadrantCPoints = GetRoundedRectangleQuadrant(cornerPointC, radius, nSides, 2);

            foreach (var point in quadrantCPoints)
            {
                vertices.Add(new ViewVertex(new Vector3(point.X, point.Y, 0.0f), color, Color4.PaleVioletRed));
            }

            var cornerPointD = new Vector2(width - radius, height - radius);
            var quadrantDPoints = GetRoundedRectangleQuadrant(cornerPointD, radius, nSides, 3);

            foreach (var point in quadrantDPoints)
            {
                vertices.Add(new ViewVertex(new Vector3(point.X, point.Y, 0.0f), color, Color4.PaleVioletRed));
            }

            return new Vertex3DSet<ViewVertex>(vertices, triangleIndices);
        }

        private static IEnumerable<Vector2> GetRoundedRectangleQuadrant(Vector2 cornerPoint, float radius, int nSides, int quadrantIndex)
        {
            var previousPoint = new Vector2();

            for (var i = 0; i < nSides; i++)
            {
                var angle = MathHelper.PiOver2 * i / nSides + quadrantIndex * MathHelper.PiOver2;

                var x = radius * (float)Math.Cos(angle);
                var y = radius * (float)Math.Sin(angle);

                var point = new Vector2(cornerPoint.X + x, cornerPoint.Y - y);

                if (i > 0)
                {
                    yield return cornerPoint;
                    yield return previousPoint;
                    yield return point;
                }

                previousPoint = point;
            }
        }
    }
}
