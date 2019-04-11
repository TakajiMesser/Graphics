using SauceEditor.Models;
using System;
using System.Linq;
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;

namespace SauceEditor.Helpers
{
    public static class DockHelper
    {
        public static void AddToDockAsAnchorableDocument(DockingManager dockingManager, DockPanel content, String title, Action closedAction = null)
        {
            var anchorable = new LayoutAnchorable
            {
                Title = title,
                Content = content,
                CanClose = true
            };

            if (closedAction != null)
            {
                anchorable.Closed += (s, args) => closedAction.Invoke();
            }
            
            anchorable.AddToLayout(dockingManager, AnchorableShowStrategy.Most);
            anchorable.DockAsDocument();
        }

        public static void AddToDockAsDocument(DockingManager dockingManager, LayoutAnchorable content)
        {
            content.AddToLayout(dockingManager, AnchorableShowStrategy.Most);
            content.DockAsDocument();
        }

        public static void AddPanesToDockAsGrid(DockingManager dockingManager, int nRows, params LayoutAnchorablePane[] panes)
        {
            var rootPanel = dockingManager.Layout.RootPanel;
            rootPanel.Children.Clear();
            rootPanel.Orientation = Orientation.Vertical;

            int nPanesInRow = panes.Length / nRows;
            int nExtraPanes = panes.Length % nRows;
            int currentPaneIndex = 0;

            for (var i = 0; i < nRows; i++)
            {
                var paneGroup = new LayoutAnchorablePaneGroup()
                {
                    Orientation = Orientation.Horizontal,
                    DockMinHeight = dockingManager.ActualHeight / nRows
                };

                for (var j = 0; j < nPanesInRow + (i < nExtraPanes ? 1 : 0); j++)
                {
                    paneGroup.Children.Add(panes[currentPaneIndex]);
                    currentPaneIndex++;
                }

                rootPanel.Children.Add(paneGroup);
            }
        }

        public static void ShowAllPanesInDockAsGrid(DockingManager dockingManager)
        {
            var rootPanel = dockingManager.Layout.RootPanel;

            foreach (var childPaneGroup in rootPanel.Children.OfType<LayoutAnchorablePaneGroup>())
            {
                childPaneGroup.DockMinHeight = dockingManager.ActualHeight / rootPanel.Children.Count;

                foreach (var child in childPaneGroup.Children)
                {
                    if (child is LayoutAnchorablePane childPane)
                    {
                        //childPane.Children.First().Show();
                        if (childPane is IAnchor anchor)
                        {
                            anchor.Anchor.Show();
                        }
                    }
                }
            }
        }

        public static void ShowSinglePaneInDockGrid(DockingManager dockingManager, LayoutAnchorablePane pane)
        {
            var rootPanel = dockingManager.Layout.RootPanel;

            foreach (var childPaneGroup in rootPanel.Children.OfType<LayoutAnchorablePaneGroup>())
            {
                childPaneGroup.DockMinHeight = 0;

                foreach (var child in childPaneGroup.Children)
                {
                    if (child is LayoutAnchorablePane childPane)
                    {
                        if (childPane == pane)
                        {
                            childPaneGroup.DockMinHeight = dockingManager.ActualHeight;
                            //childPane.Children.First().Show();
                            if (childPane is IAnchor anchor)
                            {
                                anchor.Anchor.Show();
                            }
                        }
                        else
                        {
                            //childPane.Children.First().Hide();
                            if (childPane is IAnchor anchor)
                            {
                                anchor.Anchor.Hide();
                            }
                        }
                    }
                }
            }
        }
    }
}
