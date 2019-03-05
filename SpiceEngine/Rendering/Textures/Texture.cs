using FreeImageAPI;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SpiceEngine.Rendering.Buffers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace SpiceEngine.Rendering.Textures
{
    public class Texture : IDisposable, IBindable
    {
        public int Handle => _handle;
        private readonly int _handle;

        private int _maxMipMapLevels;
        private float _maxAnisotrophy;

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
            _handle = GL.GenTexture();

            Width = width;
            Height = height;
            Depth = depth;
        }

        public void Bind()
        {
            GL.BindTexture(Target, _handle);
        }

        public void BindImageTexture(int index)
        {
            bool layered = Target == TextureTarget.Texture2DArray
                || Target == TextureTarget.TextureCubeMap
                || Target == TextureTarget.TextureCubeMapArray
                || Target == TextureTarget.Texture3D;

            GL.BindImageTexture(index, _handle, 0, layered, 0, TextureAccess.WriteOnly, (SizedInternalFormat)PixelInternalFormat);
        }

        public void Unbind()
        {
            GL.BindTexture(Target, 0);
        }

        public void GenerateMipMap()
        {
            GL.GenerateTextureMipmap(_handle);
        }

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

            return new Vector4()
            {
                X = (int)bytes[0],
                Y = (int)bytes[1],
                Z = (int)bytes[2],
                W = (int)bytes[3]
            };
        }

        public void ReserveMemory()
        {
            Specify(IntPtr.Zero);
            SetTextureParameters();
        }

        public void Load(Byte[] pixels)
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
                GL.ClearTexImage(_handle, i, PixelFormat, PixelType, IntPtr.Zero);
            }
        }

        private void Specify(Byte[] pixels)
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

        private void Specify(IntPtr pixels)
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

        private void Specify(IntPtr[] pixels)
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

        private void SetTextureParameters()
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
            GL.TexParameter(Target, TextureParameterName.TextureWrapS, (float)WrapMode);
            GL.TexParameter(Target, TextureParameterName.TextureWrapT, (float)WrapMode);
            GL.TexParameter(Target, TextureParameterName.TextureWrapR, (float)WrapMode);

            if (BorderColor != Vector4.Zero)
            {
                GL.TexParameter(Target, TextureParameterName.TextureBorderColor,
                    new[] { BorderColor.X, BorderColor.Y, BorderColor.Z, BorderColor.W });
            }
        }

        public static Texture LoadFromFile(string path, bool enableMipMap, bool enableAnisotrophy)
        {
            switch (Path.GetExtension(path))
            {
                case ".jpg":
                case ".png":
                    return LoadFromBitmap(path, enableMipMap, enableAnisotrophy);
                //case ".tga":
                    //return LoadFromTGA(path, enableMipMap, enableAnisotrophy);
                default:
                    return null;
                    /*var image = DevILSharp.Image.Load(path);
                    var texture = new Texture(image.Width, image.Height, 1)
                    {
                        Target = TextureTarget.Texture2D,
                        MinFilter = TextureMinFilter.Linear,
                        MagFilter = TextureMagFilter.Linear,
                        WrapMode = TextureWrapMode.Repeat,
                        EnableMipMap = enableMipMap,
                        EnableAnisotropy = enableAnisotrophy
                    };

                    switch (image.ChannelFormat)
                    {
                        case ChannelFormat.RGB:
                            texture.PixelInternalFormat = PixelInternalFormat.Rgb8;
                            break;
                    }

                    switch (image.ChannelType)
                    {
                        //case ChannelType.
                    }

                    texture.Bind();
                    texture.Load(image.Data);

                    image.Dispose();

                    return texture;*/
            }
        }

        public static Texture Load(Bitmap bitmap, bool enableMipMap, bool enableAnisotrophy, TextureMinFilter minFilter = TextureMinFilter.Linear, TextureMagFilter magFilter = TextureMagFilter.Linear)
        {
            var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var texture = new Texture(bitmap.Width, bitmap.Height, 1)
            {
                Target = TextureTarget.Texture2D,
                MinFilter = minFilter,
                MagFilter = magFilter,
                WrapMode = TextureWrapMode.Repeat,
                EnableMipMap = enableMipMap,
                EnableAnisotropy = enableAnisotrophy
            };

            switch (data.PixelFormat)
            {
                case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
                    texture.PixelInternalFormat = PixelInternalFormat.Rgb8;
                    texture.PixelFormat = PixelFormat.ColorIndex;
                    texture.PixelType = PixelType.Bitmap;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                    texture.PixelInternalFormat = PixelInternalFormat.Rgb8;
                    texture.PixelFormat = PixelFormat.Bgr;
                    texture.PixelType = PixelType.UnsignedByte;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                    texture.PixelInternalFormat = PixelInternalFormat.Rgba;
                    texture.PixelFormat = PixelFormat.Bgra;
                    texture.PixelType = PixelType.UnsignedByte;
                    break;
            }

            texture.Bind();
            texture.Load(data.Scan0);

            bitmap.UnlockBits(data);
            bitmap.Dispose();

            return texture;
        }

        public static Texture LoadFromBitmap(string path, bool enableMipMap, bool enableAnisotrophy, TextureMinFilter minFilter = TextureMinFilter.Linear, TextureMagFilter magFilter = TextureMagFilter.Linear)
        {
            var bitmap = new Bitmap(path);
            var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var texture = new Texture(bitmap.Width, bitmap.Height, 1)
            {
                Target = TextureTarget.Texture2D,
                MinFilter = minFilter,
                MagFilter = magFilter,
                WrapMode = TextureWrapMode.Repeat,
                EnableMipMap = enableMipMap,
                EnableAnisotropy = enableAnisotrophy
            };

            switch (data.PixelFormat)
            {
                case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
                    texture.PixelInternalFormat = PixelInternalFormat.Rgb8;
                    texture.PixelFormat = PixelFormat.ColorIndex;
                    texture.PixelType = PixelType.Bitmap;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                    texture.PixelInternalFormat = PixelInternalFormat.Rgb8;
                    texture.PixelFormat = PixelFormat.Bgr;
                    texture.PixelType = PixelType.UnsignedByte;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                    texture.PixelInternalFormat = PixelInternalFormat.Rgba;
                    texture.PixelFormat = PixelFormat.Bgra;
                    texture.PixelType = PixelType.UnsignedByte;
                    break;
            }

            texture.Bind();
            texture.Load(data.Scan0);

            bitmap.UnlockBits(data);
            bitmap.Dispose();

            return texture;
        }

        public static Texture LoadFromTGA(string path, bool enableMipMap, bool enableAnisotrophy)
        {
            var bitmap = FreeImage.LoadEx(path, FREE_IMAGE_LOAD_FLAGS.TARGA_LOAD_RGB888);
            var scan = new Scanline<byte>(bitmap);

            var texture = new Texture((int)FreeImage.GetWidth(bitmap), (int)FreeImage.GetHeight(bitmap), 1)
            {
                Target = TextureTarget.Texture2D,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Repeat,
                EnableMipMap = enableMipMap,
                EnableAnisotropy = enableAnisotrophy
            };

            /*switch (data.PixelFormat)
            {
                case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
                    texture.PixelInternalFormat = PixelInternalFormat.Rgb8;
                    texture.PixelFormat = PixelFormat.ColorIndex;
                    texture.PixelType = PixelType.Bitmap;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                    texture.PixelInternalFormat = PixelInternalFormat.Rgb8;
                    texture.PixelFormat = PixelFormat.Bgr;
                    texture.PixelType = PixelType.UnsignedByte;
                    break;
            }*/

            texture.Bind();
            texture.Load(scan.Data);

            //bitmap.UnlockBits(data);
            //bitmap.Dispose();

            return texture;

            /*using (var reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                var image = new TgaLib.TgaImage(reader);

                var texture = new Texture(image.Header.Width, image.Header.Height, 1)
                {
                    Target = TextureTarget.Texture2D,
                    MinFilter = TextureMinFilter.Linear,
                    MagFilter = TextureMagFilter.Linear,
                    WrapMode = TextureWrapMode.Repeat,
                    EnableMipMap = enableMipMap,
                    EnableAnisotropy = enableAnisotrophy
                };

                //image.Header.ImageType

                /*switch (image.PixelFormat)
                {
                    case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
                        texture.PixelInternalFormat = PixelInternalFormat.Rgb8;
                        texture.PixelFormat = PixelFormat.ColorIndex;
                        texture.PixelType = PixelType.Bitmap;
                        break;
                    case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                        texture.PixelInternalFormat = PixelInternalFormat.Rgb8;
                        texture.PixelFormat = PixelFormat.Bgr;
                        texture.PixelType = PixelType.UnsignedByte;
                        break;
                }*/

                /*texture.Bind();
                texture.Load(image.ImageBytes);

                return texture;
            }*/
        }

        public static Texture LoadFromFile(List<string> paths, TextureTarget target, bool enableMipMap, bool enableAnisotrophy)
        {
            var firstBitmap = new Bitmap(paths.First());
            var data = firstBitmap.LockBits(new Rectangle(0, 0, firstBitmap.Width, firstBitmap.Height), ImageLockMode.ReadOnly, firstBitmap.PixelFormat);

            var texture = new Texture(firstBitmap.Width, firstBitmap.Height, paths.Count)
            {
                Target = target,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.ClampToEdge,
                EnableMipMap = enableMipMap,
                EnableAnisotropy = enableAnisotrophy
            };

            switch (data.PixelFormat)
            {
                case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
                    texture.PixelInternalFormat = PixelInternalFormat.Rgb8;
                    texture.PixelFormat = PixelFormat.ColorIndex;
                    texture.PixelType = PixelType.Bitmap;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                    texture.PixelInternalFormat = PixelInternalFormat.Rgb8;
                    texture.PixelFormat = PixelFormat.Bgr;
                    texture.PixelType = PixelType.UnsignedByte;
                    break;
            }

            var bitmaps = new List<Bitmap> { firstBitmap };
            var imageData = new List<BitmapData> { data };

            for (var i = 1; i < paths.Count; i++)
            {
                var bitmap = new Bitmap(paths[i]);
                imageData.Add(bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat));

                bitmaps.Add(bitmap);
            }

            texture.Bind();
            texture.Specify(imageData.Select(d => d.Scan0).ToArray());
            texture.SetTextureParameters();

            for (var i = 0; i < bitmaps.Count; i++)
            {
                bitmaps[i].UnlockBits(imageData[i]);
                bitmaps[i].Dispose();
            }

            return texture;
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

                GL.DeleteTexture(_handle);
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
