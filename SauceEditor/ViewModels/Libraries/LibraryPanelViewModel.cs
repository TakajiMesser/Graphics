using SauceEditor.ViewModels.Docks;
using SauceEditorCore.Helpers;
using SauceEditorCore.Models.Components;
using SauceEditorCore.Models.Libraries;
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

    public class LibraryPanelViewModel : DockViewModel
    {
        private LibraryManager _libraryManager = new LibraryManager();
        private List<IPathInfo> _pathInfos = new List<IPathInfo>();
        private LibraryNode _currentNode;

        public ReadOnlyCollection<IPathInfo> PathInfos { get; set; }
        public PathSortStyles SortStyle { get; set; }
        public LibraryViewTypes ViewType { get; set; }

        public LibraryPanelViewModel() : base(DockTypes.Property)
        {
            PathInfos = new ReadOnlyCollection<IPathInfo>(_pathInfos);
            //var observableCollection = new ObservableCollection<IPathInfo>(_pathInfos);
            //PathInfos = new ReadOnlyObservableCollection<IPathInfo>(observableCollection);
        }

        public void UpdateFromModel(LibraryManager libraryManager)
        {
            _libraryManager = libraryManager;

            // For now, just load and add arbritrary components for testing purposes
            _libraryManager.Load();

            _currentNode = _libraryManager.RootNode;
            _pathInfos.Clear();
            _pathInfos.AddRange(_libraryManager.GetNodePathInfos(_currentNode.Path));
            PathInfos = new ReadOnlyCollection<IPathInfo>(_pathInfos);
        }

        public void NavigateNodeForward(string name)
        {
            _currentNode = _currentNode.GetChild(name);
            _pathInfos.Clear();
            _pathInfos.AddRange(_libraryManager.GetNodePathInfos(_currentNode.Path));
            PathInfos = new ReadOnlyCollection<IPathInfo>(_pathInfos);
        }

        public void NavigateNodeBackward()
        {
            _currentNode = _currentNode.Parent;
            _pathInfos.Clear();
            _pathInfos.AddRange(_libraryManager.GetNodePathInfos(_currentNode.Path));
            PathInfos = new ReadOnlyCollection<IPathInfo>(_pathInfos);
        }

        public void OnSortStyleChanged()
        {
            // TODO - Determine, based on sort style, if we need to update info from disk
            if (SortStyle.NeedsRecursiveRefresh())
            {
                foreach (var pathInfo in _pathInfos)
                {
                    RecursiveRefresh(pathInfo);
                }
            }
            else if (SortStyle.NeedsRefresh())
            {
                foreach (var pathInfo in _pathInfos)
                {
                    pathInfo.Refresh();
                }
            }

            var sortedPathInfos = _pathInfos.OrderBy(SortStyle).ToList();
            _pathInfos.Clear();
            _pathInfos.AddRange(sortedPathInfos);
            PathInfos = new ReadOnlyCollection<IPathInfo>(_pathInfos);
        }

        private void RecursiveRefresh(IPathInfo pathInfo)
        {
            if (pathInfo is LibraryInfo libraryInfo)
            {
                for (var i = 0; i < libraryInfo.Count; i++)
                {
                    var childPathInfo = libraryInfo.GetInfoAt(i);
                    RecursiveRefresh(childPathInfo);
                }

                libraryInfo.Refresh();
            }
            else if (pathInfo is ComponentInfo componentInfo)
            {
                componentInfo.Refresh();
            }
        }

        public void OnViewTypeChanged()
        {
            switch (ViewType)
            {
                case LibraryViewTypes.Path:
                    _currentNode = _libraryManager.RootNode;
                    _pathInfos.Clear();
                    _pathInfos.AddRange(_libraryManager.GetNodePathInfos(_currentNode.Path));
                    break;
                case LibraryViewTypes.Type:
                    break;
            }
        }

        private void GetComponentsForPath(string path)
        {
            _currentNode = _currentNode ?? _libraryManager.RootNode;

            var startIndex = path.IndexOf(_currentNode.Path);
            var currentPath = 
            _currentNode.Path;
        }

        private void DoShit()
        {
            /*
            LibraryManager
                
                LibraryNode
                    Name
                    Components (generic type)
                    Child Nodes
                
                Library<MapComponent>
                    Path
                    Components (of specific type)
                    LibraryInfo
                        ComponentInfos
                            Name
                            Path
                            Exists
                            FileSize
                            CreationTime
                            LastWriteTime
                            LastAccessTime
                            PreviewIcon

            -We want a button that can switch between DirectoryView and LibraryView
            -We want a button that can switch SortStyle

            */
        }
    }
}