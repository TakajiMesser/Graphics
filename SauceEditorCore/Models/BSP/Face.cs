﻿using SpiceEngine.Physics.Bodies;
using System;
using System.Collections.Generic;
using System.Text;

namespace SauceEditorCore.Models.BSP
{
    public class Face
    {
        public List<Vector3> Vertices { get; } = new List<Vector3>();
        public Vector3 Normal { get; set; }
        public Vector3 Tangent { get; set; }
    }
}
