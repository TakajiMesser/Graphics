using SauceEditor.Helpers.Builders;
using SauceEditor.Models;
using SauceEditor.ViewModels.Behaviors;
using SauceEditor.ViewModels.Commands;
using SauceEditor.Views.Factories;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Xceed.Wpf.AvalonDock;
using ICommand = SauceEditor.ViewModels.Commands.ICommand;

namespace SauceEditor.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        public IWindowFactory WindowFactory { get; set; }
        public IMainViewFactory MainViewFactory { get; set; }

        public CommandStack CommandStack { get; private set; } = new CommandStack();

        public DockingManager MainDock { get; set; }
        public DockingManager SideDock { get; set; }

        public List<ViewModel> MainDockViewModels { get; set; } = new List<ViewModel>();
        public List<ViewModel> SideDockViewModels { get; set; } = new List<ViewModel>();

        public string Title { get; set; }
        public bool IsPlayable { get; set; }
        public Visibility PlayVisibility { get; set; }

        public ProjectTreePanelViewModel ProjectTreePanelViewModel { get; set; }
        public ToolsPanelViewModel ToolsPanelViewModel { get; set; }
        public PropertiesViewModel PropertiesViewModel { get; set; }

        public GamePanelManagerViewModel GamePanelManagerViewModel { get; set; }
        public BehaviorViewModel BehaviorViewModel { get; set; }
        public ScriptViewModel ScriptViewModel { get; set; }

        public SettingsWindowViewModel SettingsWindowViewModel { get; set; }

        public EditorSettings Settings { get; set; }


        private RelayCommand _undoCommand;
        public RelayCommand UndoCommand
        {
            get
            {
                return _undoCommand ?? (_undoCommand = new RelayCommand(
                    p => CommandStack.Undo(),
                    p => CommandStack.CanUndo
                ));
            }
        }


        private RelayCommand _redoCommand;
        public RelayCommand RedoCommand
        {
            get
            {
                return _redoCommand ?? (_redoCommand = new RelayCommand(
                    p => CommandStack.Redo(),
                    p => CommandStack.CanRedo
                ));
            }
        }


        private RelayCommand _playCommand;
        public RelayCommand PlayCommand
        {
            get
            {
                return _playCommand ?? (_playCommand = new RelayCommand(
                    p => WindowFactory.CreateGameWindow(null),
                    p => true //???
                ));
            }
        }

        public MainWindowViewModel()
        {
            Title = "Sauce Editor";

            LoadOrCreateSettings();
            MapBuilder.CreateTestProject();
        }

        public void OnPropertiesViewModelChanged()
        {
            AddChild(PropertiesViewModel, (s, args) => GamePanelManagerViewModel?.RequestUpdate());
        }

        /*public ICommand EntityPropertiesUpdatedCommand => _entityPropertiesUpdatedCommand ?? new RelayCommand(
            p => ScriptOpened?.Invoke(this, new FileEventArgs(_behaviorFilePath)),
            p => _behaviorFilePath != null
        );*/

        private void LoadOrCreateSettings()
        {
            if (File.Exists(SauceEditor.Helpers.FilePathHelper.SETTINGS_PATH))
            {
                Settings = EditorSettings.Load(SauceEditor.Helpers.FilePathHelper.SETTINGS_PATH);
            }
            else
            {
                Settings = new EditorSettings();
                Settings.Save(SauceEditor.Helpers.FilePathHelper.SETTINGS_PATH);
            }
        }

        private void CommandExecuted(ICommand command)
        {
            CommandStack.Push(command);

            // TODO - Also need to enable/disable menu items for undo/redo
            /*if (CommandStack.CanUndo)
            {
                UndoButton.IsEnabled = true;
            }
            else
            {
                UndoButton.IsEnabled = false;
            }

            if (CommandStack.CanRedo)
            {
                RedoButton.IsEnabled = true;
            }
            else
            {
                RedoButton.IsEnabled = false;
            }*/
        }
    }
}