using SweetGraphicsCore.Vertices;
using System;
using System.Collections.Generic;

namespace StarchUICore.Text
{
    public class TextEventArgs : EventArgs
    {
        public List<TextureVertex2D> Vertices { get; }

        public TextEventArgs(IEnumerable<TextureVertex2D> vertices) => Vertices = new List<TextureVertex2D>(vertices);
    }
}
