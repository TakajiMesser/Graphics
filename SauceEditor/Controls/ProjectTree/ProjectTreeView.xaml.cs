using SauceEditor.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace SauceEditor.Controls.ProjectTree
{
    /// <summary>
    /// Interaction logic for ProjectTreeView.xaml
    /// </summary>
    public partial class ProjectTreeView : DockingLibrary.DockableContent
    {
        public event EventHandler<ItemSelectedEventArgs> MapSelected;
        public event EventHandler<ItemSelectedEventArgs> ModelSelected;
        public event EventHandler<ItemSelectedEventArgs> BehaviorSelected;
        public event EventHandler<ItemSelectedEventArgs> TextureSelected;
        public event EventHandler<ItemSelectedEventArgs> AudioSelected;

        private GameProject _project;
        private string _projectPath;

        public ProjectTreeView()
        {
            InitializeComponent();
        }

        public void Add(TreeViewItem item)
        {
            //Tree.Items.Add(item);
        }

        public void SaveProject()
        {

        }

        public void OpenProject(string filePath)
        {
            _projectPath = filePath;
            _project = GameProject.Load(filePath);

            Tree.Items.Clear();

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

            foreach (var mapPath in _project.MapPaths)
            {
                var map = new TreeViewItem()
                {
                    Header = Path.GetFileName(mapPath)
                };

                map.MouseDoubleClick += (s, args) => MapSelected?.Invoke(this, new ItemSelectedEventArgs(mapPath));
                map.MouseRightButtonDown += (s, args) =>
                {
                    var item = s as TreeViewItem;
                    item.IsSelected = true;
                    item.ContextMenu = Tree.FindResource("MapMenu") as ContextMenu;
                };
                maps.Items.Add(map);
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

            foreach (var modelPath in _project.ModelPaths)
            {
                var model = new TreeViewItem()
                {
                    Header = Path.GetFileName(modelPath)
                };

                model.MouseDoubleClick += (s, args) => ModelSelected?.Invoke(this, new ItemSelectedEventArgs(modelPath));
                model.MouseRightButtonDown += (s, args) =>
                {
                    var item = s as TreeViewItem;
                    item.IsSelected = true;
                    item.ContextMenu = FindResource("ModelMenu") as ContextMenu;
                };
                models.Items.Add(model);
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

            foreach (var behaviorPath in _project.BehaviorPaths)
            {
                var behavior = new TreeViewItem()
                {
                    Header = Path.GetFileName(behaviorPath)
                };
                behavior.MouseDoubleClick += (s, args) => BehaviorSelected?.Invoke(this, new ItemSelectedEventArgs(behaviorPath));
                behavior.MouseRightButtonDown += (s, args) =>
                {
                    var item = s as TreeViewItem;
                    item.IsSelected = true;
                    item.ContextMenu = FindResource("BehaviorMenu") as ContextMenu;
                };
                behaviors.Items.Add(behavior);
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

            foreach (var texturePath in _project.TexturePaths)
            {
                var texture = new TreeViewItem()
                {
                    Header = Path.GetFileName(texturePath)
                };
                texture.MouseDoubleClick += (s, args) => TextureSelected?.Invoke(this, new ItemSelectedEventArgs(texturePath));
                texture.MouseRightButtonDown += (s, args) =>
                {
                    var item = s as TreeViewItem;
                    item.IsSelected = true;
                    item.ContextMenu = FindResource("TextureMenu") as ContextMenu;
                };
                textures.Items.Add(texture);
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

            foreach (var audioPath in _project.AudioPaths)
            {
                var audio = new TreeViewItem()
                {
                    Header = Path.GetFileName(audioPath)
                };
                audio.MouseDoubleClick += (s, args) => AudioSelected?.Invoke(this, new ItemSelectedEventArgs(audioPath));
                audio.MouseRightButtonDown += (s, args) =>
                {
                    var item = s as TreeViewItem;
                    item.IsSelected = true;
                    item.ContextMenu = FindResource("AudioMenu") as ContextMenu;
                };
                audios.Items.Add(audio);
            }

            return audios;
        }

        private void OpenMapCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void OpenMapCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var item = e.Source as TreeViewItem;
            var fileName = Path.Combine(Path.GetDirectoryName(_projectPath), (string)item.Header);
            MapSelected?.Invoke(this, new ItemSelectedEventArgs(fileName));
        }
    }
}
