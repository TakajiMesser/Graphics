namespace StarchUICore.Attributes.Sizes
{
    public struct Dock
    {
        public Dock(IElement relativeElement, bool doesRespectChanges = true)
        {
            RelativeElement = relativeElement;
            DoesRespectChanges = doesRespectChanges;
        }

        public IElement RelativeElement { get; }
        public bool DoesRespectChanges { get; }

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
