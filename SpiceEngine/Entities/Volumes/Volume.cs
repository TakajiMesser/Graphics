using OpenTK;
using OpenTK.Graphics;
using System.Collections.Generic;
using System.Linq;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Game;
using SpiceEngine.Helpers;
using SpiceEngine.Physics.Collision;
using SpiceEngine.Rendering.Materials;
using SpiceEngine.Rendering.Matrices;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Vertices;
using SpiceEngine.Scripting.StimResponse;

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
        public Volume(List<Vector3> vertices, List<int> triangleIndices, Vector4 color)
        {
            var simpleVertices = vertices.Select(v => new ColorVertex3D(v, color)).ToList();
            //Mesh = new Mesh3D<ColorVertex3D>(simpleVertices, triangleIndices);
        }

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

        public static Volume RectangularPrism(Vector3 center, float xLength, float yLength, float zLength, Vector4 color)
        {
            var vertices = new List<Vector3>
            {
                new Vector3(center.X - xLength / 2.0f, center.Y - yLength / 2.0f, center.Z - zLength / 2.0f),
                new Vector3(center.X - xLength / 2.0f, center.Y + yLength / 2.0f, center.Z - zLength / 2.0f),
                new Vector3(center.X - xLength / 2.0f, center.Y - yLength / 2.0f, center.Z + zLength / 2.0f),
                new Vector3(center.X - xLength / 2.0f, center.Y + yLength / 2.0f, center.Z + zLength / 2.0f),
                new Vector3(center.X + xLength / 2.0f, center.Y - yLength / 2.0f, center.Z - zLength / 2.0f),
                new Vector3(center.X + xLength / 2.0f, center.Y + yLength / 2.0f, center.Z - zLength / 2.0f),
                new Vector3(center.X + xLength / 2.0f, center.Y - yLength / 2.0f, center.Z + zLength / 2.0f),
                new Vector3(center.X + xLength / 2.0f, center.Y + yLength / 2.0f, center.Z + zLength / 2.0f)
            };

            var triangleIndices = new List<int>()
            {
                7, 6, 4,
                7, 4, 5,
                1, 3, 7,
                1, 7, 5,
                3, 2, 6,
                3, 6, 7,
                1, 0, 2,
                1, 2, 3,
                2, 0, 4,
                2, 4, 6,
                5, 4, 0,
                5, 0, 1
            };

            return new Volume(vertices, triangleIndices, color);
        }
    }
}
