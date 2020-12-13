using System;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SpiceEngineCore.Rendering.Textures
{
    public interface ITexture
    {
        int Handle { get; }
        //TextureTarget Target { get; set; }

        bool EnableMipMap { get; set; }
        bool EnableAnisotropy { get; set; }
        //public bool Bindless { get; set; }

        //PixelInternalFormat PixelInternalFormat { get; set; }
        //PixelFormat PixelFormat { get; set; }
        //PixelType PixelType { get; set; }

        int Width { get; }
        int Height { get; }
        int Depth { get; }

        //TextureMinFilter MinFilter { get; set; }
        //TextureMagFilter MagFilter { get; set; }
        //TextureWrapMode WrapMode { get; set; }
        Vector4 BorderColor { get; set; }

        void Bind();
        void BindImageTexture(int index);
        void Unbind();
        void GenerateMipMap();
        void Resize(int width, int height, int depth);
        Vector4 ReadPixelColor(int x, int y);

        void ReserveMemory();
        void Load(byte[] pixels);
        void Load(IntPtr pixels);
        void Unload();
    }
}
