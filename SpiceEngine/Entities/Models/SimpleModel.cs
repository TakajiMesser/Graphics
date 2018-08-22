using OpenTK;
using OpenTK.Graphics;
using SpiceEngine.Rendering.Materials;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Vertices;
using SpiceEngine.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Entities.Models
{
    public class SimpleModel : Model3D
    {
        public List<Mesh3D<Vertex3D>> Meshes { get; private set; } = new List<Mesh3D<Vertex3D>>();
        public override List<Vector3> Vertices => Meshes.SelectMany(m => m.Vertices.Select(v => v.Position)).Distinct().ToList();

        public SimpleModel() { }
        public SimpleModel(Assimp.Scene scene)
        {
            foreach (var mesh in scene.Meshes)
            {
                var material = new Material(scene.Materials[mesh.MaterialIndex]);
                var vertices = new List<Vertex3D>();

                for (var i = 0; i < mesh.VertexCount; i++)
                {
                    var position = mesh.Vertices[i].ToVector3();
                    var normals = mesh.HasNormals ? mesh.Normals[i].ToVector3() : new Vector3();
                    var tangents = mesh.HasTangentBasis ? mesh.Tangents[i].ToVector3() : new Vector3();
                    var textureCoords = mesh.HasTextureCoords(0) ? mesh.TextureCoordinateChannels[0][i].ToVector2() : new Vector2();

                    vertices.Add(new Vertex3D(position, normals, tangents, textureCoords));
                }

                Meshes.Add(new Mesh3D<Vertex3D>(vertices, material, mesh.GetIndices().ToList()));
            }
        }

        public override void Load()
        {
            foreach (var mesh in Meshes)
            {
                mesh.Load();
            }
        }

        public override void Draw()
        {
            foreach (var mesh in Meshes)
            {
                mesh.Draw();
            }
        }

        public override void SetUniforms(ShaderProgram program, TextureManager textureManager)
        {
            _modelMatrix.Set(program);

            foreach (var mesh in Meshes)
            {
                mesh.SetUniforms(program, textureManager);
            }
        }

        public override void SetUniformsAndDraw(ShaderProgram program, TextureManager textureManager)
        {
            _modelMatrix.Set(program);

            foreach (var mesh in Meshes)
            {
                mesh.SetUniforms(program, textureManager);
                mesh.Draw();
            }
        }

        // Define how this object's state will be saved, if desired
        //public override void OnSaveState() { }
    }
}
