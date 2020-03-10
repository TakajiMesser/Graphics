namespace StarchUICore.Attributes.Sizes
{
    public struct Dock
    {
        public Dock(bool doesRespectChanges = true, IElement relativeElement = null)
        {
            RelativeElement = relativeElement;
            DoesRespectChanges = doesRespectChanges;
        }

        public bool DoesRespectChanges { get; }
        public IElement RelativeElement { get; }

        public Dock Attached(IElement relativeElement) => new Dock(DoesRespectChanges, relativeElement);
        public Dock Detached() => new Dock(DoesRespectChanges, null);

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
