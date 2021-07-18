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
    }
}
