using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SpiceEngine.Rendering.PostProcessing
{
    public class LogManager
    {
        public const int MAX_LINES_ON_SCREEN = 5;
        public const int SYNC_FRAMES = 180;

        private static ConcurrentQueue<LogLine> _lineQueue = new ConcurrentQueue<LogLine>();
        
        private TextRenderer _textRenderer;
        private LinkedList<LogLine> _screenLines = new LinkedList<LogLine>();

        public LogManager(TextRenderer renderer)
        {
            _textRenderer = renderer;
        }

        public void RenderToScreen()
        {
            var nOpenSlots = MAX_LINES_ON_SCREEN - _screenLines.Count;
            var logLine = _screenLines.Last;

            // Iterate through the list in reverse order, noting how many slots open up due to expiring items
            for (var i = 0; i < _screenLines.Count; i++)
            {
                if (logLine == null)
                {
                    break;
                }

                _textRenderer.RenderText(logLine.Value.Text, 10, 10 + i * (5 + TextRenderer.GLYPH_HEIGHT));
                logLine.Value.Increment();

                var previous = logLine.Previous;

                if (logLine.Value.IsExpired)
                {
                    // In this case, this line should be removed from the list, and we should note that we have an opening
                    _screenLines.RemoveLast();
                    nOpenSlots++;
                }

                logLine = previous;
            }

            // For each opening, we should try to get a line from the static line queue and place it at the front of the list
            for (var i = 0; i < nOpenSlots; i++)
            {
                if (_lineQueue.TryDequeue(out LogLine line))
                {
                    _screenLines.AddFirst(line);
                }
            }
        }

        public static void LogToScreen(string text)
        {
            var line = new LogLine(text)
            {
                ClearFrameDelay = 180
            };

            _lineQueue.Enqueue(line);
        }
    }
}
