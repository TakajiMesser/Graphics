using OpenTK;
using OpenTK.Graphics.OpenGL;
using SpiceEngine.Outputs;
using SpiceEngine.Properties;
using SpiceEngine.Rendering.Buffers;
using SpiceEngine.Rendering.Processing;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Vertices;
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;

namespace SpiceEngine.Rendering.PostProcessing
{
    public class LogManager
    {
        private static ConcurrentQueue<string> _lines = new ConcurrentQueue<string>();

        private string _currentLogText;
        private int _logFramesRemaining;

        public int ClearFrameDelay { get; set; } = 1;

        public bool TryGetLogText(out string text)
        {
            if (_currentLogText == null)
            {
                // Try to grab a new line
                if (_lines.TryDequeue(out string line))
                {
                    _currentLogText = line;
                    _logFramesRemaining = ClearFrameDelay;
                }
                else
                {
                    text = null;
                    return false;
                }
            }

            text = _currentLogText;

            if (_logFramesRemaining > 1)
            {
                _logFramesRemaining--;
            }
            else
            {
                _currentLogText = null;
            }

            return true;
        }

        public static void LogToScreen(string text) => _lines.Enqueue(text);
    }
}
