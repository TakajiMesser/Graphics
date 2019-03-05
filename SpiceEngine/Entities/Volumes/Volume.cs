using OpenTK;
using SpiceEngine.Rendering.Matrices;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Vertices;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Entities.Volumes
{
    /// <summary>
    /// Brushes are static geometric shapes that are baked into a scene.
    /// Unlike meshes, brushes cannot be deformed.
    /// </summary>
    public class Volume : IEntity, IRotate, IScale
    {
        public int ID { get; set; }

        public Vector3 Position
        {
            get => _modelMatrix.Translation;
            set => _modelMatrix.Translation = value;
        }

        public Vector3 OriginalRotation { get; set; }

        public Quaternion Rotation
        {
            get => _modelMatrix.Rotation;
            set => _modelMatrix.Rotation = value;
        }

        public Vector3 Scale
        {
            get => _modelMatrix.Scale;
            set => _modelMatrix.Scale = value;
        }

        //public Mesh3D<ColorVertex3D> Mesh { get; private set; }

        //public List<Vector3> Vertices => Mesh.Vertices.Select(v => v.Position).Distinct().ToList();

        private ModelMatrix _modelMatrix = new ModelMatrix();

        public Volume() { }
        /*public Volume(List<Vector3> vertices, List<int> triangleIndices, Vector4 color)
        {
            var simpleVertices = vertices.Select(v => new ColorVertex3D(v, color)).ToList();
            //Mesh = new Mesh3D<ColorVertex3D>(simpleVertices, triangleIndices);
        }*/

        public Volume Duplicate() => new Volume()
        {
            Position = Position,
            Rotation = Rotation,
            Scale = Scale,
        };

        //public void Load() => Mesh.Load();
        //public void Draw() => Mesh.Draw();

        public void SetUniforms(ShaderProgram program)
        {
            _modelMatrix.Set(program);
            //Mesh.SetUniforms(program);
        }
    }
}
