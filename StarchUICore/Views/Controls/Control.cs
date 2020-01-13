using SpiceEngineCore.Rendering.Vertices;
using System.Collections.Generic;

namespace StarchUICore.Views.Controls
{
    public abstract class Control : View
    {
        public Control() { }
        public Control(Vertex3DSet<ViewVertex> vertexSet) : base(vertexSet) { }
    }
}
