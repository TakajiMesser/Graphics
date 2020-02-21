using SpiceEngineCore.Utilities;
using StarchUICore.Attributes.Units;

namespace StarchUICore.Attributes.Positions
{
    public struct Position
    {
        public Position(IUnits x, IUnits y, IUnits minimumX, IUnits minimumY, IUnits maximumX, IUnits maximumY)
        {
            X = x;
            Y = y;
            MinimumX = minimumX;
            MinimumY = minimumY;
            MaximumX = maximumX;
            MaximumY = maximumY;
        }

        public IUnits X { get; private set; }
        public IUnits Y { get; private set; }

        public IUnits MinimumX { get; private set; }
        public IUnits MinimumY { get; private set; }

        public IUnits MaximumX { get; private set; }
        public IUnits MaximumY { get; private set; }

        public int? ConstrainX(int availableWidth, int? measuredWidth, int? anchorRelativeX, int? anchorWidth)
        {
            // What exactly is the parent providing us with?
            // availableWidth -> the remainingWidth in the Group
            // measuredWidth -> what this view measured its width to be
            // anchorWidth -> either the parent width, or the width of the relative anchor element
            // anchorX -> either the absolute X of the parent, or the absolute X of the relative anchor element
            // absoluteX -> where the Group is planning on placing this view

            // The ACTUAL issue here is that we can no longer just use availableWidth -> We cannot assume that the available X range is from zero to availableWidth!
            if (anchorRelativeX.HasValue && anchorWidth.HasValue && measuredWidth.HasValue)
            {
                var constrainedX = X.ToOffsetPixels(anchorWidth.Value);

                if (!(MinimumX is AutoUnits))
                {
                    constrainedX = constrainedX.ClampBottom(MinimumX.ToOffsetPixels(anchorWidth.Value));
                }

                if (!(MaximumX is AutoUnits))
                {
                    constrainedX = constrainedX.ClampTop(MaximumX.ToOffsetPixels(anchorWidth.Value));
                }

                // Now we have the "relative" constrained X, but this is relative to the anchor type (i.e. could be from the right-side)
                // constrainedX + measuredWidth.Value must be < availableWidth
                var relativeXBound = constrainedX + anchorRelativeX.Value + measuredWidth.Value;

                if (relativeXBound > availableWidth)
                {
                    // In this case we are SURPASSING our upper bound as far as the parent is concerned, so we should shrink our constrainedX by this amount that we surpassed
                    constrainedX -= relativeXBound - availableWidth;
                    constrainedX.ClampBottom(0);
                }

                // Now that we've determined this view's constrained X, we can add back on the anchor X to determine what our relative X in the parent should be
                return anchorRelativeX.Value + constrainedX;
            }

            return null;

            // We are returning an X value RELATIVE to the parent Group
            // It is not good enough to just have the anchor width, but we need to know the anchor's absolute X as well to ensure we stay within the parent's set boundaries
            // Let's say we have a Left Anchor, but the Anchor's relativeX is +30 from where the parent thought this child should be placed
            // This means that we must also adhere to constrainedX + anchorRelativeX + measuredWidth.Value < availableWidth

            // Now let's say we have a Right Anchor, but the Anchor's relativeX is +30 from where the parent thought this child should be placed
            // This means that we must also adhere to constrainedX + 

            // When constraining position, we need both the absolute X AND the parent width
            // PercentUnits are based off of the reference element's width, so parentWidth is necessary there
            // This method should return RELATIVE coordinates, but we need to be mindful of the AnchorType
            // TODO - How do we handle Anchor?
            // TBD, this means we want to "center"
            // Centering is difficult, because we want to base our X off of the available width
            // BUT, we need to wait until all potential further children are done being measured
            // We need to AT LEAST report back to the parent, however, so this element at least gets it's needed width
            /*var constrainedX = X.ToDimensionPixels(availableWidth, referenceWidth);

            if (HorizontalAnchor.AnchorType == AnchorTypes.End)
            {
                if (measuredWidth.HasValue)
                {
                    constrainedX = (measuredWidth.Value - constrainedX).ClampBottom(0);
                }
                else
                {
                    return null;
                }
            }

            // TODO - Do we need to adjust these values based on Anchor Type?
            if (!(MinimumX is AutoUnits))
            {
                constrainedX = constrainedX.ClampBottom(MinimumX.ToDimensionPixels(availableWidth, referenceWidth));
            }

            if (!(MaximumX is AutoUnits))
            {
                constrainedX = constrainedX.ClampTop(MaximumX.ToDimensionPixels(availableWidth, referenceWidth));
            }

            return constrainedX;*/
        }

        public int? ConstrainY(int availableHeight, int? measuredHeight, int? anchorRelativeY, int? anchorHeight)
        {
            if (anchorRelativeY.HasValue && anchorHeight.HasValue && measuredHeight.HasValue)
            {
                var constrainedY = Y.ToOffsetPixels(anchorHeight.Value);

                if (!(MinimumY is AutoUnits))
                {
                    constrainedY = constrainedY.ClampBottom(MinimumY.ToOffsetPixels(anchorHeight.Value));
                }

                if (!(MaximumY is AutoUnits))
                {
                    constrainedY = constrainedY.ClampTop(MaximumY.ToOffsetPixels(anchorHeight.Value));
                }

                var relativeYBound = constrainedY + anchorRelativeY.Value + measuredHeight.Value;

                if (relativeYBound > availableHeight)
                {
                    constrainedY -= relativeYBound - availableHeight;
                    constrainedY.ClampBottom(0);
                }

                return constrainedY;
            }

            return null;
        }

        public static Position FromXY(IUnits x, IUnits y) => new Position(x, y, Unit.Auto(), Unit.Auto(), Unit.Auto(), Unit.Auto());
    }
}
