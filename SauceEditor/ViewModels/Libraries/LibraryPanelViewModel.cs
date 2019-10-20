using SauceEditor.ViewModels.Docks;
using SauceEditor.Views.Factories;
using SauceEditorCore.Helpers;
using SauceEditorCore.Models.Components;
using SauceEditorCore.Models.Libraries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SauceEditor.ViewModels.Libraries
{
    public enum LibraryViewTypes
    {
        Path,
        Type
    }

    public class LibraryPanelViewModel : DockableViewModel, ILibraryTracker
    {
        private ILibraryFactory _libraryFactory;
        private IComponentFactory _componentFactory;

        private List<PathInfoViewModel> _children = new List<PathInfoViewModel>();
        private LibraryNode _currentNode;
        private bool _isBase = false;

        public ObservableCollection<PathInfoViewModel> Children { get; set; }
        public PathSortStyles SortStyle { get; set; }
        public LibraryViewTypes ViewType { get; set; }

        private RelayCommand _backCommand;
        public RelayCommand BackCommand
        {
            get
            {
                return _backCommand ?? (_backCommand = new RelayCommand(
                    p =>
                    {
                        switch (ViewType)
                        {
                            case LibraryViewTypes.Path:
                                _currentNode = _currentNode.Parent;
                                var nodeInfo = _libraryFactory.GetNodeInfo(_currentNode.Path);
                                SwapChildren(LibraryInfoViewModel.CreateChildren(nodeInfo, this));
                                break;
                            case LibraryViewTypes.Type:
                                LoadBaseLibrary();
                                break;
                        }
                    },
                    p =>
                    {
                        switch (ViewType)
                        {
                            case LibraryViewTypes.Path:
                                return _currentNode != null && _currentNode.Parent != null;
                            case LibraryViewTypes.Type:
                                return !_isBase;
                        }

                        return false;
                    }
                ));
            }
        }

        public void UpdateFromModel(ILibraryFactory libraryFactory, IComponentFactory componentFactory)
        {
            _libraryFactory = libraryFactory;
            _componentFactory = componentFactory;

            // For now, just load and add arbritrary components for testing purposes
            _libraryFactory.Load();
            LoadRootNode();
        }

        public void OpenLibrary(string name, IEnumerable<PathInfoViewModel> items)
        {
            // Also track current library path -> either update current node or we know that we're no longer at base library
            switch (ViewType)
            {
                case LibraryViewTypes.Path:
                    _currentNode = _currentNode.GetChild(name);
                    break;
                case LibraryViewTypes.Type:
                    _isBase = false;
                    break;
            }

            // TODO - Update "path" button trail, rather than having a single back button
            SwapChildren(items);
        }

        public void OpenComponent(ComponentInfo componentInfo)
        {
            var component = _libraryFactory.GetComponent(componentInfo.Path);

            if (component is MapComponent)
            {
                _componentFactory.OpenMap(component.Path);
            }
            else if (component is ModelComponent)
            {
                _componentFactory.OpenModel(component.Path);
            }
            else if (component is BehaviorComponent)
            {
                _componentFactory.OpenBehavior(component.Path);
            }
            else if (component is TextureComponent)
            {
                _componentFactory.OpenTexture(component.Path);
            }
            else if (component is SoundComponent)
            {
                _componentFactory.OpenSound(component.Path);
            }
            else if (component is MaterialComponent)
            {
                _componentFactory.OpenMaterial(component.Path);
            }
            else if (component is ArchetypeComponent)
            {
                _componentFactory.OpenArchetype(component.Path);
            }
            else if (component is ScriptComponent)
            {
                _componentFactory.OpenScript(component.Path);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void OnSortStyleChanged()
        {
            _children.Select(c => c.PathInfo).Refresh(SortStyle);
            var keySelector = PathInfoHelper.GetKeySelector(SortStyle);
            var sortedChildren = _children.OrderBy(c => keySelector(c.PathInfo)).ToList();
            //var sortedChildren = _children.OrderBy(c => c.PathInfo.GetKeySelector(SortStyle)).ToList();

            SwapChildren(sortedChildren);
        }

        public void OnViewTypeChanged()
        {
            switch (ViewType)
            {
                case LibraryViewTypes.Path:
                    LoadRootNode();
                    break;
                case LibraryViewTypes.Type:
                    LoadBaseLibrary();
                    break;
            }
        }

        private void LoadRootNode()
        {
            _currentNode = _libraryFactory.RootNode;
            var nodeInfo = _libraryFactory.GetNodeInfo(_currentNode.Path);
            SwapChildren(LibraryInfoViewModel.CreateChildren(nodeInfo, this));
        }

        private void LoadBaseLibrary()
        {
            _isBase = true;
            SwapChildren(_libraryFactory.GetBaseLibraryPaths().Select(l => new LibraryInfoViewModel(l, this)));
        }

        private void SwapChildren(IEnumerable<PathInfoViewModel> items)
        {
            // TODO - Is this still necessary?
            items = items.ToList();

            _children.Clear();
            _children.AddRange(items);

            Children = new ObservableCollection<PathInfoViewModel>(_children);
            BackCommand.InvokeCanExecuteChanged();
        }
    }
}