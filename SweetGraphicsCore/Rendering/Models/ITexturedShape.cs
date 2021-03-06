﻿namespace SweetGraphicsCore.Rendering.Models
{
    public interface ITexturedShape
    {
        void TranslateTexture(float x, float y);
        void RotateTexture(float angle);
        void ScaleTexture(float x, float y);
    }
}
