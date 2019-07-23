using SpiceEngine.Physics.Bodies;
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

        Body3D ToBody();
    }
}
