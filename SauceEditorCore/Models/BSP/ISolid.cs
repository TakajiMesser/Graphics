using SpiceEngine.Physics.Bodies;
using SpiceEngine.Physics.Shapes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SauceEditorCore.Models.BSP
{
    public enum SolidTypes
    {
        Additive,
        Subtractive
    }

    public interface ISolid
    {
        SolidTypes SolidType { get; }

        Shape3D ToShape();
    }
}
