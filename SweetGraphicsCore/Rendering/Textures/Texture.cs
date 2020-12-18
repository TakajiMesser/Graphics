using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SpiceEngineCore.Rendering.Textures;
using System;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using Vector4 = SpiceEngineCore.Geometry.Vectors.Vector4;

namespace SweetGraphicsCore.Rendering.Textures
{
    public class Texture : ITexture, IDisposable, IBindable
    {
        private int _maxMipMapLevels;
        private float _maxAnisotrophy;

        public int Handle { get; }

        public TextureTarget Target { get; set; }

        public bool EnableMipMap { get; set; }
        public bool EnableAnisotropy { get; set; }
        //public bool Bindless { get; set; }

        public PixelInternalFormat PixelInternalFormat { get; set; }
        public PixelFormat PixelFormat { get; set; }
        public PixelType PixelType { get; set; }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Depth { get; private set; }

        public TextureMinFilter MinFilter { get; set; }
        public TextureMagFilter MagFilter { get; set; }
        public TextureWrapMode WrapMode { get; set; }
        public Vector4 BorderColor { get; set; }

        public Texture(int width, int height, int depth)
        {
            Handle = GL.GenTexture();

            if (Handle == 0)
            {
                var errorCode = GL.GetError();
                throw new GraphicsException("Failed to generate texture: " + errorCode);//GL.GetShaderInfoLog(_handle));
            }

            Width = width;
            Height = height;
            Depth = depth;
        }

        public void Bind() => GL.BindTexture(Target, Handle);

        public void BindImageTexture(int index)
        {
            bool layered = Target == TextureTarget.Texture2DArray
                || Target == TextureTarget.TextureCubeMap
                || Target == TextureTarget.TextureCubeMapArray
                || Target == TextureTarget.Texture3D;

            GL.BindImageTexture(index, Handle, 0, layered, 0, TextureAccess.WriteOnly, (SizedInternalFormat)PixelInternalFormat);
        }

        public void Unbind() => GL.BindTexture(Target, 0);

        public void GenerateMipMap() => GL.GenerateTextureMipmap(Handle);

        public void Resize(int width, int height, int depth)
        {
            Width = width;
            Height = height;
            Depth = depth;
        }

        public Vector4 ReadPixelColor(int x, int y)
        {
            var bytes = new byte[4];

            if (x <= Width && y <= Height)
            {
                GL.ReadPixels(x, y, 1, 1, PixelFormat, PixelType, bytes);
            }

            return new Vector4(
                (int)bytes[0],
                (int)bytes[1],
                (int)bytes[2],
                (int)bytes[3]
            );
        }

        public void ReserveMemory()
        {
            Specify(IntPtr.Zero);
            SetTextureParameters();
        }

        public void Load(byte[] pixels)
        {
            Specify(pixels);
            SetTextureParameters();
        }

        public void Load(IntPtr pixels)
        {
            Specify(pixels);
            SetTextureParameters();
        }

        public void Unload()
        {
            for (var i = 0; i < _maxMipMapLevels + 1; i++)
            {
                GL.ClearTexImage(Handle, i, PixelFormat, PixelType, IntPtr.Zero);
            }
        }

        public void Specify(byte[] pixels)
        {
            switch (Target)
            {
                case TextureTarget.Texture1D:
                    GL.TexImage1D(Target, 0, PixelInternalFormat, Width, 0, PixelFormat, PixelType, pixels);
                    break;
                case TextureTarget.Texture2D:
                    GL.TexImage2D(Target, 0, PixelInternalFormat, Width, Height, 0, PixelFormat, PixelType, pixels);
                    break;
                case TextureTarget.Texture2DArray:
                    GL.TexImage3D(Target, 0, PixelInternalFormat, Width, Height, Depth, 0, PixelFormat, PixelType, pixels);
                    break;
                case TextureTarget.TextureCubeMap:
                    for (var i = 0; i < Depth; i++)
                    {
                        GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat, Width, Height, 0, PixelFormat, PixelType, pixels);
                    }
                    break;
                case TextureTarget.TextureCubeMapArray:
                    GL.TexStorage3D((TextureTarget3d)Target, _maxMipMapLevels + 1, (SizedInternalFormat)PixelInternalFormat, Width, Height, Depth * 6);
                    break;
                case TextureTarget.Texture3D:
                    GL.TexImage3D(Target, 0, PixelInternalFormat, Width, Height, Depth, 0, PixelFormat, PixelType, pixels);
                    break;
                default:
                    throw new NotImplementedException("Cannot specify texture target " + Target);
            }
        }

        public void Specify(IntPtr pixels)
        {
            switch (Target)
            {
                case TextureTarget.Texture1D:
                    GL.TexImage1D(Target, 0, PixelInternalFormat, Width, 0, PixelFormat, PixelType, pixels);
                    break;
                case TextureTarget.Texture2D:
                    GL.TexImage2D(Target, 0, PixelInternalFormat, Width, Height, 0, PixelFormat, PixelType, pixels);
                    break;
                case TextureTarget.Texture2DArray:
                    GL.TexImage3D(Target, 0, PixelInternalFormat, Width, Height, Depth, 0, PixelFormat, PixelType, pixels);
                    break;
                case TextureTarget.TextureCubeMap:
                    for (var i = 0; i < Depth; i++)
                    {
                        GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat, Width, Height, 0, PixelFormat, PixelType, pixels);
                    }
                    break;
                case TextureTarget.TextureCubeMapArray:
                    GL.TexStorage3D((TextureTarget3d)Target, _maxMipMapLevels + 1, (SizedInternalFormat)PixelInternalFormat, Width, Height, Depth * 6);
                    break;
                case TextureTarget.Texture3D:
                    GL.TexImage3D(Target, 0, PixelInternalFormat, Width, Height, Depth, 0, PixelFormat, PixelType, pixels);
                    break;
                default:
                    throw new NotImplementedException("Cannot specify texture target " + Target);
            }
        }

        public void Specify(IntPtr[] pixels)
        {
            if (pixels.Length != Depth)
            {
                throw new ArgumentException("Pixel array length (" + pixels.Length + ") must match texture depth (" + Depth + ")");
            }

            switch (Target)
            {
                case TextureTarget.Texture2DArray:
                    GL.TexImage3D(Target, 0, PixelInternalFormat, Width, Height, Depth, 0, PixelFormat, PixelType, IntPtr.Zero);
                    for (var i = 0; i < Depth; i++)
                    {
                        GL.TexSubImage3D(Target, 0, 0, 0, i, Width, Height, 1, PixelFormat, PixelType, pixels[i]);
                    }
                    break;
                case TextureTarget.TextureCubeMap:
                    for (var i = 0; i < 6; i++)
                    {
                        GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat, Width, Height, 0, PixelFormat, PixelType, pixels[i]);
                    }
                    break;
                case TextureTarget.Texture3D:
                    GL.TexImage3D(Target, 0, PixelInternalFormat, Width, Height, Depth, 0, PixelFormat, PixelType, IntPtr.Zero);
                    for (var i = 0; i < Depth; i++)
                    {
                        GL.TexSubImage3D(Target, 0, 0, 0, i, Width, Height, 1, PixelFormat, PixelType, pixels[i]);
                    }
                    break;
                default:
                    throw new NotImplementedException("Cannot specify texture target " + Target);
            }
        }

        public void SetTextureParameters()
        {
            if (EnableMipMap)
            {
                _maxMipMapLevels = (int)(Math.Log(Math.Max(Width, Height), 2.0) - 1.0);
                MinFilter = TextureMinFilter.LinearMipmapLinear;

                GL.TexParameter(Target, TextureParameterName.TextureBaseLevel, 0);
                GL.TexParameter(Target, TextureParameterName.TextureMaxLevel, _maxMipMapLevels);

                GL.GenerateMipmap((GenerateMipmapTarget)Target);

                // This appears to require OpenGL v4.5 :(
                //GL.GenerateTextureMipmap(_handle);
            }

            if (EnableAnisotropy)
            {
                _maxAnisotrophy = GL.GetFloat((GetPName)All.MaxTextureMaxAnisotropyExt);
                GL.TexParameter(Target, (TextureParameterName)All.TextureMaxAnisotropyExt, _maxAnisotrophy);
            }

            GL.TexParameter(Target, TextureParameterName.TextureMinFilter, (float)MinFilter);
            GL.TexParameter(Target, TextureParameterName.TextureMagFilter, (float)MinFilter);
            GL.TexParameter(Target, TextureParameterName.TextureWrapS, (int)WrapMode);
            GL.TexParameter(Target, TextureParameterName.TextureWrapT, (int)WrapMode);
            GL.TexParameter(Target, TextureParameterName.TextureWrapR, (int)WrapMode);

            if (BorderColor != Vector4.Zero)
            {
                GL.TexParameter(Target, TextureParameterName.TextureBorderColor,
                    new[] { BorderColor.X, BorderColor.Y, BorderColor.Z, BorderColor.W });
            }
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue && GraphicsContext.CurrentContext != null && !GraphicsContext.CurrentContext.IsDisposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                GL.DeleteTexture(Handle);
                disposedValue = true;
            }
        }

        ~Texture()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
