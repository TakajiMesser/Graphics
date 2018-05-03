using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TakoEngine.Entities;
using TakoEngine.Entities.Lights;
using SauceEditor.Utilities;
using Brush = TakoEngine.Entities.Brush;
using SauceEditor.Controls.Transform;

namespace SauceEditor.Controls.Tools
{
    public enum Tools
    {
        Brush,
        Mesh,
        Texture
    }

    /// <summary>
    /// Interaction logic for PropertyWindow.xaml
    /// </summary>
    /// 
    public partial class ToolsWindow : DockingLibrary.DockableContent
    {
        public Tools SelectedTool { get; set; }

        public ToolsWindow()
        {
            InitializeComponent();
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
        }

        private void BrushButton_Click(object sender, RoutedEventArgs e)
        {
            EnableAllToolButtons();
            BrushButton.IsEnabled = false;
        }

        private void MeshButton_Click(object sender, RoutedEventArgs e)
        {
            EnableAllToolButtons();
            MeshButton.IsEnabled = false;
        }

        private void TextureButton_Click(object sender, RoutedEventArgs e)
        {
            EnableAllToolButtons();
            TextureButton.IsEnabled = false;
        }
    }
}
