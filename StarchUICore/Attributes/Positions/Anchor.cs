using SpiceEngineCore.Utilities;
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

        public int? GetAnchorX(int desiredX, int measuredWidth, int minParentX, int maxParentX, int parentWidth, int parentAbsoluteX)
        {
            var anchorRelativeX = GetAnchorXRelativeToParent(parentAbsoluteX, parentWidth);

            if (anchorRelativeX.HasValue)
            {
                desiredX += anchorRelativeX.Value;

                var leftX = 0;
                var rightX = 0;

                switch (SelfAnchorType)
                {
                    case AnchorTypes.Start:
                        leftX = desiredX;
                        rightX = desiredX + measuredWidth;
                        break;
                    case AnchorTypes.Center:
                        var halfWidth = (int)Math.Round(measuredWidth / 2.0f);
                        leftX = desiredX - halfWidth;
                        rightX = desiredX + halfWidth;
                        break;
                    case AnchorTypes.End:
                        leftX = desiredX - measuredWidth;
                        rightX = desiredX;
                        break;
                }

                desiredX = leftX.ClampBottom(minParentX);

                if (rightX > maxParentX)
                {
                    // In this case, we need to try to push as much as we can left by however much is "jutting out" on the right
                    var excessX = rightX - maxParentX;
                    desiredX = (leftX - excessX).ClampBottom(minParentX);
                }

                return desiredX;
            }

            return null;
        }

        public int? GetAnchorY(int desiredY, int measuredHeight, int minParentY, int maxParentY, int parentHeight, int parentAbsoluteY)
        {
            var anchorRelativeY = GetAnchorYRelativeToParent(parentAbsoluteY, parentHeight);

            if (anchorRelativeY.HasValue)
            {
                desiredY += anchorRelativeY.Value;

                var topY = 0;
                var bottomY = 0;

                switch (SelfAnchorType)
                {
                    case AnchorTypes.Start:
                        topY = desiredY;
                        bottomY = desiredY + measuredHeight;
                        break;
                    case AnchorTypes.Center:
                        var halfHeight = (int)Math.Round(measuredHeight / 2.0f);
                        topY = desiredY - halfHeight;
                        bottomY = desiredY + halfHeight;
                        break;
                    case AnchorTypes.End:
                        topY = desiredY - measuredHeight;
                        bottomY = desiredY;
                        break;
                }

                desiredY = topY.ClampBottom(minParentY);

                if (bottomY > maxParentY)
                {
                    // In this case, we need to try to push as much as we can up by however much is "jutting out" on the bottom
                    var excessY = bottomY - maxParentY;
                    desiredY = (topY - excessY).ClampBottom(minParentY);
                }

                return desiredY;
            }

            return null;
        }

        private int? GetAnchorXRelativeToParent(int parentAbsoluteX, int parentWidth)
        {
            if (RelativeElement != null)
            {
                // We need to translate the anchor's absolute X to be relative to this element's parent, then we need to interpret our Element's desired X as being relative to its anchor, so that we can convert it to be relative from the parent
                switch (RelativeAnchorType)
                {
                    case AnchorTypes.Start:
                        return RelativeElement.Location.X - parentAbsoluteX;
                    case AnchorTypes.Center:
                        return RelativeElement.Location.X + (RelativeElement.Measurement.Width / 2) - parentAbsoluteX;
                    case AnchorTypes.End:
                        return RelativeElement.Location.X + RelativeElement.Measurement.Width - parentAbsoluteX;
                }
            }
            else
            {
                switch (RelativeAnchorType)
                {
                    case AnchorTypes.Start:
                        return 0;
                    case AnchorTypes.Center:
                        return parentWidth / 2;
                    case AnchorTypes.End:
                        return parentWidth;
                }
            }

            return null;
        }

        private int? GetAnchorYRelativeToParent(int parentAbsoluteY, int parentHeight)
        {
            if (RelativeElement != null)
            {
                // We need to translate the anchor's absolute X to be relative to this element's parent, then we need to interpret our Element's desired X as being relative to its anchor, so that we can convert it to be relative from the parent
                switch (RelativeAnchorType)
                {
                    case AnchorTypes.Start:
                        return RelativeElement.Location.Y - parentAbsoluteY;
                    case AnchorTypes.Center:
                        return RelativeElement.Location.Y + (RelativeElement.Measurement.Height / 2) - parentAbsoluteY;
                    case AnchorTypes.End:
                        return RelativeElement.Location.Y + RelativeElement.Measurement.Height - parentAbsoluteY;
                }
            }
            else
            {
                switch (RelativeAnchorType)
                {
                    case AnchorTypes.Start:
                        return 0;
                    case AnchorTypes.Center:
                        return parentHeight / 2;
                    case AnchorTypes.End:
                        return parentHeight;
                }
            }

            return null;
        }

        /// <summary>
        /// What we WANT to do here is to determine the relative X between where this anchor would place the View's left side vs where the parent wants it to be
        /// We also need the parentWidth in case this AnchorType is RIGHT
        /// </summary>
        /// <param name="relativeX">The parent's intended positioning of the left-side of this element, relative to the parent.</param>
        /// <param name="parentAbsoluteX">The absolute X position of the left-side of the anchored element's parent.</param>
        /// <param name="measuredWidth">The measured width of the anchored element.</param>
        /// <returns>The X position of this element, relative to its parent, after the anchor is taken into account.</returns>
        /*public int? GetAnchoredX(int relativeX, int parentAbsoluteX, int availableParentWidth, int? measuredWidth)
        {


            if (RelativeElement != null)
            {
                // TODO - Determine all of the scenarios that require either this element or the relative element to already be measured
                if (RelativeElement.Location.NeedsLocating || RelativeElement.Measurement.NeedsMeasuring
                    && SelfAnchorType == AnchorTypes.End)
                {
                    return null;
                }
                else
                {
                    if (RelativeAnchorType == AnchorTypes.Start && SelfAnchorType == AnchorTypes.Start)
                    {
                        // This is the most simple case, where 
                    }

                    if (AnchorType == AnchorTypes.Start)
                    {
                        // How far "off" is this Anchor element's X from the desired relativeX?
                        //var desiredAbsoluteLeftX = parentAbsoluteX + relativeX;
                        //var anchorAbsoluteLeftX = RelativeElement.Location.X;
                        return RelativeElement.Location.X - (parentAbsoluteX + relativeX);
                    }
                    else if (AnchorType == AnchorTypes.End)
                    {
                        if (RelativeElement.Measurement.NeedsMeasuring || !measuredWidth.HasValue)
                        {
                            return null;
                        }
                        else
                        {
                            //var desiredAbsoluteRightX = parentAbsoluteX + relativeX + measuredWidth.Value;
                            //var anchorAbsoluteRightX = RelativeElement.Location.X + RelativeElement.Measurement.Width;
                            return RelativeElement.Location.X + RelativeElement.Measurement.Width - (parentAbsoluteX + relativeX + measuredWidth.Value);
                        }
                    }
                    else
                    {
                        // TODO - Handle Centered Anchor Type
                        return null;
                    }
                }
            }
            else
            {
                if (RelativeAnchorType == AnchorTypes.Start && SelfAnchorType == AnchorTypes.Start) // Anchor element-left to parent-left
                {
                    return relativeX;
                }
                else if (RelativeAnchorType == AnchorTypes.Start && SelfAnchorType == AnchorTypes.End) // Anchor element-right to parent-left
                {
                    // This now means that this element's "X" value is meant to denote its position relative from the RIGHT side of the parent
                    // SO, we need to return 
                }


                // TODO - Still need to handle other anchor types
                if (AnchorType == AnchorTypes.Start)
                {
                    return relativeX;
                }
                else if (AnchorType == AnchorTypes.End)
                {
                    if (!measuredWidth.HasValue)
                    {
                        return null;
                    }
                    else
                    {
                        return relativeX + measuredWidth.Value;
                    }
                }
                else
                {
                    // TODO - Handle Centered Anchor Type
                    return null;
                }
            }
        }

        public int? GetAnchoredY(int relativeY, int parentAbsoluteY, int? measuredHeight)
        {
            if (RelativeElement != null)
            {
                if (RelativeElement.Location.NeedsLocating)
                {
                    return null;
                }
                else
                {
                    if (AnchorType == AnchorTypes.Start)
                    {
                        return RelativeElement.Location.X - (parentAbsoluteY + relativeY);
                    }
                    else if (AnchorType == AnchorTypes.End)
                    {
                        if (RelativeElement.Measurement.NeedsMeasuring || !measuredHeight.HasValue)
                        {
                            return null;
                        }
                        else
                        {
                            return RelativeElement.Location.X + RelativeElement.Measurement.Width - (parentAbsoluteY + relativeY + measuredHeight.Value);
                        }
                    }
                    else
                    {
                        // TODO - Handle Centered Anchor Type
                        return null;
                    }
                }
            }
            else
            {
                // TODO - Still need to handle other anchor types
                if (AnchorType == AnchorTypes.Start)
                {
                    return relativeY;
                }
                else if (AnchorType == AnchorTypes.End)
                {
                    if (!measuredHeight.HasValue)
                    {
                        return null;
                    }
                    else
                    {
                        return relativeY + measuredHeight.Value;
                    }
                }
                else
                {
                    // TODO - Handle Centered Anchor Type
                    return null;
                }
            }
        }*/

        public int? GetAnchoredWidth(int parentWidth)
        {
            if (RelativeElement != null)
            {
                if (RelativeElement.Measurement.NeedsMeasuring)
                {
                    return null;
                }
                else
                {
                    return RelativeElement.Measurement.Width;
                }
            }
            else
            {
                return parentWidth;
            }
        }

        public int? GetAnchoredHeight(int parentHeight)
        {
            if (RelativeElement != null)
            {
                if (RelativeElement.Measurement.NeedsMeasuring)
                {
                    return null;
                }
                else
                {
                    return RelativeElement.Measurement.Height;
                }
            }
            else
            {
                return parentHeight;
            }
        }
    }
}
