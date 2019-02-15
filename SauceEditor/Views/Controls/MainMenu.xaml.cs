using Jidai.Helpers;
using Microsoft.Win32;
using OpenTK;
using SauceEditor.Models;
using SauceEditor.ViewModels.Commands;
using SauceEditor.Views.Controls.GamePanels;
using SauceEditor.Views.Controls.ProjectTree;
using SauceEditor.Views.Controls.Properties;
using SauceEditor.Views.Controls.Settings;
using SauceEditor.Views.Controls.Tools;
using SpiceEngine.Maps;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GameWindow = SpiceEngine.Game.GameWindow;

namespace SauceEditor.Views.Controls
{
    public class FileEventArgs : EventArgs
    {
        public string FileName { get; private set; }

        public FileEventArgs(string fileName) => FileName = fileName;
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainMenu : Menu
    {
        private SettingsWindow _settingsWindow;
        private EditorSettings _settings;

        private string _mapPath;

        public event EventHandler<FileEventArgs> MapOpened;
        public event EventHandler<FileEventArgs> ProjectOpened;
        public event EventHandler<FileEventArgs> ModelOpened;
        public event EventHandler<FileEventArgs> BehaviorOpened;

        public MainMenu()
        {
            PresentationTraceSources.DataBindingSource.Listeners.Add(new ConsoleTraceListener());
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Error;

            InitializeComponent();
        }

        private void NewMap()
        {

        }

        private void NewModel()
        {

        }

        private void NewBehavior()
        {

        }

        private void NewTexture()
        {

        }

        private void NewAudio()
        {

        }

        private void OpenModel(string filePath)
        {
            //PlayButton.Visibility = Visibility.Visible;

            /*var modelView = new DockableGamePanel(MainDockManager);
            modelView.Panel.LoadFromModel(filePath);
            modelView.EntitySelectionChanged += (s, args) =>
            {
                _propertyPanel.Entity = args.Entity;
                SideDockManager.ActiveContent = _propertyPanel;
            };
            modelView.Closed += (s, args) => PlayButton.Visibility = Visibility.Hidden;
            modelView.ShowAsDocument();*/

            //_propertyPanel.TransformChanged += (s, args) => modelView.Panel.Invalidate();
        }

        private void NewProjectCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void NewMapCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void NewModelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void NewBehaviorCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void OpenProjectCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void OpenMapCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void OpenModelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void OpenBehaviorCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void SaveAsCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void SaveAllCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            /*var titleBuilder = new StringBuilder("Document");

            int i = 1;
            while (ContainsDocument(titleBuilder.ToString() + i))
            {
                i++;
            }
            titleBuilder.Append(i);

            var doc = new DocWindow()
            {
                DockManager = MainDockManager,
                Title = titleBuilder.ToString()
            };
            doc.ShowAsDocument();*/

            //files.Add(new RecentFile(doc.Title, "PATH" + doc.Title, doc.Title.Length * i));
        }

        private void NewProjectCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new FileNameDialog();
            if (dialog.ShowDialog() == true)
            {

            }
        }

        private void NewMapCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void NewModelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void NewBehaviorCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void OpenProjectCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "Project Files|*.pro",
                DefaultExt = GameProject.FILE_EXTENSION,
                InitialDirectory = string.IsNullOrEmpty(_mapPath)
                    ? @"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\SpiceEngine\Jidai\Maps"
                    : System.IO.Path.GetDirectoryName(_mapPath)
            };

            if (dialog.ShowDialog() == true)
            {
                ProjectOpened?.Invoke(this, new FileEventArgs(dialog.FileName));
            }
        }

        private void OpenMapCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "map",
                Filter = "Map Files|*.map",
                InitialDirectory = string.IsNullOrEmpty(_mapPath)
                    ? @"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\SpiceEngine\Jidai\Maps"
                    : System.IO.Path.GetDirectoryName(_mapPath)
            };

            if (dialog.ShowDialog() == true)
            {
                MapOpened?.Invoke(this, new FileEventArgs(dialog.FileName));
            }
        }

        private void OpenModelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "obj",
                InitialDirectory = string.IsNullOrEmpty(_mapPath)
                    ? @"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\SpiceEngine\Jidai\Maps"
                    : System.IO.Path.GetDirectoryName(_mapPath)
            };

            if (dialog.ShowDialog() == true)
            {
                ModelOpened?.Invoke(this, new FileEventArgs(dialog.FileName));
            }
        }

        private void OpenBehaviorCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "bhv",
                InitialDirectory = string.IsNullOrEmpty(_mapPath)
                    ? @"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\SpiceEngine\Jidai\Maps"
                    : System.IO.Path.GetDirectoryName(_mapPath)
            };

            if (dialog.ShowDialog() == true)
            {
                BehaviorOpened?.Invoke(this, new FileEventArgs(dialog.FileName));
            }
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //_map.Save(_mapPath);
        }

        private void SaveAsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            /*var dialog = new OpenFileDialog()
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
                //RunButton.IsEnabled = true;
                Title = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName) + " - " + "SauceEditor";
            }*/
        }

        private void SaveAllCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            /*_map?.Save(_mapPath);
            _projectTree?.SaveProject();*/
        }

        private void Settings_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void Settings_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _settingsWindow = new SettingsWindow(_settings);
            _settingsWindow.SettingsChanged += (s, args) =>
            {
                _settings = args.Settings;
                _settings.Save(SauceEditor.Helpers.FilePathHelper.SETTINGS_PATH);
            };
            _settingsWindow.Show();
        }
    }
}
