using SauceEditor.ViewModels.Docks;
using SauceEditorCore.Models.Components;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LibraryManager = SauceEditorCore.Models.Libraries.LibraryManager;
using SauceEditorCore.Models.Libraries;

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
        public LibraryInfo.SortStyles SortStyle { get; set; }
        public LibraryViewTypes ViewType { get; set; }

        public LibraryPanelViewModel() : base(DockTypes.Property) => PathInfos = new ReadOnlyCollection<IPathInfo>(_pathInfos);

        public void UpdateFromModel(LibraryManager libraryManager)
        {
            _libraryManager = libraryManager;

            // For now, just load and add arbritrary components for testing purposes
            _libraryManager.Load();
            _components.Add(libraryManager.MapLibrary.GetInfoAt(0));
        }

        public void NavigateNodeForward(string name)
        {
            _currentNode = _currentNode.GetChild(name);
        }

        public void NavigateNodeBackward()
        {
            _currentNode = _currentNode.Parent;
        }

        private void AddComponentsFromNode(LibraryNode node)
        {
            _pathInfos.Clear();
            _pathInfos.AddRange(node.);
        }

        public void OnSortStyleChanged()
        {

        }

        public void OnViewTypeChanged()
        {
            switch (ViewType)
            {
                case LibraryViewTypes.Path:
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