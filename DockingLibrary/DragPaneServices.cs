using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DockingLibrary
{
    public class DragPaneServices
    {
        public DockManager DockManager { get; private set; }
        public FloatingWindow FloatingWindow { get; private set; }
        public DockablePane DockablePane { get; private set; }

        private List<IDropSurface> _surfaces = new List<IDropSurface>();
        private List<IDropSurface> _surfacesWithDragOver = new List<IDropSurface>();

        private Point _offset;

        public DragPaneServices(DockManager owner)
        {
            DockManager = owner;
        }

        public void Register(IDropSurface surface)
        {
            if (!_surfaces.Contains(surface))
            {
                _surfaces.Add(surface);
            }
        }

        public void Unregister(IDropSurface surface)
        {
            _surfaces.Remove(surface);
        }

        //public static void StartDrag(DockablePane pane, Point point)
        //{
        //    StartDrag(new FloatingWindow(_pane), point);
        //}

        public void StartDrag(FloatingWindow window, Point point, Point offset)
        {
            DockablePane = window.HostedPane;
            FloatingWindow = window;
            _offset = offset;

            if (_offset.X >= FloatingWindow.Width)
            {
                _offset.X = FloatingWindow.Width / 2;
            }

            FloatingWindow.Left = point.X - _offset.X;
            FloatingWindow.Top = point.Y - _offset.Y;
            FloatingWindow.Show();

            foreach (IDropSurface surface in _surfaces)
            {
                if (surface.SurfaceRectangle.Contains(point))
                {
                    _surfacesWithDragOver.Add(surface);
                    surface.OnDragEnter(point);
                }
            }
        }

        public void MoveDrag(Point point)
        {
            if (FloatingWindow != null)
            {
                FloatingWindow.Left = point.X - _offset.X;
                FloatingWindow.Top = point.Y - _offset.Y;

                List<IDropSurface> enteringSurfaces = new List<IDropSurface>();
                foreach (IDropSurface surface in _surfaces)
                {
                    if (surface.SurfaceRectangle.Contains(point))
                    {
                        if (!_surfacesWithDragOver.Contains(surface))
                            enteringSurfaces.Add(surface);
                        else
                            surface.OnDragOver(point);
                    }
                    else if (_surfacesWithDragOver.Contains(surface))
                    {
                        _surfacesWithDragOver.Remove(surface);
                        surface.OnDragLeave(point);
                    }
                }

                foreach (IDropSurface surface in enteringSurfaces)
                {
                    _surfacesWithDragOver.Add(surface);
                    surface.OnDragEnter(point);
                }
            }
        }

        public void EndDrag(Point point)
        {
            IDropSurface dropSurface = _surfaces.FirstOrDefault(s => s.SurfaceRectangle.Contains(point) && s.OnDrop(point));

            foreach (IDropSurface surface in _surfacesWithDragOver.Where(s => s != dropSurface))
            {
                surface.OnDragLeave(point);
            }

            _surfacesWithDragOver.Clear();
            
            if (dropSurface != null)
            {
                FloatingWindow.Close();
            }

            FloatingWindow = null;
            DockablePane = null;
        }
    }
}
