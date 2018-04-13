using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DockingLibrary
{
    class OverlayDockablePane : DockablePane
    {
        public readonly DockablePane ReferencedPane;
        public readonly DockableContent ReferencedContent;

        public OverlayDockablePane(DockManager dockManager, DockableContent content, Docks initialDock) : base(dockManager, initialDock)
        {
            btnAutoHide.LayoutTransform = new RotateTransform(90);
            ReferencedPane = content.ContainerPane as DockablePane;
            ReferencedContent = content;
            Add(ReferencedContent);
            Show(ReferencedContent);
            ReferencedContent.ContainerPane = ReferencedPane;

            State = PaneState.AutoHide;
        }

        public override void Show()
        {
            ChangeState(PaneState.Docked);
        }

        public override void Close()
        {
            ChangeState(PaneState.Hidden);
        }

        public override void Close(DockableContent content)
        {
            ChangeState(PaneState.Hidden);
        }
     }
}
