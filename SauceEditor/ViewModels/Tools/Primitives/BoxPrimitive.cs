using SauceEditor.Views;
using SpiceEngine.Rendering.Meshes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SauceEditor.ViewModels.Tools.Primitives
{
    public class BoxPrimitive : Primitive
    {
        public BoxPrimitive() : base("Box") { }

        public float Width { get; set; } = 10.0f;
        public float Height { get; set; } = 10.0f;
        public float Depth { get; set; } = 10.0f;

        public override ModelMesh MeshShape => ModelMesh.Box(Width, Height, Depth);

        private RelayCommand _openCommand;
        public RelayCommand OpenCommand => (_openCommand = _openCommand ?? new RelayCommand(
            p => { },
            p => true
        ));

        /*var meshShape = ModelMesh.Box(width, height, depth);
        var meshBuild = new ModelBuilder(meshShape);

        return new MapBrush()
        {
            Position = center,
            Vertices = meshBuild.GetVertices().Select(v => new Vertex3D(v.Position, v.Normal, v.Tangent, v.UV)).ToList(),
            TriangleIndices = meshBuild.TriangleIndices,
            Material = Material.LoadFromFile(FilePathHelper.GENERIC_MATERIAL_PATH).First().Item2
        };*/
    }
}
