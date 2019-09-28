using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SpiceEngine.Entities;
using SpiceEngine.Entities.Cameras;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Entities.Volumes;
using SpiceEngine.Maps;
using SpiceEngine.Outputs;
using SpiceEngine.Rendering.Batches;
using SpiceEngine.Rendering.PostProcessing;
using SpiceEngine.Rendering.Processing;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SpiceEngine.Rendering
{
    public interface IGridRenderer
    {
        bool RenderGrid { get; set; }

        void RotateGrid(float pitch, float yaw, float roll);
        void SetGridUnit(float unit);
        void SetGridLineThickness(float thickness);
        void SetGridUnitColor(Color4 color);
        void SetGridAxisColor(Color4 color);
        void SetGrid5Color(Color4 color);
        void SetGrid10Color(Color4 color);
    }
}
