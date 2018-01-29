﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using Graphics.Rendering.Buffers;
using OpenTK.Graphics;
using System.Drawing;
using System.Drawing.Imaging;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace Graphics.Rendering.Textures
{
    public struct TextureMapping
    {
        public int MainTextureID { get; set; }
        public int NormalMapID { get; set; }
        public int DiffuseMapID { get; set; }
        public int SpecularMapID { get; set; }
    }
}