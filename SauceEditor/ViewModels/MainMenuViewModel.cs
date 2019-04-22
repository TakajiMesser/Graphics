using Microsoft.Win32;
using OpenTK;
using SauceEditor.Models;
using SauceEditor.Models.Components;
using SauceEditor.Utilities;
using SauceEditor.Views.Factories;
using SauceEditor.Views.Properties;
using SpiceEngine.Entities;
using SpiceEngine.Maps;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SauceEditor.ViewModels
{
    public class MainMenuViewModel : ViewModel
    {
        private const string DEFAULT_INITIAL_DIRECTORY = @"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\SpiceEngine\SampleGameProject\Maps";

        private RelayCommand _newProjectCommand;
        private RelayCommand _newMapCommand;
        private RelayCommand _newModelCommand;
        private RelayCommand _newBehaviorCommand;

        private RelayCommand _openProjectCommand;
        private RelayCommand _openMapCommand;
        private RelayCommand _openModelCommand;
        private RelayCommand _openBehaviorCommand;

        private RelayCommand _saveAllCommand;
        private RelayCommand _settingsCommand;

        public IMainViewFactory MainViewFactory { get; set; }

        public RelayCommand NewProjectCommand
        {
            get
            {
                if (_newProjectCommand == null)
                {
                    _newProjectCommand = new RelayCommand(
                        p =>
                        {
                            
                        },
                        p => true
                    );
                }

                return _newProjectCommand;
            }
        }

        public RelayCommand NewMapCommand
        {
            get
            {
                if (_newMapCommand == null)
                {
                    _newMapCommand = new RelayCommand(
                        p =>
                        {
                            
                        },
                        p => true
                    );
                }

                return _newMapCommand;
            }
        }

        public RelayCommand NewModelCommand
        {
            get
            {
                if (_newModelCommand == null)
                {
                    _newModelCommand = new RelayCommand(
                        p =>
                        {
                            
                        },
                        p => true
                    );
                }

                return _newModelCommand;
            }
        }

        public RelayCommand NewBehaviorCommand
        {
            get
            {
                if (_newBehaviorCommand == null)
                {
                    _newBehaviorCommand = new RelayCommand(
                        p =>
                        {

                        },
                        p => true
                    );
                }

                return _newBehaviorCommand;
            }
        }

        public RelayCommand OpenProjectCommand
        {
            get
            {
                if (_openProjectCommand == null)
                {
                    _openProjectCommand = new RelayCommand(
                        p =>
                        {
                            var fileName = OpenDialog(Project.FILE_EXTENSION, "Project Files|*.pro", DEFAULT_INITIAL_DIRECTORY);
                            if (fileName != null)
                            {
                                MainViewFactory.OpenProject(fileName);
                            }
                        },
                        p => true
                    );
                }

                return _openProjectCommand;
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
                            var fileName = OpenDialog("map", "Map Files|*.map", DEFAULT_INITIAL_DIRECTORY);
                            if (fileName != null)
                            {
                                MainViewFactory.OpenMap(fileName);
                            }
                        },
                        p => true
                    );
                }

                return _openMapCommand;
            }
        }

        public RelayCommand OpenModelCommand
        {
            get
            {
                if (_openModelCommand == null)
                {
                    _openModelCommand = new RelayCommand(
                        p =>
                        {
                            var fileName = OpenDialog("obj", "Model Files|*.obj", DEFAULT_INITIAL_DIRECTORY);
                            if (fileName != null)
                            {
                                MainViewFactory.OpenModel(fileName);
                            }
                        },
                        p => true
                    );
                }

                return _openModelCommand;
            }
        }

        public RelayCommand OpenBehaviorCommand
        {
            get
            {
                if (_openBehaviorCommand == null)
                {
                    _openBehaviorCommand = new RelayCommand(
                        p =>
                        {

                        },
                        p => true
                    );
                }

                return _openBehaviorCommand;
            }
        }

        public RelayCommand SaveAllCommand
        {
            get
            {
                if (_saveAllCommand == null)
                {
                    _saveAllCommand = new RelayCommand(
                        p =>
                        {

                        },
                        p => true
                    );
                }

                return _saveAllCommand;
            }
        }

        public RelayCommand SettingsCommand
        {
            get
            {
                if (_settingsCommand == null)
                {
                    _settingsCommand = new RelayCommand(
                        p =>
                        {

                        },
                        p => true
                    );
                }

                return _settingsCommand;
            }
        }

        private string OpenDialog(string defaultExt, string filter, string initialDirectory)
        {
            var dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = defaultExt,
                Filter = filter,
                InitialDirectory = initialDirectory
            };

            return dialog.ShowDialog() == true
                ? dialog.FileName
                : null;
        }
    }
}