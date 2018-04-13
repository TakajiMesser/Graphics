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
    [Flags]
    public enum Docks
    {
        None = 0,
        Left = 1,
        Top = 2,
        Right = 4,
        Bottom = 8
    }

    /// <summary>
    /// Rappresents a content embeddable in a dockable pane or in a documents pane
    /// </summary>
    public class DockableContent : ManagedContent
    {
        /// <summary>
        /// Set/get content title whish is shown at top of dockable panes and in tab items
        /// </summary>
        public new string Title
        {
            get => base.Title;
            set
            {
                base.Title = value;
                ContainerPane?.RefreshTitle();
            }
        }

        public DockableContent() { }
        public DockableContent(DockManager manager) : base(manager) { }

        /// <summary>
        /// Show this content
        /// </summary>
        /// <remarks>Show this content in a dockable pane. If no pane was previously created, it creates a new one with default right dock. </remarks>
        public override void Show()
        {
            if (ContainerPane != null)
            {
                ContainerPane.Show(this);
            }
            else
            {
                //Show(Docks.Right);
                if (ContainerPane == null)
                {
                    ContainerPane = new DockablePane(DockManager);
                    ContainerPane.Add(this);
                    ContainerPane.Show();

                    DockManager.Add(ContainerPane as DockablePane);
                }
                else
                {
                    ContainerPane.Show(this);
                    ContainerPane.Show();
                }
            }
        }

        /// <summary>
        /// Show this content
        /// </summary>
        /// <remarks>Show this content in a dockable pane. If no pane was previously created, it creates a new one with passed initial dock. </remarks>
        public void Show(Docks dock)
        {
            if (ContainerPane == null)
            {
                ContainerPane = new DockablePane(DockManager, dock);
                ContainerPane.Add(this);
                ContainerPane.Show();

                DockManager.Add(ContainerPane as DockablePane);
            }
            else
            {
                ContainerPane.Show(this);
                ContainerPane.Show();
            }
        }

        /// <summary>
        /// Show content into default documents pane
        /// </summary>
        public void ShowAsDocument()
        {
            if (ContainerPane == null)
            {
                ContainerPane = DockManager.AddDocument(this);
            }

            ContainerPane.Show(this);
        }

        /// <summary>
        /// Hides content from container pane
        /// </summary>
        /// <remarks>If container pane doesn't contain any more content, it is automaticly hidden.</remarks>
        public virtual new void Hide() => ContainerPane.Hide(this);

        public virtual void ChangeDock(Docks dock) { }

        public virtual void Float() { }

        public virtual void AutoHide() { }
    }
}
