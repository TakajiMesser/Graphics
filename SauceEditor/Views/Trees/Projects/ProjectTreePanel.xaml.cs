using SauceEditor.ViewModels.Docks;
using Xceed.Wpf.AvalonDock.Layout;

namespace SauceEditor.Views.Trees.Projects
{
    /// <summary>
    /// Interaction logic for ProjectTreePanel.xaml
    /// </summary>
    public partial class ProjectTreePanel : LayoutAnchorable, IHaveDockViewModel
    {
        /*public event EventHandler<ItemSelectedEventArgs> MapSelected;
        public event EventHandler<ItemSelectedEventArgs> ModelSelected;
        public event EventHandler<ItemSelectedEventArgs> BehaviorSelected;
        public event EventHandler<ItemSelectedEventArgs> TextureSelected;
        public event EventHandler<ItemSelectedEventArgs> AudioSelected;

        private Project _project;*/

        public ProjectTreePanel()
        {
            InitializeComponent();
            //ClearProject();
        }

        public DockViewModel GetViewModel() => ViewModel;

        /*public void ClearProject()
        {
            _project = null;

            ProjectType.Content = "No Files to Show.";

            Tree.Items.Clear();
        }

        public void Add(TreeViewItem item)
        {
            //Tree.Items.Add(item);
        }

        public void SaveProject()
        {
            _project.Save();
        }

        public void OpenMap(string filePath)
        {
            ClearProject();

            _project = Project.Load(filePath);
            ProjectType.Content = "Map";

            var root = new TreeViewItem()
            {
                Header = Path.GetFileNameWithoutExtension(filePath),
                IsExpanded = true
            };

            var maps = new TreeViewItem()
            {
                Header = "Maps",
                IsExpanded = true
            };

            maps.MouseRightButtonDown += (s, args) =>
            {
                var item = s as TreeViewItem;
                item.IsSelected = true;
                item.ContextMenu = Tree.FindResource("MapsMenu") as ContextMenu;
            };

            var map = new TreeViewItem()
            {
                Header = Path.GetFileName(filePath)
            };

            map.MouseDoubleClick += (s, args) => MapSelected?.Invoke(this, new ItemSelectedEventArgs(filePath));
            map.MouseRightButtonDown += (s, args) =>
            {
                var item = s as TreeViewItem;
                item.IsSelected = true;
                item.ContextMenu = Tree.FindResource("MapMenu") as ContextMenu;
            };
            maps.Items.Add(map);

            root.Items.Add(maps);
            Tree.Items.Add(root);
        }

        public void OpenProject(string filePath)
        {
            ClearProject();

            _project = Project.Load(filePath);
            ProjectType.Content = "Project";

            var root = new TreeViewItem()
            {
                Header = Path.GetFileNameWithoutExtension(filePath),
                IsExpanded = true
            };

            root.Items.Add(LoadMaps());
            root.Items.Add(LoadModels());
            root.Items.Add(LoadBehaviors());
            root.Items.Add(LoadTextures());
            root.Items.Add(LoadAudio());

            Tree.Items.Add(root);
        }

        private TreeViewItem LoadMaps()
        {
            var maps = new TreeViewItem()
            {
                Header = "Maps",
                IsExpanded = true
            };

            maps.MouseRightButtonDown += (s, args) =>
            {
                var item = s as TreeViewItem;
                item.IsSelected = true;
                item.ContextMenu = Tree.FindResource("MapsMenu") as ContextMenu;
            };

            foreach (var map in _project.Maps)
            {
                var mapTreeItem = new TreeViewItem()
                {
                    Header = Path.GetFileName(map.Path)
                };

                mapTreeItem.MouseDoubleClick += (s, args) => MapSelected?.Invoke(this, new ItemSelectedEventArgs(map.Path));
                mapTreeItem.MouseRightButtonDown += (s, args) =>
                {
                    var item = s as TreeViewItem;
                    item.IsSelected = true;
                    item.ContextMenu = Tree.FindResource("MapMenu") as ContextMenu;
                };
                maps.Items.Add(mapTreeItem);
            }

            return maps;
        }

        private TreeViewItem LoadModels()
        {
            var models = new TreeViewItem()
            {
                Header = "Models",
                IsExpanded = true
            };

            models.MouseRightButtonDown += (s, args) =>
            {
                var item = s as TreeViewItem;
                item.IsSelected = true;
                item.ContextMenu = Tree.FindResource("ModelsMenu") as ContextMenu;
            };

            foreach (var model in _project.Models)
            {
                var modelTreeItem = new TreeViewItem()
                {
                    Header = Path.GetFileName(model.Path)
                };

                modelTreeItem.MouseDoubleClick += (s, args) => ModelSelected?.Invoke(this, new ItemSelectedEventArgs(model.Path));
                modelTreeItem.MouseRightButtonDown += (s, args) =>
                {
                    var item = s as TreeViewItem;
                    item.IsSelected = true;
                    item.ContextMenu = Tree.FindResource("ModelMenu") as ContextMenu;
                };
                models.Items.Add(modelTreeItem);
            }

            return models;
        }

        private TreeViewItem LoadBehaviors()
        {
            var behaviors = new TreeViewItem()
            {
                Header = "Behaviors",
                IsExpanded = true
            };

            behaviors.MouseRightButtonDown += (s, args) =>
            {
                var item = s as TreeViewItem;
                item.IsSelected = true;
                item.ContextMenu = Tree.FindResource("BehaviorsMenu") as ContextMenu;
            };

            foreach (var behavior in _project.Behaviors)
            {
                var behaviorTreeItem = new TreeViewItem()
                {
                    Header = Path.GetFileName(behavior.Path)
                };
                behaviorTreeItem.MouseDoubleClick += (s, args) => BehaviorSelected?.Invoke(this, new ItemSelectedEventArgs(behavior.Path));
                behaviorTreeItem.MouseRightButtonDown += (s, args) =>
                {
                    var item = s as TreeViewItem;
                    item.IsSelected = true;
                    item.ContextMenu = Tree.FindResource("BehaviorMenu") as ContextMenu;
                };
                behaviors.Items.Add(behaviorTreeItem);
            }

            return behaviors;
        }

        private TreeViewItem LoadTextures()
        {
            var textures = new TreeViewItem()
            {
                Header = "Textures",
                IsExpanded = true
            };

            textures.MouseRightButtonDown += (s, args) =>
            {
                var item = s as TreeViewItem;
                item.IsSelected = true;
                item.ContextMenu = Tree.FindResource("TexturesMenu") as ContextMenu;
            };

            foreach (var texture in _project.Textures)
            {
                var textureTreeItem = new TreeViewItem()
                {
                    Header = Path.GetFileName(texture.Path)
                };
                textureTreeItem.MouseDoubleClick += (s, args) => TextureSelected?.Invoke(this, new ItemSelectedEventArgs(texture.Path));
                textureTreeItem.MouseRightButtonDown += (s, args) =>
                {
                    var item = s as TreeViewItem;
                    item.IsSelected = true;
                    item.ContextMenu = Tree.FindResource("TextureMenu") as ContextMenu;
                };
                textures.Items.Add(textureTreeItem);
            }

            return textures;
        }

        private TreeViewItem LoadAudio()
        {
            var audios = new TreeViewItem()
            {
                Header = "Audio",
                IsExpanded = true
            };

            audios.MouseRightButtonDown += (s, args) =>
            {
                var item = s as TreeViewItem;
                item.IsSelected = true;
                item.ContextMenu = Tree.FindResource("AudiosMenu") as ContextMenu;
            };

            foreach (var sound in _project.Sounds)
            {
                var soundTreeItem = new TreeViewItem()
                {
                    Header = Path.GetFileName(sound.Path)
                };
                soundTreeItem.MouseDoubleClick += (s, args) => AudioSelected?.Invoke(this, new ItemSelectedEventArgs(sound.Path));
                soundTreeItem.MouseRightButtonDown += (s, args) =>
                {
                    var item = s as TreeViewItem;
                    item.IsSelected = true;
                    item.ContextMenu = Tree.FindResource("AudioMenu") as ContextMenu;
                };
                audios.Items.Add(soundTreeItem);
            }

            return audios;
        }

        private void AddMapCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void AddModelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void AddBehaviorCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void AddTextureCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void AddAudioCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void OpenMapCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void OpenModelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void OpenBehaviorCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void OpenTextureCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void OpenAudioCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void ExcludeCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void RenameCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void AddMapCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var item = e.Source as TreeViewItem;

            var dialog = new FileNameDialog((string)item.Header);
            if (dialog.ShowDialog() == true)
            {
                string fileName = dialog.FileName;
            }
        }

        private void AddModelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var item = e.Source as TreeViewItem;

            var dialog = new FileNameDialog((string)item.Header);
            if (dialog.ShowDialog() == true)
            {
                string fileName = dialog.FileName;
            }
        }

        private void AddBehaviorCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var item = e.Source as TreeViewItem;

            var dialog = new FileNameDialog((string)item.Header);
            if (dialog.ShowDialog() == true)
            {
                string fileName = dialog.FileName;
            }
        }

        private void AddTextureCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var item = e.Source as TreeViewItem;

            var dialog = new FileNameDialog((string)item.Header);
            if (dialog.ShowDialog() == true)
            {
                string fileName = dialog.FileName;
            }
        }

        private void AddAudioCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var item = e.Source as TreeViewItem;

            var dialog = new FileNameDialog((string)item.Header);
            if (dialog.ShowDialog() == true)
            {
                string fileName = dialog.FileName;
            }
        }

        private void OpenMapCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var fileName = GetFileNameFromTreeViewItem(e.Source as TreeViewItem);
            MapSelected?.Invoke(this, new ItemSelectedEventArgs(fileName));
        }

        private void OpenModelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var fileName = GetFileNameFromTreeViewItem(e.Source as TreeViewItem);
            ModelSelected?.Invoke(this, new ItemSelectedEventArgs(fileName));
        }

        private void OpenBehaviorCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var fileName = GetFileNameFromTreeViewItem(e.Source as TreeViewItem);
            BehaviorSelected?.Invoke(this, new ItemSelectedEventArgs(fileName));
        }

        private void OpenTextureCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var fileName = GetFileNameFromTreeViewItem(e.Source as TreeViewItem);
            TextureSelected?.Invoke(this, new ItemSelectedEventArgs(fileName));
        }

        private void OpenAudioCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var fileName = GetFileNameFromTreeViewItem(e.Source as TreeViewItem);
            AudioSelected?.Invoke(this, new ItemSelectedEventArgs(fileName));
        }

        private void ExcludeCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var item = e.Source as TreeViewItem;
            var parent = item.Parent as TreeViewItem;
            parent.Items.Remove(item);
        }

        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var item = e.Source as TreeViewItem;

            var fileName = GetFileNameFromTreeViewItem(item);
            File.Delete(fileName);

            var parent = item.Parent as TreeViewItem;
            parent.Items.Remove(item);
        }

        private void RenameCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //var fileName = GetFileNameFromTreeViewItem(e.Source as TreeViewItem);

            var item = e.Source as TreeViewItem;
            var fileName = (string)item.Header;

            var dialog = new FileNameDialog(fileName);
            if (dialog.ShowDialog() == true)
            {
                File.Move(fileName, dialog.FileName);
                item.Header = dialog.FileName;
            }
        }

        private string GetFileNameFromTreeViewItem(TreeViewItem item) => Path.Combine(Path.GetDirectoryName(_project.Path), (string)item.Header);*/
    }
}
