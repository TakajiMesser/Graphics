using System;
using System.Diagnostics;
using System.Timers;

namespace SpiceEngineCore.Helpers
{
    public class LogWatch
    {
        private Stopwatch _stopwatch = new Stopwatch();
        private Timer _timeoutTimer = null;

        private string _name = "";

        public string Name
        {
            get => _name;
            set => _name = value == null ? "" : value + " ";
        }

        public long Timeout { get; private set; }
        public long TimeoutCheckFrequency { get; private set; }

        public bool IsTimedOut { get; private set; } = false;

        public event EventHandler<EventArgs> TimedOut;

        public void Log(string message) => Console.WriteLine(Name + message + " Time: " + _stopwatch.ElapsedMilliseconds + "ms");

        public void Start(string message = "Start")
        {
            _timeoutTimer?.Start();
            _stopwatch.Start();
            Log(message);
        }

        public void Stop(string message = "Stop")
        {
            _timeoutTimer?.Stop();
            _stopwatch.Stop();
            Log(message);
        }

        private void CheckTimeout()
        {
            if (_stopwatch.ElapsedMilliseconds > Timeout)
            {
                IsTimedOut = true;
                TimedOut?.Invoke(this, new EventArgs());
            }
        }

        public static LogWatch CreateAndStart(string name)
        {
            var logWatch = new LogWatch()
            {
                Name = name
            };

            logWatch.Start();

            return logWatch;
        }

        public static LogWatch CreateWithTimeout(string name, long timeout, long checkFrequency)
        {
            var logWatch = new LogWatch()
            {
                Name = name,
                Timeout = timeout,
                TimeoutCheckFrequency = checkFrequency
            };

            logWatch._timeoutTimer = new Timer(checkFrequency);
            logWatch._timeoutTimer.Elapsed += (s, args) => logWatch.CheckTimeout();

            logWatch.Start();

            return logWatch;
        }
    }
}
