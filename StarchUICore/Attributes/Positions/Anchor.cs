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
        public Anchor(AnchorTypes anchorType, bool doesRespectChanges = true, IElement relativeElement = null)
        {
            AnchorType = anchorType;
            RelativeElement = relativeElement;
            DoesRespectChanges = doesRespectChanges;
        }

        public AnchorTypes AnchorType { get; }
        public bool DoesRespectChanges { get; }
        public IElement RelativeElement { get; }

        public Anchor Attached(IElement relativeElement) => new Anchor(AnchorType, DoesRespectChanges, relativeElement);
        public Anchor Detached() => new Anchor(AnchorType, DoesRespectChanges, null);

        // relativeX is where the parent THINKS this View should be placed relative to its left side
        // parentAbsoluteX is the absolute X of the left side of the parent group
        // What we WANT to do here is to determine the relative X between where this anchor would place the View's left side vs where the parent wants it to be
        // We also need the parentWidth in case this AnchorType is RIGHT
        public int? GetReferenceX(int relativeX, int parentAbsoluteX, int? measuredWidth)
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

        public int? GetReferenceY(int relativeY, int parentAbsoluteY, int? measuredHeight)
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
        }

        public int? GetReferenceWidth(int parentWidth)
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

        public int? GetReferenceHeight(int parentHeight)
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
