using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Textures;
using SweetGraphicsCore.Buffers;
using SweetGraphicsCore.Rendering.Textures;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace SweetGraphicsCore.Helpers
{
    public static class TextureHelper
    {
        public static Texture LoadFromFile(IRenderContextProvider contextProvider, string path, bool enableMipMap, bool enableAnisotrophy)
        {
            switch (Path.GetExtension(path))
            {
                case ".jpg":
                case ".png":
                    return LoadFromBitmap(contextProvider, path, enableMipMap, enableAnisotrophy);
                //case ".tga":
                //return LoadFromTGA(path, enableMipMap, enableAnisotrophy);
                default:
                    return null;
                    /*var image = DevILSharp.Image.Load(path);
                    var texture = new Texture(image.Width, image.Height, 1)
                    {
                        Target = TextureTarget.Texture2d,
                        MinFilter = TextureMinFilter.Linear,
                        MagFilter = TextureMagFilter.Linear,
                        WrapMode = TextureWrapMode.Repeat,
                        EnableMipMap = enableMipMap,
                        EnableAnisotropy = enableAnisotrophy
                    };

                    switch (image.ChannelFormat)
                    {
                        case ChannelFormat.RGB:
                            texture.InternalFormat = InternalFormat.Rgb8;
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

        public static Texture Load(IRenderContextProvider contextProvider, Bitmap bitmap, bool enableMipMap, bool enableAnisotrophy, TextureMinFilter minFilter = TextureMinFilter.Linear, TextureMagFilter magFilter = TextureMagFilter.Linear)
        {
            var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var texture = new Texture(contextProvider, bitmap.Width, bitmap.Height, 1)
            {
                Target = TextureTarget.Texture2d,
                MinFilter = minFilter,
                MagFilter = magFilter,
                WrapMode = TextureWrapMode.Repeat,
                EnableMipMap = enableMipMap,
                EnableAnisotropy = enableAnisotrophy
            };

            switch (data.PixelFormat)
            {
                case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
                    texture.InternalFormat = InternalFormat.Rgb8;
                    texture.PixelFormat = SpiceEngine.GLFWBindings.GLEnums.PixelFormat.ColorIndex;
                    texture.PixelType = PixelType.Bitmap;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                    texture.InternalFormat = InternalFormat.Rgb8;
                    texture.PixelFormat = SpiceEngine.GLFWBindings.GLEnums.PixelFormat.Bgr;
                    texture.PixelType = PixelType.UnsignedByte;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                    texture.InternalFormat = InternalFormat.Rgba;
                    texture.PixelFormat = SpiceEngine.GLFWBindings.GLEnums.PixelFormat.Bgra;
                    texture.PixelType = PixelType.UnsignedByte;
                    break;
            }

            texture.Load(data.Scan0);

            bitmap.UnlockBits(data);
            bitmap.Dispose();

            return texture;
        }

        public static Texture LoadFromBitmap(IRenderContextProvider contextProvider, string path, bool enableMipMap, bool enableAnisotrophy, TextureMinFilter minFilter = TextureMinFilter.Linear, TextureMagFilter magFilter = TextureMagFilter.Linear)
        {
            var bitmap = new Bitmap(path);
            var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var texture = new Texture(contextProvider, bitmap.Width, bitmap.Height, 1)
            {
                Target = TextureTarget.Texture2d,
                MinFilter = minFilter,
                MagFilter = magFilter,
                WrapMode = TextureWrapMode.Repeat,
                EnableMipMap = enableMipMap,
                EnableAnisotropy = enableAnisotrophy
            };

            switch (data.PixelFormat)
            {
                case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
                    texture.InternalFormat = InternalFormat.Rgb8;
                    texture.PixelFormat = SpiceEngine.GLFWBindings.GLEnums.PixelFormat.ColorIndex;
                    texture.PixelType = PixelType.Bitmap;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                    texture.InternalFormat = InternalFormat.Rgb8;
                    texture.PixelFormat = SpiceEngine.GLFWBindings.GLEnums.PixelFormat.Bgr;
                    texture.PixelType = PixelType.UnsignedByte;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                    texture.InternalFormat = InternalFormat.Rgba;
                    texture.PixelFormat = SpiceEngine.GLFWBindings.GLEnums.PixelFormat.Bgra;
                    texture.PixelType = PixelType.UnsignedByte;
                    break;
            }

            texture.Load(data.Scan0);

            bitmap.UnlockBits(data);
            bitmap.Dispose();

            return texture;
        }

        public static Texture LoadFromTGA(string path, bool enableMipMap, bool enableAnisotrophy)
        {
            return null;
            /*var bitmap = FreeImage.LoadEx(path, FREE_IMAGE_LOAD_FLAGS.TARGA_LOAD_RGB888);
            var scan = new Scanline<byte>(bitmap);

            var texture = new Texture((int)FreeImage.GetWidth(bitmap), (int)FreeImage.GetHeight(bitmap), 1)
            {
                Target = TextureTarget.Texture2d,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Repeat,
                EnableMipMap = enableMipMap,
                EnableAnisotropy = enableAnisotrophy
            };

            /*switch (data.PixelFormat)
            {
                case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
                    texture.InternalFormat = InternalFormat.Rgb8;
                    texture.PixelFormat = PixelFormat.ColorIndex;
                    texture.PixelType = PixelType.Bitmap;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                    texture.InternalFormat = InternalFormat.Rgb8;
                    texture.PixelFormat = PixelFormat.Bgr;
                    texture.PixelType = PixelType.UnsignedByte;
                    break;
            }*

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
                    Target = TextureTarget.Texture2d,
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
                        texture.InternalFormat = InternalFormat.Rgb8;
                        texture.PixelFormat = PixelFormat.ColorIndex;
                        texture.PixelType = PixelType.Bitmap;
                        break;
                    case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                        texture.InternalFormat = InternalFormat.Rgb8;
                        texture.PixelFormat = PixelFormat.Bgr;
                        texture.PixelType = PixelType.UnsignedByte;
                        break;
                }*/

            /*texture.Bind();
            texture.Load(image.ImageBytes);

            return texture;
        }*/
        }

        public static Texture LoadFromFile(IRenderContextProvider contextProvider, IList<string> paths, TextureTarget target, bool enableMipMap, bool enableAnisotrophy)
        {
            var firstBitmap = new Bitmap(paths.First());
            var data = firstBitmap.LockBits(new Rectangle(0, 0, firstBitmap.Width, firstBitmap.Height), ImageLockMode.ReadOnly, firstBitmap.PixelFormat);

            var texture = new Texture(contextProvider, firstBitmap.Width, firstBitmap.Height, paths.Count)
            {
                Target = target,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Repeat,
                EnableMipMap = enableMipMap,
                EnableAnisotropy = enableAnisotrophy
            };

            switch (data.PixelFormat)
            {
                case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
                    texture.InternalFormat = InternalFormat.Rgb8;
                    texture.PixelFormat = SpiceEngine.GLFWBindings.GLEnums.PixelFormat.ColorIndex;
                    texture.PixelType = PixelType.Bitmap;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                    texture.InternalFormat = InternalFormat.Rgb8;
                    texture.PixelFormat = SpiceEngine.GLFWBindings.GLEnums.PixelFormat.Bgr;
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

            texture.Load(imageData.Select(d => d.Scan0).ToArray());

            for (var i = 0; i < bitmaps.Count; i++)
            {
                bitmaps[i].UnlockBits(imageData[i]);
                bitmaps[i].Dispose();
            }

            return texture;
        }

        public static void SaveToFile(IRenderContextProvider contextProvider, string filePath, ITexture texture)
        {
            // Create a frame buffer for our texture
            var frameBuffer = new FrameBuffer(contextProvider);
            frameBuffer.Add(FramebufferAttachment.ColorAttachment0, texture);
            frameBuffer.Load();

            // Bind the frame buffer for reading
            frameBuffer.BindAndRead(ReadBufferMode.ColorAttachment0);
            GL.PixelStorei(PixelStoreParameter.UnpackAlignment, 1);

            // Create bitmap to transfer texture pixels over to
            var bitmap = new Bitmap(texture.Width, texture.Height);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, texture.Width, texture.Height), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.ReadPixels(0, 0, texture.Width, texture.Height, SpiceEngine.GLFWBindings.GLEnums.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            GL.Finish();

            bitmap.UnlockBits(data);
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);

            bitmap.Save(filePath, ImageFormat.Png);
            bitmap.Dispose();

            /*var color = texture.ReadPixelColor((int)point.X, (int)point.Y);
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
            };*/
        }
    }
}
