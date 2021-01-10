using SpiceEngine.Game;
using SpiceEngine.Maps;
using StarchUICore;
using StarchUICore.Attributes.Units;
using StarchUICore.Builders;
using StarchUICore.Groups;
using System;
using System.Text;
using TowerWarfare.Builders;
using TowerWarfare.Helpers;

namespace TowerWarfare
{
    class Program
    {
        static void Main(string[] args)
        {
            //PerformUITest();

            ProjectBuilder.CreateTestProject();
            var map = Map.Load(FilePathHelper.MAP_PATH);

            /*var configuration = new Configuration()
            {
                UpdatesPerSecond = 60,
                RendersPerSecond = 60
            };

            var window = new Window(configuration)
            {
                Map = map
            };

            window.Start();*/

            using (var gameWindow = new GameWindow(map))
            {
                gameWindow.LoadAndRun();
            }
        }

        private static void PerformUITest()
        {
            var root = GetRootElement();

            //var windowSize = new MeasuredSize(1000, 1000);
            //var windowPosition = new LocatedPosition(0, 0);

            //root.Layout(new LayoutInfo(1000, 1000, 1000, 1000, 0, 0, 0, 0));
            //root.Measure(windowSize);
            //root.Locate(windowPosition);

            LogMeasurements(root);
        }

        private static IElement GetRootElement()
        {
            var view2A = new UIBuilder()
                .WithPosition(Unit.Pixels(0), Unit.Pixels(0))
                .WithSize(Unit.Percents(10), Unit.Percents(10))
                .CreateView();

            var view2B = new UIBuilder()
                .WithPosition(Unit.Pixels(0), Unit.Pixels(0))
                .WithSize(Unit.Percents(10), Unit.Percents(10))
                .CreateView();

            var view3A = new UIBuilder()
                .WithPosition(Unit.Pixels(0), Unit.Pixels(0))
                .WithSize(Unit.Pixels(200), Unit.Pixels(300))
                .CreateView();

            var view3B = new UIBuilder()
                .WithPosition(Unit.Pixels(0), Unit.Pixels(0))
                .WithSize(Unit.Percents(100), Unit.Auto())
                .CreateView();

            var rowGroup2C = new UIBuilder()
                .WithPosition(Unit.Pixels(0), Unit.Pixels(0))
                .WithSize(Unit.Percents(50), Unit.Pixels(200))
                .WithChildren(view3A, view3B)
                .CreateRowGroup();

            var view2D = new UIBuilder()
                .WithPosition(Unit.Pixels(0), Unit.Pixels(0))
                .WithSize(Unit.Percents(100), Unit.Percents(100))
                .CreateView();

            var rowGroup1A = new UIBuilder()
                .WithPosition(Unit.Pixels(0), Unit.Pixels(0))
                .WithSize(Unit.Pixels(800), Unit.Auto())
                .WithChildren(view2A, view2B, rowGroup2C, view2D)
                .CreateView();

            return rowGroup1A;
        }

        private static int GetAncestryTier(IElement element, int currentTier = 0) => element.Parent != null
            ? GetAncestryTier(element.Parent, currentTier + 1)
            : currentTier;

        private static void LogMeasurements(IElement element)
        {
            var builder = new StringBuilder();

            for(var i = 0; i < GetAncestryTier(element); i++)
            {
                builder.Append("\t");
            }

            builder.Append(element.GetType().Name);

            var measurement = element.Measurement;
            builder.Append(" - (" + measurement.X + ", " + measurement.Y + ", " + measurement.Width + ", " + measurement.Height + ")");

            Console.WriteLine(builder.ToString());

            if (element is IGroup group)
            {
                foreach (var child in group.Children)
                {
                    LogMeasurements(child);
                }
            }
        }
    }
}
