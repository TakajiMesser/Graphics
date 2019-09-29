using System;

namespace SpiceEngine.Entities
{
    public interface ITexturedEntity
    {
        bool IsInTextureMode { get; set; }

        void TranslateTexture(float x, float y);
        void RotateTexture(float angle);
        void ScaleTexture(float x, float y);

        event EventHandler<TextureTransformEventArgs> TextureTransformed;
    }
}
