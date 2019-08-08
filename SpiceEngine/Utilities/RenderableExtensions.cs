using OpenTK;
using OpenTK.Graphics;
using SpiceEngine.Rendering.Batches;
using System;

namespace SpiceEngine.Utilities
{
    public static class RenderableExtensions
    {
        public static RenderTypes Opaque(this RenderTypes renderType)
        {
            switch (renderType)
            {
                case RenderTypes.OpaqueStatic:
                    return RenderTypes.OpaqueStatic;
                case RenderTypes.OpaqueAnimated:
                    return RenderTypes.OpaqueAnimated;
                case RenderTypes.OpaqueBillboard:
                    return RenderTypes.OpaqueBillboard;
                case RenderTypes.TransparentStatic:
                    return RenderTypes.OpaqueStatic;
                case RenderTypes.TransparentAnimated:
                    return RenderTypes.OpaqueAnimated;
                case RenderTypes.TransparentBillboard:
                    return RenderTypes.OpaqueBillboard;
            }

            throw new ArgumentOutOfRangeException("Could not handle render type " + renderType);
        }

        public static RenderTypes Transparent(this RenderTypes renderType)
        {
            switch (renderType)
            {
                case RenderTypes.OpaqueStatic:
                    return RenderTypes.TransparentStatic;
                case RenderTypes.OpaqueAnimated:
                    return RenderTypes.TransparentAnimated;
                case RenderTypes.OpaqueBillboard:
                    return RenderTypes.TransparentBillboard;
                case RenderTypes.TransparentStatic:
                    return RenderTypes.TransparentStatic;
                case RenderTypes.TransparentAnimated:
                    return RenderTypes.TransparentAnimated;
                case RenderTypes.TransparentBillboard:
                    return RenderTypes.TransparentBillboard;
            }

            throw new ArgumentOutOfRangeException("Could not handle render type " + renderType);
        }
    }
}
