using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace SpiceEngine.Rendering.Matrices
{
    public class TransformEventArgs : EventArgs
    {
        public Transform Transform { get; private set; }

        public TransformEventArgs(Transform transform) => Transform = transform;
    }
}
