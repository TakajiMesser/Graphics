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

        public ProjectTreeView()
        {
            InitializeComponent();
        }

        public void OpenProject(string filePath)
        {
            _project = GameProject.Load(filePath);

            Tree.Items.Clear();

            var root = new TreeViewItem() { Header = Path.GetFileNameWithoutExtension(filePath), IsExpanded = true };
            Tree.Items.Add(root);

            var maps = new TreeViewItem() { Header = "Maps", IsExpanded = true };
            foreach (var mapPath in _project.MapPaths)
            {
                var map = new TreeViewItem() { Header = Path.GetFileName(mapPath) };
                map.MouseDoubleClick += (s, args) => MapSelected?.Invoke(this, new ItemSelectedEventArgs(mapPath));
                maps.Items.Add(map);
            }
            root.Items.Add(maps);

            var models = new TreeViewItem() { Header = "Models", IsExpanded = true };
            foreach (var modelPath in _project.ModelPaths)
            {
                var model = new TreeViewItem() { Header = Path.GetFileName(modelPath) };
                model.MouseDoubleClick += (s, args) => ModelSelected?.Invoke(this, new ItemSelectedEventArgs(modelPath));
                models.Items.Add(model);
            }
            root.Items.Add(models);

            var behaviors = new TreeViewItem() { Header = "Behaviors", IsExpanded = true };
            foreach (var behaviorPath in _project.BehaviorPaths)
            {
                var behavior = new TreeViewItem() { Header = Path.GetFileName(behaviorPath) };
                behavior.MouseDoubleClick += (s, args) => BehaviorSelected?.Invoke(this, new ItemSelectedEventArgs(behaviorPath));
                behaviors.Items.Add(behavior);
            }
            root.Items.Add(behaviors);

            var textures = new TreeViewItem() { Header = "Textures", IsExpanded = true };
            foreach (var texturePath in _project.TexturePaths)
            {
                var texture = new TreeViewItem() { Header = Path.GetFileName(texturePath) };
                texture.MouseDoubleClick += (s, args) => TextureSelected?.Invoke(this, new ItemSelectedEventArgs(texturePath));
                textures.Items.Add(texture);
            }
            root.Items.Add(textures);

            var audios = new TreeViewItem() { Header = "Audio", IsExpanded = true };
            foreach (var audioPath in _project.AudioPaths)
            {
                var audio = new TreeViewItem() { Header = Path.GetFileName(audioPath) };
                audio.MouseDoubleClick += (s, args) => AudioSelected?.Invoke(this, new ItemSelectedEventArgs(audioPath));
                audios.Items.Add(audio);
            }
            root.Items.Add(audios);
        }

        public void SaveProject()
        {

        }

        public void Add(TreeViewItem item)
        {
            //Tree.Items.Add(item);
        }
    }
}
