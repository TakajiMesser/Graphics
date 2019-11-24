using SpiceEngineCore.Maps;
using SpiceEngineCore.Rendering.Models;
using System.ComponentModel;

namespace SauceEditor.ViewModels.Tools.Brushes
{
    public abstract class BrushTool : Tool<MapBrush>
    {
        private RelayCommand _openCommand;

        public BrushTool(string name) : base(name) { }

        [Browsable(false)]
        public abstract ModelMesh MeshShape { get; }

        // TODO - Do we need to mark this inherited property as not browsable as well?
        public override MapBrush MapEntity
        {
            get
            {
                var builder = new ModelBuilder(MeshShape);
                return new MapBrush(builder);
            }
        }

        [Browsable(false)]
        public override RelayCommand OpenCommand => (_openCommand = _openCommand ?? new RelayCommand(
            p => { },
            p => true
        ));
    }
}
