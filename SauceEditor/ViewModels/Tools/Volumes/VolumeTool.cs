using SpiceEngine.Maps;
using SweetGraphicsCore.Rendering.Models;
using System.ComponentModel;

namespace SauceEditor.ViewModels.Tools.Volumes
{
    public abstract class VolumeTool : Tool<MapVolume>
    {
        private RelayCommand _openCommand;

        public VolumeTool(string name) : base(name) { }

        public MapVolume.VolumeTypes VolumeType { get; set; }

        [Browsable(false)]
        public abstract ModelMesh MeshShape { get; }

        // TODO - Do we need to mark this inherited property as not browsable as well?
        public override MapVolume MapEntity
        {
            get
            {
                var builder = new ModelBuilder(MeshShape);
                return new MapVolume(builder)
                {
                    VolumeType = VolumeType
                };
            }
        }

        [Browsable(false)]
        public override RelayCommand OpenCommand => (_openCommand = _openCommand ?? new RelayCommand(
            p => { },
            p => true
        ));
    }
}
