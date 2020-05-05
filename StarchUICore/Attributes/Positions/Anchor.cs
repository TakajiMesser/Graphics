using SpiceEngineCore.Utilities;
using StarchUICore.Attributes.Sizes;
using System;

namespace StarchUICore.Attributes.Positions
{
    public enum AnchorTypes
    {
        Start,
        Center,
        End
    }

    public struct Anchor
    {
        public Anchor(AnchorTypes selfAnchorType, AnchorTypes relativeAnchorType, bool doesRespectChanges = true, IElement relativeElement = null)
        {
            SelfAnchorType = selfAnchorType;
            RelativeAnchorType = relativeAnchorType;
            RelativeElement = relativeElement;
            DoesRespectChanges = doesRespectChanges;
        }

        public AnchorTypes SelfAnchorType { get; }
        public AnchorTypes RelativeAnchorType { get; }
        public bool DoesRespectChanges { get; }
        public IElement RelativeElement { get; }

        public Anchor Attached(IElement relativeElement) => new Anchor(SelfAnchorType, RelativeAnchorType, DoesRespectChanges, relativeElement);
        public Anchor Detached() => new Anchor(SelfAnchorType, RelativeAnchorType, DoesRespectChanges, null);

        public int GetReferenceWidth(LayoutInfo layoutInfo)
        {
            if (RelativeElement != null)
            {
                return RelativeElement.Measurement.Width;
            }
            else
            {
                return layoutInfo.ParentWidth;
            }
        }

        public int GetReferenceHeight(LayoutInfo layoutInfo)
        {
            if (RelativeElement != null)
            {
                return RelativeElement.Measurement.Height;
            }
            else
            {
                return layoutInfo.ParentHeight;
            }
        }

        public int GetAnchorX(int desiredX, Measurement measurement, LayoutInfo layoutInfo)
        {
            var anchorRelativeX = GetAnchorXRelativeToParent(layoutInfo);

            if (anchorRelativeX > desiredX)
            {
                desiredX = anchorRelativeX;
            }
            //desiredX += anchorRelativeX.Value;

            var leftX = 0;
            var rightX = 0;

            switch (SelfAnchorType)
            {
                case AnchorTypes.Start:
                    leftX = desiredX;
                    rightX = desiredX + measurement.Width;
                    break;
                case AnchorTypes.Center:
                    var halfWidth = (int)Math.Round(measurement.Width / 2.0f);
                    leftX = desiredX - halfWidth;
                    rightX = desiredX + halfWidth;
                    break;
                case AnchorTypes.End:
                    leftX = desiredX - measurement.Width;
                    rightX = desiredX;
                    break;
            }

            var minParentX = layoutInfo.AvailableValue;
            var maxParentX = layoutInfo.ParentWidth;
            desiredX = leftX.ClampBottom(minParentX);

            if (rightX > maxParentX)
            {
                // In this case, we need to try to push as much as we can left by however much is "jutting out" on the right
                var excessX = rightX - maxParentX;
                desiredX = (leftX - excessX).ClampBottom(minParentX);
            }

            return desiredX;
        }

        public int GetAnchorY(int desiredY, Measurement measurement, LayoutInfo layoutInfo)
        {
            var anchorRelativeY = GetAnchorYRelativeToParent(layoutInfo);

            if (anchorRelativeY > desiredY)
            {
                desiredY = anchorRelativeY;
            }
            //desiredY += anchorRelativeY.Value;

            var topY = 0;
            var bottomY = 0;

            switch (SelfAnchorType)
            {
                case AnchorTypes.Start:
                    topY = desiredY;
                    bottomY = desiredY + measurement.Height;
                    break;
                case AnchorTypes.Center:
                    var halfHeight = (int)Math.Round(measurement.Height / 2.0f);
                    topY = desiredY - halfHeight;
                    bottomY = desiredY + halfHeight;
                    break;
                case AnchorTypes.End:
                    topY = desiredY - measurement.Height;
                    bottomY = desiredY;
                    break;
            }

            var minParentY = layoutInfo.AvailableValue;
            var maxParentY = layoutInfo.ParentHeight;
            desiredY = topY.ClampBottom(minParentY);

            if (bottomY > maxParentY)
            {
                // In this case, we need to try to push as much as we can up by however much is "jutting out" on the bottom
                var excessX = bottomY - maxParentY;
                desiredY = (topY - excessX).ClampBottom(minParentY);
            }

            return desiredY;
        }

        private int GetAnchorXRelativeToParent(LayoutInfo layoutInfo)
        {
            if (RelativeElement != null)
            {
                // We need to translate the anchor's absolute X to be relative to this element's parent, then we need to interpret our Element's desired X as being relative to its anchor, so that we can convert it to be relative from the parent
                switch (RelativeAnchorType)
                {
                    case AnchorTypes.Start:
                        return RelativeElement.Measurement.X - layoutInfo.ParentX;
                    case AnchorTypes.Center:
                        return RelativeElement.Measurement.X + (RelativeElement.Measurement.Width / 2) - layoutInfo.ParentX;
                    case AnchorTypes.End:
                        return RelativeElement.Measurement.X + RelativeElement.Measurement.Width - layoutInfo.ParentX;
                }
            }
            else
            {
                switch (RelativeAnchorType)
                {
                    case AnchorTypes.Center:
                        return layoutInfo.ParentWidth / 2;
                    case AnchorTypes.End:
                        return layoutInfo.ParentWidth;
                }
            }

            return 0;
        }

        private int GetAnchorYRelativeToParent(LayoutInfo layoutInfo)
        {
            if (RelativeElement != null)
            {
                // We need to translate the anchor's absolute X to be relative to this element's parent, then we need to interpret our Element's desired X as being relative to its anchor, so that we can convert it to be relative from the parent
                switch (RelativeAnchorType)
                {
                    case AnchorTypes.Start:
                        return RelativeElement.Measurement.Y - layoutInfo.ParentY;
                    case AnchorTypes.Center:
                        return RelativeElement.Measurement.Y + (RelativeElement.Measurement.Height / 2) - layoutInfo.ParentY;
                    case AnchorTypes.End:
                        return RelativeElement.Measurement.Y + RelativeElement.Measurement.Height - layoutInfo.ParentY;
                }
            }
            else
            {
                switch (RelativeAnchorType)
                {
                    case AnchorTypes.Center:
                        return layoutInfo.ParentHeight / 2;
                    case AnchorTypes.End:
                        return layoutInfo.ParentHeight;
                }
            }

            return 0;
        }
    }
}
