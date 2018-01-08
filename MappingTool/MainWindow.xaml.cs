using Graphics.Maps;
using Microsoft.Win32;
using OpenTK;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using GameWindow = Graphics.GameObjects.GameWindow;

namespace MappingTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Map _map;
        private string _mapPath;
        private GameWindow _gameWindow;

        public MainWindow()
        {
            InitializeComponent();

            var item = new TreeViewItem()
            {
                Header = "Project"
            };
            ProjectTree.Add(item);
        }

        private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                CheckPathExists = true,
                DefaultExt = "map"
            };

            if (dialog.ShowDialog() == true)
            {
                _map = new Map();
                _map.Save(dialog.FileName);

                _mapPath = dialog.FileName;
                RunButton.IsEnabled = true;
                Title = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName) + " - " + "MappingTool";
            }
        }

        private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "map",
                InitialDirectory = string.IsNullOrEmpty(_mapPath)
                    ? @"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\Graphics\GraphicsTest\Maps"
                    : System.IO.Path.GetDirectoryName(_mapPath)
            };

            if (dialog.ShowDialog() == true)
            {
                _map = Map.Load(dialog.FileName);

                _mapPath = dialog.FileName;
                RunButton.IsEnabled = true;
                Title = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName) + " - " + "MappingTool";
            }
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _map.Save(_mapPath);
        }

        private void SaveAsCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SaveAsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "map",
                InitialDirectory = System.IO.Path.GetDirectoryName(_mapPath)
            };

            if (dialog.ShowDialog() == true)
            {
                _map.Save(dialog.FileName);

                _mapPath = dialog.FileName;
                RunButton.IsEnabled = true;
                Title = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName) + " - " + "MappingTool";
            }
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            _gameWindow = new GameWindow(_mapPath)
            {
                VSync = VSyncMode.Adaptive
            };
            _gameWindow.Run(60.0, 0.0);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _gameWindow?.Close();
            base.OnClosing(e);
        }
    }
}
