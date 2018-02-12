using Graphics.Inputs;
using Graphics.Meshes;
using Graphics.Physics.Collision;
using Graphics.Rendering.Matrices;
using Graphics.Rendering.Shaders;
using Graphics.Scripting.BehaviorTrees;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Graphics.Lighting;
using Graphics.Rendering.Textures;
using OpenTK.Graphics.OpenGL;
using Graphics.Rendering.Vertices;
using OpenTK.Graphics;
using System.IO;
using Graphics.Materials;

namespace Graphics.GameObjects
{
    public class SimpleModel : Model
    {
        public List<Mesh<Vertex>> Meshes { get; private set; } = new List<Mesh<Vertex>>();
        public override List<Vector3> Vertices => Meshes.SelectMany(m => m.Vertices.Select(v => v.Position)).Distinct().ToList();

        public override void Load(ShaderProgram program) => Meshes.ForEach(m => m.Load(program));
        public override void ClearLights() => Meshes.ForEach(m => m.ClearLights());
        public override void AddPointLights(IEnumerable<PointLight> lights) => Meshes.ForEach(m => m.AddPointLights(lights));

        public override void AddTestColors()
        {
            foreach (var mesh in Meshes)
            {
                var vertices = new List<Vertex>();

                for (var i = 0; i < mesh.Vertices.Count; i++)
                {
                    if (i % 3 == 0)
                    {
                        vertices.Add(mesh.Vertices[i].Colored(Color4.Lime));
                    }
                    else if (i % 3 == 1)
                    {
                        vertices.Add(mesh.Vertices[i].Colored(Color4.Red));
                    }
                    else if (i % 3 == 2)
                    {
                        vertices.Add(mesh.Vertices[i].Colored(Color4.Blue));
                    }
                }

                mesh.ClearVertices();
                mesh.AddVertices(vertices);
                mesh.RefreshVertices();
            }
        }

        public override void Draw(ShaderProgram program)
        {
            _modelMatrix.Set(program);
            Meshes.ForEach(m => m.Draw(program));
        }

        public new static SimpleModel LoadFromFile(string filePath)
        {
            var model = new SimpleModel();

            using (var importer = new Assimp.AssimpContext())
            {
                var scene = importer.ImportFile(filePath, Assimp.PostProcessSteps.JoinIdenticalVertices | Assimp.PostProcessSteps.CalculateTangentSpace);

                foreach (var mesh in scene.Meshes)
                {
                    var material = new Material(scene.Materials[mesh.MaterialIndex]);
                    var vertices = new List<Vertex>();

                    for (var i = 0; i < mesh.VertexCount; i++)
                    {
                        var vertex = new Vertex()
                        {
                            Position = new Vector3(mesh.Vertices[i].X, mesh.Vertices[i].Y, mesh.Vertices[i].Z),
                            Color = new Color4(),
                            MaterialIndex = 0
                        };

                        if (mesh.HasNormals)
                        {
                            vertex.Normal = new Vector3(mesh.Normals[i].X, mesh.Normals[i].Y, mesh.Normals[i].Z);
                        }

                        if (mesh.HasTextureCoords(0))
                        {
                            vertex.TextureCoords = new Vector2(mesh.TextureCoordinateChannels[0][i].X, mesh.TextureCoordinateChannels[0][i].Y);
                        }

                        if (mesh.HasTangentBasis)
                        {
                            vertex.Tangent = new Vector3(mesh.Tangents[i].X, mesh.Tangents[i].Y, mesh.Tangents[i].Z);
                        }

                        vertices.Add(vertex);
                    }

                    model.Meshes.Add(new Mesh<Vertex>(vertices, material, mesh.GetIndices().ToList()));
                }

                return model;
            }
        }

        // Define how this object's state will be saved, if desired
        //public override void OnSaveState() { }
    }
}
