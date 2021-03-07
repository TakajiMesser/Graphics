using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Textures;
using System;

namespace SweetGraphicsCore.Rendering.Textures
{
    public class Texture : OpenGLObject, ITexture
    {
        private int _maxMipMapLevels;
        private float _maxAnisotrophy;

        public Texture(IRenderContextProvider contextProvider, int width, int height, int depth) : base(contextProvider)
        {
            Width = width;
            Height = height;
            Depth = depth;
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Depth { get; private set; }

        public TextureTarget Target { get; set; }
        public InternalFormat InternalFormat { get; set; }
        public PixelFormat PixelFormat { get; set; }
        public PixelType PixelType { get; set; }
        public TextureMinFilter MinFilter { get; set; }
        public TextureMagFilter MagFilter { get; set; }
        public TextureWrapMode WrapMode { get; set; }
        public Color4 BorderColor { get; set; }

        public bool EnableMipMap { get; set; }
        public bool EnableAnisotropy { get; set; }
        //public bool Bindless { get; set; }

        public override void Load()
        {
            base.Load();

            Bind();
            ReserveMemory();
            //Unbind();
        }

        public void Load(IntPtr pixels)
        {
            base.Load();

            Bind();
            Specify(pixels);
            SetTextureParameters();
        }

        public void Load(IntPtr[] pixels)
        {
            base.Load();

            Bind();
            Specify(pixels);
            SetTextureParameters();
        }

        protected override int Create() => GL.GenTexture();
        protected override void Delete() => GL.DeleteTexture(Handle);

        public override void Bind() => GL.BindTexture(Target, Handle);
        public override void Unbind() => GL.BindTexture(Target, 0);

        public void BindImageTexture(int index)
        {
            bool layered = Target == TextureTarget.Texture2dArray
                || Target == TextureTarget.TextureCubeMap
                || Target == TextureTarget.TextureCubeMapArray
                || Target == TextureTarget.Texture3d;

            GL.BindImageTexture(index, Handle, 0, layered, 0, BufferAccessARB.WriteOnly, InternalFormat);
        }

        public void GenerateMipMap() => GL.GenerateTextureMipmap(Handle);

        public void Resize(int width, int height, int depth)
        {
            Width = width;
            Height = height;
            Depth = depth;

            Bind();
            ReserveMemory();
        }

        public Color4 ReadPixelColor(int x, int y) => GL.ReadPixels(x, y, 1, 1, PixelFormat, PixelType);

        public void ReserveMemory()
        {
            Specify(IntPtr.Zero);
            SetTextureParameters();
        }

        public void Unload()
        {
            for (var i = 0; i < _maxMipMapLevels + 1; i++)
            {
                GL.ClearTexImage(Handle, i, PixelFormat, PixelType, IntPtr.Zero);
            }
        }

        /*public void Specify(byte[] pixels)
        {
            switch (Target)
            {
                case TextureTarget.Texture1d:
                    GL.TexImage1D(Target, 0, InternalFormat, Width, 0, PixelFormat, PixelType, pixels);
                    break;
                case TextureTarget.Texture2d:
                    GL.TexImage2D(Target, 0, InternalFormat, Width, Height, 0, PixelFormat, PixelType, pixels);
                    break;
                case TextureTarget.Texture2dArray:
                    GL.TexImage3D(Target, 0, InternalFormat, Width, Height, Depth, 0, PixelFormat, PixelType, pixels);
                    break;
                case TextureTarget.TextureCubeMap:
                    for (var i = 0; i < Depth; i++)
                    {
                        GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, InternalFormat, Width, Height, 0, PixelFormat, PixelType, pixels);
                    }
                    break;
                case TextureTarget.TextureCubeMapArray:
                    GL.TexStorage3D(Target, _maxMipMapLevels + 1, InternalFormat, Width, Height, Depth * 6);
                    break;
                case TextureTarget.Texture3d:
                    GL.TexImage3D(Target, 0, InternalFormat, Width, Height, Depth, 0, PixelFormat, PixelType, pixels);
                    break;
                default:
                    throw new NotImplementedException("Cannot specify texture target " + Target);
            }
        }*/

        public void Specify(IntPtr pixels)
        {
            switch (Target)
            {
                case TextureTarget.Texture1d:
                    GL.TexImage1D(Target, 0, InternalFormat, Width, 0, PixelFormat, PixelType, pixels);
                    break;
                case TextureTarget.Texture2d:
                    GL.TexImage2D(Target, 0, InternalFormat, Width, Height, 0, PixelFormat, PixelType, pixels);
                    break;
                case TextureTarget.Texture2dArray:
                    GL.TexImage3D(Target, 0, InternalFormat, Width, Height, Depth, 0, PixelFormat, PixelType, pixels);
                    break;
                case TextureTarget.TextureCubeMap:
                    for (var i = 0; i < Depth; i++)
                    {
                        GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, InternalFormat, Width, Height, 0, PixelFormat, PixelType, pixels);
                    }
                    break;
                case TextureTarget.TextureCubeMapArray:
                    GL.TexStorage3D(Target, _maxMipMapLevels + 1, InternalFormat, Width, Height, Depth * 6);
                    break;
                case TextureTarget.Texture3d:
                    GL.TexImage3D(Target, 0, InternalFormat, Width, Height, Depth, 0, PixelFormat, PixelType, pixels);
                    break;
                default:
                    throw new NotImplementedException("Cannot specify texture target " + Target);
            }
        }

        public void Specify(IntPtr[] pixels)
        {
            if (pixels.Length != Depth) throw new ArgumentException("Pixel array length (" + pixels.Length + ") must match texture depth (" + Depth + ")");

            switch (Target)
            {
                case TextureTarget.Texture2dArray:
                    GL.TexImage3D(Target, 0, InternalFormat, Width, Height, Depth, 0, PixelFormat, PixelType, IntPtr.Zero);
                    for (var i = 0; i < Depth; i++)
                    {
                        GL.TexSubImage3D(Target, 0, 0, 0, i, Width, Height, 1, PixelFormat, PixelType, pixels[i]);
                    }
                    break;
                case TextureTarget.TextureCubeMap:
                    for (var i = 0; i < 6; i++)
                    {
                        GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, InternalFormat, Width, Height, 0, PixelFormat, PixelType, pixels[i]);
                    }
                    break;
                case TextureTarget.Texture3d:
                    GL.TexImage3D(Target, 0, InternalFormat, Width, Height, Depth, 0, PixelFormat, PixelType, IntPtr.Zero);
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

                GL.TexParameteri(Target, TextureParameterName.TextureBaseLevel, 0);
                GL.TexParameteri(Target, TextureParameterName.TextureMaxLevel, _maxMipMapLevels);

                GL.GenerateMipmap(Target);

                // This appears to require OpenGL v4.5 :(
                //GL.GenerateTextureMipmap(_handle);
            }

            if (EnableAnisotropy)
            {
                //_maxAnisotrophy = GL.GetFloatv((GetPName)All.MaxTextureMaxAnisotropyExt);
                //GL.TexParameterf(Target, (TextureParameterName)All.TextureMaxAnisotropyExt, _maxAnisotrophy);
            }

            GL.TexParameterf(Target, TextureParameterName.TextureMinFilter, (float)MinFilter);
            GL.TexParameterf(Target, TextureParameterName.TextureMagFilter, (float)MinFilter);
            GL.TexParameteri(Target, TextureParameterName.TextureWrapS, (int)WrapMode);
            GL.TexParameteri(Target, TextureParameterName.TextureWrapT, (int)WrapMode);
            GL.TexParameteri(Target, TextureParameterName.TextureWrapR, (int)WrapMode);

            if (BorderColor != Color4.Zero)
            {
                GL.TexParameterfv(Target, TextureParameterName.TextureBorderColor, new[] { BorderColor.R, BorderColor.G, BorderColor.B, BorderColor.A });
            }
        }
    }
}
