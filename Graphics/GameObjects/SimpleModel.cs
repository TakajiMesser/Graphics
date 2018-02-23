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
using Graphics.Utilities;

namespace Graphics.GameObjects
{
    public class SimpleModel : Model
    {
        public List<Mesh<Vertex>> Meshes { get; private set; } = new List<Mesh<Vertex>>();
        public override List<Vector3> Vertices => Meshes.SelectMany(m => m.Vertices.Select(v => v.Position)).Distinct().ToList();

        public SimpleModel() { }
        public SimpleModel(Assimp.Scene scene)
        {
            foreach (var mesh in scene.Meshes)
            {
                var material = new Material(scene.Materials[mesh.MaterialIndex]);
                var vertices = new List<Vertex>();

                for (var i = 0; i < mesh.VertexCount; i++)
                {
                    var vertex = new Vertex()
                    {
                        Position = mesh.Vertices[i].ToVector3(),
                        Color = new Color4()
                    };

                    if (mesh.HasNormals)
                    {
                        vertex.Normal = mesh.Normals[i].ToVector3();
                    }

                    if (mesh.HasTextureCoords(0))
                    {
                        vertex.TextureCoords = mesh.TextureCoordinateChannels[0][i].ToVector2();
                    }

                    if (mesh.HasTangentBasis)
                    {
                        vertex.Tangent = mesh.Tangents[i].ToVector3();
                    }

                    vertices.Add(vertex);
                }

                Meshes.Add(new Mesh<Vertex>(vertices, material, mesh.GetIndices().ToList()));
            }
        }

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

        public override void Draw(ShaderProgram program, TextureManager textureManager)
        {
            _modelMatrix.Set(program);

            foreach (var mesh in Meshes)
            {
                mesh.Draw(program, textureManager);
            }
        }

        // Define how this object's state will be saved, if desired
        //public override void OnSaveState() { }
    }
}
