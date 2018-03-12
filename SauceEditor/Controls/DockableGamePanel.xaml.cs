using DockingLibrary;
using TakoEngine.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.ComponentModel;

namespace SauceEditor.Controls
{
    /// <summary>
    /// Interaction logic for DockableGamePanel.xaml
    /// </summary>
    public partial class DockableGamePanel : DockableContent
    {
        public GamePanel GamePanel { get; set; }

        public DockableGamePanel()
        {
            InitializeComponent();
        }

        public DockableGamePanel(string mapPath, DockManager dockManager) : base(dockManager)
        {
            InitializeComponent();
            GamePanel = new GamePanel(mapPath)
            {
                Dock = System.Windows.Forms.DockStyle.Fill
            };

            var host = new System.Windows.Forms.Integration.WindowsFormsHost
            {
                Child = GamePanel
            };
            MainDock.Children.Add(host);

            // Default to wireframe rendering
            WireframeButton.IsEnabled = false;
            GamePanel.RenderMode = TakoEngine.Rendering.Processing.RenderModes.Wireframe;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            //DockManager.ParentWindow = this;
            //Grid.Children.Add(GameWindow);
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            //GamePanel?.Close();
            base.OnClosing(e);
        }

        private void WireframeButton_Click(object sender, RoutedEventArgs e)
        {
            WireframeButton.IsEnabled = false;
            DiffuseButton.IsEnabled = true;
            LitButton.IsEnabled = true;
            GamePanel.RenderMode = TakoEngine.Rendering.Processing.RenderModes.Wireframe;
        }

        private void DiffuseButton_Click(object sender, RoutedEventArgs e)
        {
            WireframeButton.IsEnabled = true;
            DiffuseButton.IsEnabled = false;
            LitButton.IsEnabled = true;
            GamePanel.RenderMode = TakoEngine.Rendering.Processing.RenderModes.Diffuse;
        }

        private void LitButton_Click(object sender, RoutedEventArgs e)
        {
            WireframeButton.IsEnabled = true;
            DiffuseButton.IsEnabled = true;
            LitButton.IsEnabled = false;
            GamePanel.RenderMode = TakoEngine.Rendering.Processing.RenderModes.Lit;
        }
    }
}
