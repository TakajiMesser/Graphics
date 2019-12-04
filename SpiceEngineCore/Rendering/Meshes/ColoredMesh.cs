using OpenTK;
using OpenTK.Graphics;
using SpiceEngineCore.Rendering.Vertices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngineCore.Rendering.Meshes
{
    public class ColoredMesh<T> : Mesh<T>, IColoredMesh, IDisposable where T : IVertex3D, IColorVertex
    {
        private static Color4 DEFAULT_COLOR = new Color4(1.0f, 1.0f, 1.0f, 0.5f);

        private Color4 _color;

        public Color4 Color
        {
            get => _color;
            set
            {
                if (_color != value)
                {
                    if (value.A >= 1.0f) throw new ArgumentOutOfRangeException("Alpha value of color must be less than 1.0");
                    Update(v => (IVertex3D)((IColorVertex)v).Colored(value));

                    _color = value;
                    ColorChanged?.Invoke(this, new ColorEventArgs(value));
                }
            }
        }

        public event EventHandler<ColorEventArgs> ColorChanged;

        public ColoredMesh(List<T> vertices, List<int> triangleIndices) : base(vertices.Select(v => (T)v.Colored(DEFAULT_COLOR)).ToList(), triangleIndices)
        {
            Alpha = 0.5f;
            _color = DEFAULT_COLOR;
        }
    }
}
