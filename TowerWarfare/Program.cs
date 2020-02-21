using OpenTK;
using SpiceEngine.Maps;
using StarchUICore;
using StarchUICore.Attributes.Positions;
using StarchUICore.Attributes.Sizes;
using StarchUICore.Attributes.Units;
using StarchUICore.Groups;
using StarchUICore.Views;
using StarchUICore.Views.Controls.Buttons;
using System;
using System.Text;
using TowerWarfare.Builders;
using TowerWarfare.Helpers;
using GameWindow = SpiceEngine.Game.GameWindow;

namespace TowerWarfare
{
    class Program
    {
        static void Main(string[] args)
        {
            PerformUITest();

            ProjectBuilder.CreateTestProject();
            var map = Map.Load(FilePathHelper.MAP_PATH);

            using (var gameWindow = new GameWindow(map))
            {
                gameWindow.VSync = VSyncMode.Adaptive;
                gameWindow.LoadAndRun();
                //gameWindow.Run(60.0, 0.0);
            }
        }

        private static void PerformUITest()
        {
            var root = GetRootElement();

            //var windowSize = new MeasuredSize(1000, 1000);
            //var windowPosition = new LocatedPosition(0, 0);

            root.Layout(new LayoutInfo(1000, 1000, 1000, 1000, 0, 0, 0, 0));
            //root.Measure(windowSize);
            //root.Locate(windowPosition);

            LogMeasurements(root);
            LogLocations(root);
        }

        private static IElement GetRootElement()
        {
            var rowGroup1A = new RowGroup()
            {
                Position = Position.FromXY(Unit.Pixels(0), Unit.Pixels(0)),
                Size = Size.FromDimensions(Unit.Pixels(800), Unit.Auto())
            };

            var view2A = new TestView(rowGroup1A, Unit.Percents(10), Unit.Percents(10), Unit.Pixels(0), Unit.Pixels(0));
            rowGroup1A.AddChild(view2A);

            var view2B = new TestView(rowGroup1A, Unit.Pixels(100), Unit.Pixels(100), Unit.Pixels(0), Unit.Pixels(0));
            rowGroup1A.AddChild(view2B);

            var rowGroup2A = new RowGroup()
            {
                Position = Position.FromXY(Unit.Pixels(0), Unit.Pixels(0)),
                Size = Size.FromDimensions(Unit.Percents(50), Unit.Pixels(200))
            };
            rowGroup1A.AddChild(rowGroup2A);

            var view2C = new TestView(rowGroup1A, Unit.Percents(100), Unit.Percents(100), Unit.Pixels(0), Unit.Pixels(0));
            rowGroup1A.AddChild(view2C);

            var view3A = new TestView(rowGroup2A, Unit.Pixels(200), Unit.Pixels(300), Unit.Pixels(0), Unit.Pixels(0));
            rowGroup2A.AddChild(view3A);

            var view3B = new TestView(rowGroup2A, Unit.Percents(100), Unit.Auto(), Unit.Pixels(0), Unit.Pixels(0));
            rowGroup2A.AddChild(view3B);

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
            builder.Append(" - (" + measurement.Width + ", " + measurement.Height + ")");

            Console.WriteLine(builder.ToString());

            if (element is IGroup group)
            {
                foreach (var child in group.Children)
                {
                    LogMeasurements(child);
                }
            }
        }

        private static void LogLocations(IElement element)
        {
            var builder = new StringBuilder();

            for (var i = 0; i < GetAncestryTier(element); i++)
            {
                builder.Append("\t");
            }

            builder.Append(element.GetType().Name);

            var location = element.Location;
            builder.Append(" - (" + location.X + ", " + location.Y + ")");

            Console.WriteLine(builder.ToString());

            if (element is IGroup group)
            {
                foreach (var child in group.Children)
                {
                    LogLocations(child);
                }
            }
        }
    }
}
