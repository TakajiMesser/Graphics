using OpenTK;
using SauceEditor.Models;
using SauceEditor.Utilities;
using SpiceEngine.Entities;
using SpiceEngine.Maps;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SauceEditor.ViewModels
{
    public class ProjectTreePanelViewModel : ViewModel
    {
        private GameProject _gameProject;

        private string _projectPath;
        private string _projectType;

        private RelayCommand _addMapCommand;
        private RelayCommand _addModelCommand;
        private RelayCommand _addBehaviorCommand;
        private RelayCommand _addTextureCommand;
        private RelayCommand _addAudioCommand;
        private RelayCommand _openMapCommand;
        private RelayCommand _openModelCommand;
        private RelayCommand _openBehaviorCommand;
        private RelayCommand _openTextureCommand;
        private RelayCommand _openAudioCommand;
        private RelayCommand _excludeCommand;
        private RelayCommand _deleteCommand;
        private RelayCommand _renameCommand;

        public RelayCommand AddMapCommand
        {
            get
            {
                if (_addMapCommand == null)
                {
                    _addMapCommand = new RelayCommand(
                        p => ScriptOpened?.Invoke(this, new FileEventArgs(_behaviorFilePath))
                    );
                }

                return _addMapCommand;
            }
        }

        public RelayCommand AddModelCommand
        {
            get
            {
                if (_addModelCommand == null)
                {
                    _addModelCommand = new RelayCommand(
                        p => ScriptOpened?.Invoke(this, new FileEventArgs(_behaviorFilePath))
                    );
                }

                return _addModelCommand;
            }
        }

        public RelayCommand AddBehaviorCommand
        {
            get
            {
                if (_addBehaviorCommand == null)
                {
                    _addBehaviorCommand = new RelayCommand(
                        p => ScriptOpened?.Invoke(this, new FileEventArgs(_behaviorFilePath))
                    );
                }

                return _addBehaviorCommand;
            }
        }

        public RelayCommand AddTextureCommand
        {
            get
            {
                if (_addTextureCommand == null)
                {
                    _addTextureCommand = new RelayCommand(
                        p => ScriptOpened?.Invoke(this, new FileEventArgs(_behaviorFilePath))
                    );
                }

                return _addTextureCommand;
            }
        }

        public RelayCommand AddAudioCommand
        {
            get
            {
                if (_addAudioCommand == null)
                {
                    _addAudioCommand = new RelayCommand(
                        p => ScriptOpened?.Invoke(this, new FileEventArgs(_behaviorFilePath))
                    );
                }

                return _addAudioCommand;
            }
        }

        public RelayCommand OpenMapCommand
        {
            get
            {
                if (_openMapCommand == null)
                {
                    _openMapCommand = new RelayCommand(
                        p => ScriptOpened?.Invoke(this, new FileEventArgs(_behaviorFilePath))
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
                        p => ScriptOpened?.Invoke(this, new FileEventArgs(_behaviorFilePath))
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
                        p => ScriptOpened?.Invoke(this, new FileEventArgs(_behaviorFilePath))
                    );
                }

                return _openBehaviorCommand;
            }
        }

        public RelayCommand OpenTextureCommand
        {
            get
            {
                if (_openTextureCommand == null)
                {
                    _openTextureCommand = new RelayCommand(
                        p => ScriptOpened?.Invoke(this, new FileEventArgs(_behaviorFilePath))
                    );
                }

                return _openTextureCommand;
            }
        }

        public RelayCommand OpenAudioCommand
        {
            get
            {
                if (_openAudioCommand == null)
                {
                    _openAudioCommand = new RelayCommand(
                        p => ScriptOpened?.Invoke(this, new FileEventArgs(_behaviorFilePath))
                    );
                }

                return _openAudioCommand;
            }
        }

        public RelayCommand ExcludeCommand
        {
            get
            {
                if (_excludeCommand == null)
                {
                    _excludeCommand = new RelayCommand(
                        p => ScriptOpened?.Invoke(this, new FileEventArgs(_behaviorFilePath))
                    );
                }

                return _excludeCommand;
            }
        }

        public RelayCommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new RelayCommand(
                        p => ScriptOpened?.Invoke(this, new FileEventArgs(_behaviorFilePath))
                    );
                }

                return _deleteCommand;
            }
        }

        public RelayCommand RenameCommand
        {
            get
            {
                if (_renameCommand == null)
                {
                    _renameCommand = new RelayCommand(
                        p => ScriptOpened?.Invoke(this, new FileEventArgs(_behaviorFilePath))
                    );
                }

                return _renameCommand;
            }
        }

        public EditorEntity EditorEntity { get; set; }

        public string ProjectType
        {
            get => _projectType;
            set => SetProperty(ref _projectType, value);
        }

        public void SetValues(GameProject gameProject)
        {
            _gameProject = gameProject;

            
            EntityType = editorEntity != null ? editorEntity.Entity.GetType().Name : "No Properties to Show";

            // Unnecessary since every command can always be executed
            // CommandManager.InvalidateRequerySuggested();
        }
    }
}