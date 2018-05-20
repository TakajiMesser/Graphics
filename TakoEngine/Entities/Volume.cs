using OpenTK;
using OpenTK.Graphics;
using System.Collections.Generic;
using System.Linq;
using TakoEngine.Entities.Lights;
using TakoEngine.Game;
using TakoEngine.Helpers;
using TakoEngine.Physics.Collision;
using TakoEngine.Rendering.Materials;
using TakoEngine.Rendering.Matrices;
using TakoEngine.Rendering.Meshes;
using TakoEngine.Rendering.Shaders;
using TakoEngine.Rendering.Textures;
using TakoEngine.Rendering.Vertices;
using TakoEngine.Scripting.StimResponse;

namespace TakoEngine.Entities
{
    /// <summary>
    /// Brushes are static geometric shapes that are baked into a scene.
    /// Unlike meshes, brushes cannot be deformed.
    /// </summary>
    public class Volume : IEntity, IStimulate, ICollidable, IRotate, IScale
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

        public Mesh<Simple3DVertex> Mesh { get; private set; }
        public Dictionary<string, GameProperty> Properties { get; private set; } = new Dictionary<string, GameProperty>();
        public List<Stimulus> Stimuli { get; private set; } = new List<Stimulus>();

        public Bounds Bounds { get; set; }
        public bool HasCollision { get; set; } = false;
        public List<Vector3> Vertices => Mesh.Vertices.Select(v => v.Position).Distinct().ToList();

        private ModelMatrix _modelMatrix = new ModelMatrix();

        public Volume(List<Vector3> vertices, List<int> triangleIndices)
        {
            var simpleVertices = vertices.Select(v => new Simple3DVertex(v)).ToList();
            Mesh = new Mesh<Simple3DVertex>(simpleVertices, triangleIndices);
        }

        public void Load() => Mesh.Load();

        public void Draw(ShaderProgram program)
        {
            _modelMatrix.Set(program);
            Mesh.Draw(program);
        }

        public static Volume RectangularPrism(Vector3 center, float xLength, float yLength, float zLength)
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

            return new Volume(vertices, triangleIndices);
        }
    }
}
