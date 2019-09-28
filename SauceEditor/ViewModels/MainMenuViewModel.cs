using Microsoft.Win32;
using SauceEditor.Models;
using SauceEditor.Models.Components;
using SauceEditor.Views.Factories;
using System;
using System.IO;

namespace SauceEditor.ViewModels
{
    public class MainMenuViewModel : ViewModel
    {
        public IMainView MainView { get; set; }
        //public IMainViewFactory MainViewFactory { get; set; }
        public IWindowFactory WindowFactory { get; set; }
        public IComponentFactory ComponentFactory { get; set; }

        private RelayCommand _newProjectCommand;
        public RelayCommand NewProjectCommand
        {
            get
            {
                return _newProjectCommand ?? (_newProjectCommand = new RelayCommand(
                    p => ComponentFactory.CreateProject(),
                    p => true
                ));
            }
        }

        private RelayCommand _newMapCommand;
        public RelayCommand NewMapCommand
        {
            get
            {
                return _newMapCommand ?? (_newMapCommand = new RelayCommand(
                    p => ComponentFactory.CreateMap(),
                    p => true
                ));
            }
        }

        private RelayCommand _newModelCommand;
        public RelayCommand NewModelCommand
        {
            get
            {
                return _newModelCommand ?? (_newModelCommand = new RelayCommand(
                    p => ComponentFactory.CreateModel(),
                    p => true
                ));
            }
        }

        private RelayCommand _newBehaviorCommand;
        public RelayCommand NewBehaviorCommand
        {
            get
            {
                return _newBehaviorCommand ?? (_newBehaviorCommand = new RelayCommand(
                    p => ComponentFactory.CreateBehavior(),
                    p => true
                ));
            }
        }

        private RelayCommand _newTextureCommand;
        public RelayCommand NewTextureCommand
        {
            get
            {
                return _newTextureCommand ?? (_newTextureCommand = new RelayCommand(
                    p => ComponentFactory.CreateTexture(),
                    p => true
                ));
            }
        }

        private RelayCommand _newSoundCommand;
        public RelayCommand NewSoundCommand
        {
            get
            {
                return _newSoundCommand ?? (_newSoundCommand = new RelayCommand(
                    p => ComponentFactory.CreateSound(),
                    p => true
                ));
            }
        }

        private RelayCommand _newMaterialCommand;
        public RelayCommand NewMaterialCommand
        {
            get
            {
                return _newMaterialCommand ?? (_newMaterialCommand = new RelayCommand(
                    p => ComponentFactory.CreateMaterial(),
                    p => true
                ));
            }
        }

        private RelayCommand _newArchetypeCommand;
        public RelayCommand NewArchetypeCommand
        {
            get
            {
                return _newArchetypeCommand ?? (_newArchetypeCommand = new RelayCommand(
                    p => ComponentFactory.CreateArchetype(),
                    p => true
                ));
            }
        }

        private RelayCommand _newScriptCommand;
        public RelayCommand NewScriptCommand
        {
            get
            {
                return _newScriptCommand ?? (_newScriptCommand = new RelayCommand(
                    p => ComponentFactory.CreateScript(),
                    p => true
                ));
            }
        }

        private RelayCommand _openProjectCommand;
        public RelayCommand OpenProjectCommand
        {
            get
            {
                return _openProjectCommand ?? (_openProjectCommand = new RelayCommand(
                    p =>
                    {
                        var fileName = OpenDialog(Project.FILE_EXTENSION, "Project Files|*.pro", EditorSettings.Instance.InitialProjectDirectory);
                        if (fileName != null)
                        {
                            ComponentFactory.OpenProject(fileName);
                        }
                    },
                    p => true
                ));
            }
        }

        private RelayCommand _openMapCommand;
        public RelayCommand OpenMapCommand
        {
            get
            {
                return _openMapCommand ?? (_openMapCommand = new RelayCommand(
                    p =>
                    {
                        var fileName = OpenDialog("map", "Map Files|*.map", EditorSettings.Instance.InitialMapDirectory);
                        if (fileName != null)
                        {
                            ComponentFactory.OpenMap(fileName);
                        }
                    },
                    p => true
                ));
            }
        }

        private RelayCommand _openModelCommand;
        public RelayCommand OpenModelCommand
        {
            get
            {
                return _openModelCommand ?? (_openModelCommand = new RelayCommand(
                    p =>
                    {
                        var fileName = OpenDialog("obj", "Model Files|*.obj", EditorSettings.Instance.InitialModelDirectory);
                        if (fileName != null)
                        {
                            ComponentFactory.OpenMap(fileName);
                        }
                    },
                    p => true
                ));
            }
        }

        private RelayCommand _openBehaviorCommand;
        public RelayCommand OpenBehaviorCommand
        {
            get
            {
                return _openBehaviorCommand ?? (_openBehaviorCommand = new RelayCommand(
                    p =>
                    {
                        var fileName = OpenDialog("map", "Map Files|*.map", EditorSettings.Instance.InitialBehaviorDirectory);
                        if (fileName != null)
                        {
                            ComponentFactory.OpenMap(fileName);
                        }
                    },
                    p => true
                ));
            }
        }

        private RelayCommand _openTextureCommand;
        public RelayCommand OpenTextureCommand
        {
            get
            {
                return _openTextureCommand ?? (_openTextureCommand = new RelayCommand(
                    p =>
                    {
                        var fileName = OpenDialog("map", "Map Files|*.map", EditorSettings.Instance.InitialTextureDirectory);
                        if (fileName != null)
                        {
                            ComponentFactory.OpenMap(fileName);
                        }
                    },
                    p => true
                ));
            }
        }

        private RelayCommand _openSoundCommand;
        public RelayCommand OpenSoundCommand
        {
            get
            {
                return _openSoundCommand ?? (_openSoundCommand = new RelayCommand(
                    p =>
                    {
                        var fileName = OpenDialog("map", "Map Files|*.map", EditorSettings.Instance.InitialSoundDirectory);
                        if (fileName != null)
                        {
                            ComponentFactory.OpenMap(fileName);
                        }
                    },
                    p => true
                ));
            }
        }

        private RelayCommand _openMaterialCommand;
        public RelayCommand OpenMaterialCommand
        {
            get
            {
                return _openMaterialCommand ?? (_openMaterialCommand = new RelayCommand(
                    p =>
                    {
                        var fileName = OpenDialog("map", "Map Files|*.map", EditorSettings.Instance.InitialMaterialDirectory);
                        if (fileName != null)
                        {
                            ComponentFactory.OpenMap(fileName);
                        }
                    },
                    p => true
                ));
            }
        }

        private RelayCommand _openArchetypeCommand;
        public RelayCommand OpenArchetypeCommand
        {
            get
            {
                return _openArchetypeCommand ?? (_openArchetypeCommand = new RelayCommand(
                    p =>
                    {
                        var fileName = OpenDialog("map", "Map Files|*.map", EditorSettings.Instance.InitialArchetypeDirectory);
                        if (fileName != null)
                        {
                            ComponentFactory.OpenMap(fileName);
                        }
                    },
                    p => true
                ));
            }
        }

        private RelayCommand _openScriptCommand;
        public RelayCommand OpenScriptCommand
        {
            get
            {
                return _openScriptCommand ?? (_openScriptCommand = new RelayCommand(
                    p =>
                    {
                        var fileName = OpenDialog("map", "Map Files|*.map", EditorSettings.Instance.InitialScriptDirectory);
                        if (fileName != null)
                        {
                            ComponentFactory.OpenMap(fileName);
                        }
                    },
                    p => true
                ));
            }
        }

        private RelayCommand _saveAllCommand;
        public RelayCommand SaveAllCommand
        {
            get
            {
                return _saveAllCommand ?? (_saveAllCommand = new RelayCommand(
                    p => MainView.SaveAll(),
                    p => true
                ));
            }
        }

        private RelayCommand _settingsCommand;
        public RelayCommand SettingsCommand
        {
            get
            {
                return _settingsCommand ?? (_settingsCommand = new RelayCommand(
                    p => WindowFactory.CreateSettingsWindow(),
                    p => true
                ));
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
                InitialDirectory = NormalizedPath(initialDirectory)
            };

            return dialog.ShowDialog() == true
                ? dialog.FileName
                : null;
        }

        private string NormalizedPath(string path) => Path.GetFullPath(new Uri(path).LocalPath)
            .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            .ToUpperInvariant();
    }
}