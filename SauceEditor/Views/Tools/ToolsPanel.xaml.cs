using System;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock.Layout;

namespace SauceEditor.Views.Tools
{
    /// <summary>
    /// Interaction logic for ToolsPanel.xaml
    /// </summary>
    public partial class ToolsPanel : LayoutAnchorable
    {
        public SpiceEngine.Game.Tools SelectedTool { get; set; }

        public event EventHandler<ToolSelectedEventArgs> ToolSelected;

        public ToolsPanel()
        {
            InitializeComponent();

            SelectButton.IsEnabled = false;
            SelectedTool = SpiceEngine.Game.Tools.Selector;
        }

        private void EnableAllToolButtons()
        {
            foreach (var child in ButtonPanel.Children)
            {
                if (child is Button button)
                {
                    button.IsEnabled = true;
                }
            }
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            EnableAllToolButtons();
            SelectButton.IsEnabled = false;

            ToolSelected?.Invoke(this, new ToolSelectedEventArgs(SpiceEngine.Game.Tools.Selector, SelectedTool));
            SelectedTool = SpiceEngine.Game.Tools.Selector;
        }

        private void VolumeButton_Click(object sender, RoutedEventArgs e)
        {
            EnableAllToolButtons();
            VolumeButton.IsEnabled = false;

            ToolSelected?.Invoke(this, new ToolSelectedEventArgs(SpiceEngine.Game.Tools.Volume, SelectedTool));
            SelectedTool = SpiceEngine.Game.Tools.Volume;
        }

        private void BrushButton_Click(object sender, RoutedEventArgs e)
        {
            EnableAllToolButtons();
            BrushButton.IsEnabled = false;

            ToolSelected?.Invoke(this, new ToolSelectedEventArgs(SpiceEngine.Game.Tools.Brush, SelectedTool));
            SelectedTool = SpiceEngine.Game.Tools.Brush;
        }

        private void MeshButton_Click(object sender, RoutedEventArgs e)
        {
            EnableAllToolButtons();
            MeshButton.IsEnabled = false;

            ToolSelected?.Invoke(this, new ToolSelectedEventArgs(SpiceEngine.Game.Tools.Mesh, SelectedTool));
            SelectedTool = SpiceEngine.Game.Tools.Mesh;
        }

        private void TextureButton_Click(object sender, RoutedEventArgs e)
        {
            EnableAllToolButtons();
            TextureButton.IsEnabled = false;

            ToolSelected?.Invoke(this, new ToolSelectedEventArgs(SpiceEngine.Game.Tools.Texture, SelectedTool));
            SelectedTool = SpiceEngine.Game.Tools.Texture;
        }
    }
}
