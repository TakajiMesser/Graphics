namespace SauceEditorCore.Models.Components
{
    public abstract class Component : IComponent
    {
        private string _path;

        public Component() { }
        public Component(string filePath) => Path = filePath;

        public string Path
        {
            get => _path;
            set
            {
                _path = value;
                Name = System.IO.Path.GetFileNameWithoutExtension(value);
            }
        }

        public string Name { get; set; }

        public abstract void Load();
        public abstract void Save();
    }
}
