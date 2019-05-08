namespace SauceEditor.Models.Components
{
    public abstract class Component
    {
        private string _path;

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
