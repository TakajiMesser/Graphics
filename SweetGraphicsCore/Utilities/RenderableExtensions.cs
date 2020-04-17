using SpiceEngineCore.Rendering.Batches;
using System;

namespace SweetGraphicsCore.Utilities
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
                case RenderTypes.OpaqueView:
                    return RenderTypes.OpaqueView;
                case RenderTypes.OpaqueText:
                    return RenderTypes.OpaqueText;
                case RenderTypes.TransparentStatic:
                    return RenderTypes.OpaqueStatic;
                case RenderTypes.TransparentAnimated:
                    return RenderTypes.OpaqueAnimated;
                case RenderTypes.TransparentBillboard:
                    return RenderTypes.OpaqueBillboard;
                case RenderTypes.TransparentView:
                    return RenderTypes.OpaqueView;
                case RenderTypes.TransparentText:
                    return RenderTypes.OpaqueText;
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
                case RenderTypes.OpaqueView:
                    return RenderTypes.TransparentView;
                case RenderTypes.OpaqueText:
                    return RenderTypes.TransparentText;
                case RenderTypes.TransparentStatic:
                    return RenderTypes.TransparentStatic;
                case RenderTypes.TransparentAnimated:
                    return RenderTypes.TransparentAnimated;
                case RenderTypes.TransparentBillboard:
                    return RenderTypes.TransparentBillboard;
                case RenderTypes.TransparentView:
                    return RenderTypes.TransparentView;
                case RenderTypes.TransparentText:
                    return RenderTypes.TransparentText;
            }

            throw new ArgumentOutOfRangeException("Could not handle render type " + renderType);
        }
    }
}
