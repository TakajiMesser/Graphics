using OpenTK;
using SauceEditor.Models;
using SauceEditor.Utilities;
using SauceEditor.ViewModels.Behaviors;
using SauceEditor.ViewModels.Commands;
using SauceEditor.Views.Factories;
using SauceEditor.Views.Properties;
using SpiceEngine.Entities;
using SpiceEngine.Maps;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Xceed.Wpf.AvalonDock;
using ICommand = SauceEditor.ViewModels.Commands.ICommand;

namespace SauceEditor.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        private readonly IWindowFactory _windowFactory;
        private readonly IMainViewFactory _mainViewFactory;

        public CommandStack CommandStack { get; private set; } = new CommandStack();

        public DockingManager MainDock { get; set; }
        public DockingManager SideDock { get; set; }

        public List<ViewModel> MainDockViewModels { get; set; } = new List<ViewModel>();
        public List<ViewModel> SideDockViewModels { get; set; } = new List<ViewModel>();

        public string Title { get; set; }
        public bool IsPlayable { get; set; }
        public Visibility PlayVisibility { get; set; }

        private ProjectTreePanelViewModel _projectTreePanelViewModel;
        private ToolsPanelViewModel _toolsPanelViewModel;

        public GamePanelManagerViewModel GamePanelManagerViewModel { get; set; }
        private BehaviorViewModel _behaviorViewModel;
        private ScriptViewModel _scriptViewModel;

        private SettingsWindowViewModel _settingsWindowViewModel;

        public EditorSettings Settings { get; set; }

        public PropertiesViewModel PropertiesViewModel { get; set; }

        private RelayCommand _undoCommand;
        private RelayCommand _redoCommand;
        private RelayCommand _playCommand;
        private RelayCommand _openMapCommand;

        public RelayCommand UndoCommand
        {
            get
            {
                if (_undoCommand == null)
                {
                    _undoCommand = new RelayCommand(
                        p => CommandStack.Undo(),
                        p => CommandStack.CanUndo
                    );
                }

                return _undoCommand;
            }
        }

        public RelayCommand RedoCommand
        {
            get
            {
                if (_redoCommand == null)
                {
                    _redoCommand = new RelayCommand(
                        p => CommandStack.Redo(),
                        p => CommandStack.CanRedo
                    );
                }

                return _redoCommand;
            }
        }

        public RelayCommand PlayCommand
        {
            get
            {
                if (_playCommand == null)
                {
                    _playCommand = new RelayCommand(
                        p => _windowFactory.CreateGameWindow(null),
                        p => true //???
                    );
                }

                return _playCommand;
            }
        }

        public RelayCommand OpenMapCommand
        {
            get
            {
                if (_openMapCommand == null)
                {
                    _openMapCommand = new RelayCommand(
                        p =>
                        {
                            Title = Path.GetFileNameWithoutExtension(/*filePath*/"") + " - " + "SauceEditor";
                            PlayVisibility = Visibility.Visible;
                            //_mainViewFactory.CreateGamePanelManager();
                        },
                        p => true //???
                    );
                }

                return _openMapCommand;
            }
        }

        public MainWindowViewModel()
        {
            Title = "Sauce Editor";
        }

        public void OnPropertiesViewModelChanged()
        {
            AddChild(PropertiesViewModel, (s, args) => GamePanelManagerViewModel?.RequestUpdate());//EntityUpdated?.Invoke(this, new EntityEventArgs(PropertiesViewModel.EditorEntity)));
        }

        public event EventHandler<EntityEventArgs> EntityUpdated;

        /*public ICommand EntityPropertiesUpdatedCommand => _entityPropertiesUpdatedCommand ?? new RelayCommand(
            p => ScriptOpened?.Invoke(this, new FileEventArgs(_behaviorFilePath)),
            p => _behaviorFilePath != null
        );*/

        public void LoadOrCreateSettings()
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