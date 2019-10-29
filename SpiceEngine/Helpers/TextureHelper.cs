using FreeImageAPI;
using OpenTK.Graphics.OpenGL;
using SpiceEngineCore.Rendering.Textures;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace SpiceEngine.Helpers
{
    public static class TextureHelper
    {
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
                    texture.PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.ColorIndex;
                    texture.PixelType = PixelType.Bitmap;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                    texture.PixelInternalFormat = PixelInternalFormat.Rgb8;
                    texture.PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Bgr;
                    texture.PixelType = PixelType.UnsignedByte;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                    texture.PixelInternalFormat = PixelInternalFormat.Rgba;
                    texture.PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Bgra;
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
                    texture.PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.ColorIndex;
                    texture.PixelType = PixelType.Bitmap;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                    texture.PixelInternalFormat = PixelInternalFormat.Rgb8;
                    texture.PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Bgr;
                    texture.PixelType = PixelType.UnsignedByte;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                    texture.PixelInternalFormat = PixelInternalFormat.Rgba;
                    texture.PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Bgra;
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

        public static Texture LoadFromFile(IList<string> paths, TextureTarget target, bool enableMipMap, bool enableAnisotrophy)
        {
            var firstBitmap = new Bitmap(paths.First());
            var data = firstBitmap.LockBits(new Rectangle(0, 0, firstBitmap.Width, firstBitmap.Height), ImageLockMode.ReadOnly, firstBitmap.PixelFormat);

            var texture = new Texture(firstBitmap.Width, firstBitmap.Height, paths.Count)
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
                    texture.PixelInternalFormat = PixelInternalFormat.Rgb8;
                    texture.PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.ColorIndex;
                    texture.PixelType = PixelType.Bitmap;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                    texture.PixelInternalFormat = PixelInternalFormat.Rgb8;
                    texture.PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Bgr;
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
    }
}
