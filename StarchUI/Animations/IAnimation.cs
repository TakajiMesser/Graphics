namespace StarchUICore.Animations
{
    public interface IAnimation
    {
        int Duration { get; }
        int Delay { get; set; }

        bool IsEnded { get; }

        void Apply(IElement element, int delay = 0);
        void Update(int nTicks);
        void Reset();
    }
}
