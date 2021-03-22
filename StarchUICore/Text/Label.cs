using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Textures;
using StarchUICore.Attributes.Sizes;
using StarchUICore.Attributes.Units;
using StarchUICore.Text;
using StarchUICore.Traversal;
using SweetGraphicsCore.Vertices;
using System;
using System.Collections.Generic;

namespace StarchUICore.Views
{
    public class Label : Element
    {
        private IFont _font;
        private string _text;

        public Label(int entityID) : base(entityID) { }

        public bool WordWrap { get; set; } = true;
        public float FontScale { get; set; } = 1.0f;

        public IFont Font
        {
            get => _font;
            set
            {
                _font = value;
                
                if (_text != null && !Measurement.NeedsMeasuring)
                {
                    CalculateTextVertices();
                }
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                _text = value;

                if (_font != null && !Measurement.NeedsMeasuring)
                {
                    CalculateTextVertices();
                }
            }
        }

        public Color4 Color { get; set; } = Color4.White;

        public event EventHandler<TextEventArgs> TextChanged;

        protected override void OnMeasured()
        {
            if (_font != null && _text != null && !Measurement.NeedsMeasuring)
            {
                CalculateTextVertices();
            }
        }

        public override void InvokeMeasurementChanged()
        {
            if (_font != null && _text != null && !Measurement.NeedsMeasuring)
            {
                CalculateTextVertices();
            }

            base.InvokeMeasurementChanged();
        }

        public override IEnumerable<LayoutDependency> GetHeightDependencies()
        {
            if (Size.Height is AutoUnits && WordWrap)
            {
                yield return LayoutDependency.Width(EntityID);
            }

            foreach (var dependency in base.GetHeightDependencies())
            {
                yield return dependency;
            }
        }

        protected override int GetRelativeX(LayoutInfo layoutInfo)
        {
            var anchorWidth = HorizontalAnchor.GetReferenceWidth(layoutInfo);

            // Apply our Position attribute to achieve this element's desired X
            var relativeX = Position.X.ToOffsetPixels(layoutInfo.AvailableValue, anchorWidth);

            // Pass this desired relative X back to the Anchor to reposition it appropriately
            if (!(Position.X is AutoUnits))
            {
                relativeX = HorizontalAnchor.GetAnchorX(relativeX, Measurement, layoutInfo);
            }

            // Apply the Minimum and Maximum constraints last, as these are HARD requirements
            relativeX = Position.MinimumX.ConstrainAsMinimum(relativeX, anchorWidth);
            relativeX = Position.MaximumX.ConstrainAsMaximum(relativeX, anchorWidth);

            return relativeX;
        }

        protected override int GetRelativeY(LayoutInfo layoutInfo)
        {
            var anchorHeight = VerticalAnchor.GetReferenceHeight(layoutInfo);

            // Apply our Position attribute to achieve this element's desired Y
            var relativeY = Position.Y.ToOffsetPixels(layoutInfo.AvailableValue, anchorHeight);

            // Pass this desired relative Y back to the Anchor to reposition it appropriately
            if (!(Position.Y is AutoUnits))
            {
                relativeY = VerticalAnchor.GetAnchorY(relativeY, Measurement, layoutInfo);
            }

            // Apply the Minimum and Maximum constraints last, as these are HARD requirements
            relativeY = Position.MinimumY.ConstrainAsMinimum(relativeY, anchorHeight);
            relativeY = Position.MaximumY.ConstrainAsMaximum(relativeY, anchorHeight);

            return relativeY;
        }

        protected override int GetMeasuredWidth(LayoutInfo layoutInfo)
        {
            var dockWidth = HorizontalDock.GetReferenceWidth(layoutInfo);
            var width = Size.Width.ToDimensionPixels(layoutInfo.AvailableValue, dockWidth);

            // For AUTO sizing, use the measured dimensions as maximums and shrink to the measured text size
            if (Size.Width is AutoUnits)
            {
                /*var textWidth = _text.Length * _font.GlyphWidth;

                if (textWidth > width)
                {
                    var charactersPerLine = width / _font.GlyphWidth;
                    width = (charactersPerLine * _font.GlyphWidth).ClampTop(width);
                }
                else
                {
                    width = textWidth;
                }*/
            }

            width = Size.MinimumWidth.ConstrainAsMinimum(width, dockWidth);
            width = Size.MaximumWidth.ConstrainAsMinimum(width, dockWidth);

            return width;
        }

        protected override int GetMeasuredHeight(LayoutInfo layoutInfo)
        {
            var dockHeight = VerticalDock.GetReferenceHeight(layoutInfo);
            var height = Size.Height.ToDimensionPixels(layoutInfo.AvailableValue, dockHeight);

            // For AUTO sizing, use the measured dimensions as maximums and shrink to the measured text size
            if (Size.Height is AutoUnits)
            {
                /*if (WordWrap)
                {
                    // How many lines will it take?
                    var charactersPerLine = Measurement.Width / _font.GlyphWidth;
                    var nLines = (_text.Length / charactersPerLine).ClampBottom(1);

                    height = (nLines * _font.GlyphHeight).ClampTop(height);
                }
                else
                {
                    height = _font.GlyphHeight.ClampTop(height);
                }*/
            }

            height = Size.MinimumHeight.ConstrainAsMinimum(height, dockHeight);
            height = Size.MaximumHeight.ConstrainAsMinimum(height, dockHeight);

            return height;
        }

        private void CalculateTextVertices()
        {
            var uStep = (float)_font.GlyphWidth / _font.Texture.Width;
            var vStep = (float)_font.GlyphHeight / _font.Texture.Height;

            var width = (int)(_font.GlyphWidth * FontScale);
            var height = (int)(_font.GlyphHeight * FontScale);

            var x = Measurement.X;
            var y = Measurement.Y;

            var vertices = new List<TextureVertex2D>();

            for (var i = 0; i < _text.Length; i++)
            {
                char character = _text[i];

                var u = (character % _font.GlyphsPerLine) * uStep;
                var v = (character / _font.GlyphsPerLine) * vStep;

                if (WordWrap && width > Measurement.Width)
                {
                    x = Measurement.X;
                    y += height + _font.YSpacing;
                }

                var ptr = new Vector2(x + width, y + height);
                var ptl = new Vector2(x, y + height);
                var pbl = new Vector2(x, y);
                var pbr = new Vector2(x + width, y);

                var tbr = new Vector2(u + uStep, v);
                var tbl = new Vector2(u, v);
                var ttl = new Vector2(u, v + vStep);
                var ttr = new Vector2(u + uStep, v + vStep);

                vertices.AddRange(new List<TextureVertex2D>()
                {
                    new TextureVertex2D(pbr, tbr),
                    new TextureVertex2D(pbl, tbl),
                    new TextureVertex2D(ptl, ttl),
                    new TextureVertex2D(ptr, ttr)
                });

                /*vertices.Add(new TextureVertex2D(new Vector2(x + width, y + height), new Vector2(u + uStep, v)));
                vertices.Add(new TextureVertex2D(new Vector2(x, y + height), new Vector2(u, v)));
                vertices.Add(new TextureVertex2D(new Vector2(x, y), new Vector2(u, v + vStep)));
                vertices.Add(new TextureVertex2D(new Vector2(x + width, y), new Vector2(u + uStep, v + vStep)));*/

                x += _font.XSpacing + 20;
            }

            TextChanged?.Invoke(this, new TextEventArgs(vertices));
        }

        public override void Load(IRenderContextProvider contextProvider) { }
        public override void Draw() { }
    }
}
