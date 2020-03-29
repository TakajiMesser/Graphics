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

        public int? GetConstrainedX(int suggestedX, int? referenceWidth)
        {
            // First, determine what this element thinks its X should be
            var desiredX = X.ToOffsetPixels(suggestedX, referenceWidth);

            // TODO - Handle situation where parent then resizes its own width afterwards during this layout cycle...
            if (desiredX.HasValue)
            {
                var minimumConstrainedX = MinimumX.ConstrainAsMinimum(desiredX.Value, referenceWidth);

                if (minimumConstrainedX.HasValue)
                {
                    var maximumConstrainedX = MaximumX.ConstrainAsMaximum(minimumConstrainedX.Value, referenceWidth);

                    if (maximumConstrainedX.HasValue)
                    {
                        return maximumConstrainedX.Value;
                    }
                }
            }

            return null;
        }

        public int? GetConstrainedY(int suggestedY, int? referenceHeight)
        {
            // First, determine what this element thinks its X should be
            var desiredY = Y.ToOffsetPixels(suggestedY, referenceHeight);

            // TODO - Handle situation where parent then resizes its own width afterwards during this layout cycle...
            if (desiredY.HasValue)
            {
                var minimumConstrainedY = MinimumY.ConstrainAsMinimum(desiredY.Value, referenceHeight);

                if (minimumConstrainedY.HasValue)
                {
                    var maximumConstrainedY = MaximumY.ConstrainAsMaximum(minimumConstrainedY.Value, referenceHeight);

                    if (maximumConstrainedY.HasValue)
                    {
                        return maximumConstrainedY.Value;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="availableWidth">The remaining width in the containing Group.</param>
        /// <param name="measuredWidth">The measured width of this element.</param>
        /// <param name="anchorRelativeX">This element's relative position, as requested by the containing Group and constrained by the anchor</param>
        /// <param name="referenceWidth"></param>
        /// <returns></returns>
        /*public int? ConstrainX(int availableWidth, int? measuredWidth, int? anchoredX, int? referenceWidth)
        {
            // anchorWidth -> either the parent width, or the width of the relative anchor element
            // anchorX -> either the absolute X of the parent, or the absolute X of the relative anchor element
            // absoluteX -> where the Group is planning on placing this view

            // The ACTUAL issue here is that we can no longer just use availableWidth -> We cannot assume that the available X range is from zero to availableWidth!
            if (anchoredX.HasValue && referenceWidth.HasValue && measuredWidth.HasValue)
            {
                var constrainedX = X.ToOffsetPixels(referenceWidth.Value);

                if (X is AutoUnits)
                {
                    // constrainedX represents the relative X BEFORE being applied to the actual anchor, which could be Left/Right/Center
                    // But assuming that constrainedX is still relative from the LEFT of the parent, if X is AUTO, then we need to subtract half of this view
                    constrainedX = (int)Math.Round((constrainedX - measuredWidth.Value) / 2.0f);
                }

                if (!(MinimumX is AutoUnits))
                {
                    constrainedX = constrainedX.ClampBottom(MinimumX.ToOffsetPixels(referenceWidth.Value));
                }

                if (!(MaximumX is AutoUnits))
                {
                    constrainedX = constrainedX.ClampTop(MaximumX.ToOffsetPixels(referenceWidth.Value));
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

            return constrainedX;*
        }

        public int? ConstrainY(int availableHeight, int? measuredHeight, int? anchorRelativeY, int? anchorHeight)
        {
            if (anchorRelativeY.HasValue && anchorHeight.HasValue && measuredHeight.HasValue)
            {
                var constrainedY = Y.ToOffsetPixels(anchorHeight.Value);

                if (Y is AutoUnits)
                {
                    constrainedY = (int)Math.Round((constrainedY - measuredHeight.Value) / 2.0f);
                }

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
        }*/

        public Position Offset(IUnits x, IUnits y) => new Position(x, y, MinimumX, MinimumY, MaximumX, MaximumY);

        public Position OffsetMinimums(IUnits minimumX, IUnits minimumY) => new Position(X, Y, minimumX, minimumY, MaximumX, MaximumY);

        public Position OffsetMaximums(IUnits maximumX, IUnits maximumY) => new Position(X, Y, MinimumX, MinimumY, maximumX, maximumY);

        public static Position FromOffsets(IUnits x, IUnits y) => new Position(x, y, Unit.Auto(), Unit.Auto(), Unit.Auto(), Unit.Auto());
    }
}
