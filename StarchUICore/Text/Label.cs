using OpenTK;
using OpenTK.Graphics;
using SpiceEngineCore.UserInterfaces;
using SpiceEngineCore.Utilities;
using StarchUICore.Attributes.Positions;
using StarchUICore.Attributes.Sizes;
using StarchUICore.Attributes.Units;
using StarchUICore.Text;
using SweetGraphicsCore.Vertices;
using System;
using System.Collections.Generic;

namespace StarchUICore.Views
{
    public class Label : Element
    {
        private IFont _font;
        private string _text;

        public bool WordWrap { get; set; } = true;
        public float FontScale { get; set; } = 1.0f;

        public IFont Font
        {
            get => _font;
            set
            {
                _font = value;
                
                if (_text != null && !Measurement.NeedsMeasuring && !Location.NeedsLocating)
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

                if (_font != null && !Measurement.NeedsMeasuring && !Location.NeedsLocating)
                {
                    CalculateTextVertices();
                }
            }
        }

        public Color4 Color { get; set; } = Color4.White;

        public event EventHandler<TextEventArgs> TextChanged;

        protected override void OnLaidOut(LayoutInfo layoutInfo)
        {
            if (_font != null && _text != null && !Measurement.NeedsMeasuring && !Location.NeedsLocating)
            {
                CalculateTextVertices();
            }
        }

        public override void InvokeLayoutChange()
        {
            if (_font != null && _text != null && !Measurement.NeedsMeasuring && !Location.NeedsLocating)
            {
                CalculateTextVertices();
            }

            base.InvokeLayoutChange();
        }

        protected override LayoutResult OnLayout(LayoutInfo layoutInfo)
        {
            var width = GetMeasuredWidth(layoutInfo.AvailableWidth, layoutInfo.ParentWidth);
            var height = GetMeasuredHeight(layoutInfo.AvailableHeight, layoutInfo.ParentHeight);

            // For AUTO sizing, use the measured dimensions as maximums and shrink to the measured text size
            if (Size.Width is AutoUnits && width.HasValue)
            {
                var textWidth = _text.Length * _font.GlyphWidth;

                if (textWidth > width.Value)
                {
                    var charactersPerLine = width.Value / _font.GlyphWidth;
                    width = (charactersPerLine * _font.GlyphWidth).ClampTop(width.Value);
                }
                else
                {
                    width = textWidth;
                }
            }

            if (Size.Height is AutoUnits && height.HasValue)
            {
                if (WordWrap)
                {
                    if (width.HasValue)
                    {
                        // How many lines will it take?
                        var charactersPerLine = width.Value / _font.GlyphWidth;
                        var nLines = (_text.Length / charactersPerLine).ClampBottom(1);

                        height = (nLines * _font.GlyphHeight).ClampTop(height.Value);
                    }
                }
                else
                {
                    height = _font.GlyphHeight.ClampTop(height.Value);
                }
            }

            var relativeX = GetRelativeX(layoutInfo.RelativeX, layoutInfo.ParentAbsoluteX, layoutInfo.AvailableWidth, layoutInfo.ParentWidth, width);
            var relativeY = GetRelativeY(layoutInfo.RelativeY, layoutInfo.ParentAbsoluteY, layoutInfo.AvailableHeight, layoutInfo.ParentHeight, height);

            var absoluteX = GetAbsoluteX(layoutInfo.ParentAbsoluteX, relativeX, width);
            var absoluteY = GetAbsoluteY(layoutInfo.ParentAbsoluteY, relativeY, height);
            /*var width = layoutInfo.AvailableWidth;
            var height = layoutInfo.AvailableHeight;
            var absoluteX = layoutInfo.ParentAbsoluteX + layoutInfo.RelativeX;
            var absoluteY = layoutInfo.ParentAbsoluteY + layoutInfo.RelativeY;*/

            return new LayoutResult(absoluteX, absoluteY, width, height);
        }

        /*private int MeasureTextWidth(int maximumWidth)
        {
            // Let's say the text width is AUTO. We should measure the total width we need
            var width = _text.Length * _font.GlyphWidth;

            if (width > maximumWidth)
            {
                var charactersPerLine = maximumWidth / _font.GlyphWidth;
                width = charactersPerLine * _font.GlyphWidth;

                for (var i = 0; i < _text.Length; i++)
                {
                    _font.GlyphWidth;
                }
            }

            return width;
        }*/

        private void CalculateTextVertices()
        {
            var uStep = (float)_font.GlyphWidth / _font.Texture.Width;
            var vStep = (float)_font.GlyphHeight / _font.Texture.Height;

            var width = (int)(_font.GlyphWidth * FontScale);
            var height = (int)(_font.GlyphHeight * FontScale);

            var x = Location.X;
            var y = Location.Y;

            var vertices = new List<TextureVertex2D>();

            for (var i = 0; i < _text.Length; i++)
            {
                char character = _text[i];

                var u = (character % _font.GlyphsPerLine) * uStep;
                var v = (character / _font.GlyphsPerLine) * vStep;

                if (WordWrap && width > Measurement.Width)
                {
                    x = Location.X;
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

        protected override MeasuredSize OnMeasure(MeasuredSize availableSize) => new MeasuredSize();

        protected override LocatedPosition OnLocate(LocatedPosition availablePosition) => new LocatedPosition();

        public override void Load() { }
        public override void Draw() { }
    }
}
