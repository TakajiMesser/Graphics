namespace SweetGraphicsCore.Rendering.Processing.Post
{
    public enum LogPriorities
    {
        Low,
        High
    }

    public class LogLine
    {
        private int _logFramesRemaining;
        private int _clearFrameDelay = 1;

        public string Text { get; private set; }
        public int ClearFrameDelay
        {
            get => _clearFrameDelay;
            set
            {
                _clearFrameDelay = value;
                _logFramesRemaining = _clearFrameDelay;
            }
        }

        public bool IsExpired => _logFramesRemaining <= 0;

        public LogLine(string text)
        {
            Text = text;
            _logFramesRemaining = ClearFrameDelay;
        }

        public void Increment() => _logFramesRemaining--;
    }
}
