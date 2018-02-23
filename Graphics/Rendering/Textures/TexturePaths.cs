using System;
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
    public class TexturePaths
    {
        public string DiffuseMapFilePath { get; set; }
        public string NormalMapFilePath { get; set; }
        public string SpecularMapFilePath { get; set; }
        public string ParallaxMapFilePath { get; set; }

        public TexturePaths() { }
        public TexturePaths(Assimp.Material material)
        {
            DiffuseMapFilePath = material.TextureDiffuse.FilePath;
            NormalMapFilePath = material.TextureNormal.FilePath;
            SpecularMapFilePath = material.TextureSpecular.FilePath;
        }

        public TextureMapping ToTextureMapping(TextureManager textureManager)
        {
            var textureMapping = new TextureMapping();

            if (!string.IsNullOrEmpty(DiffuseMapFilePath))
            {
                textureMapping.DiffuseMapID = textureManager.AddTexture(DiffuseMapFilePath);
            }

            if (!string.IsNullOrEmpty(NormalMapFilePath))
            {
                textureMapping.NormalMapID = textureManager.AddTexture(NormalMapFilePath);
            }

            if (!string.IsNullOrEmpty(SpecularMapFilePath))
            {
                textureMapping.SpecularMapID = textureManager.AddTexture(SpecularMapFilePath);
            }

            if (!string.IsNullOrEmpty(ParallaxMapFilePath))
            {
                textureMapping.ParallaxMapID = textureManager.AddTexture(ParallaxMapFilePath);
            }

            return textureMapping;
        }
    }
}
