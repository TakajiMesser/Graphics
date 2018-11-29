using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiceEngine.Rendering.Buffers;
using SpiceEngine.Rendering.Vertices;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Entities;
using OpenTK.Graphics;

namespace SpiceEngine.Rendering.Batches
{
    public class MeshBatch : IBatch
    {
        public int EntityID { get; private set; }
        public IMesh3D Mesh { get; }
        public IEnumerable<IVertex3D> Vertices => Mesh.Vertices;

        public MeshBatch(int entityID, IMesh3D mesh)
        {
            EntityID = entityID;
            Mesh = mesh;
        }

        public void AddTestColors()
        {
            /*var vertices = new List<Vertex3D>();

            for (var i = 0; i < Mesh.Vertices.Count; i++)
            {
                if (i % 3 == 0)
                {
                    vertices.Add(Mesh.Vertices[i].Colored(Color4.Lime));
                }
                else if (i % 3 == 1)
                {
                    vertices.Add(Mesh.Vertices[i].Colored(Color4.Red));
                }
                else if (i % 3 == 2)
                {
                    vertices.Add(Mesh.Vertices[i].Colored(Color4.Blue));
                }
            }

            Mesh.ClearVertices();
            Mesh.AddVertices(vertices);*/
        }

        public void Load() => Mesh.Load();

        public void Draw(IEntityProvider entityProvider, ShaderProgram shaderProgram, TextureManager textureManager = null)
        {
            var brush = (Brush)entityProvider.GetEntity(EntityID);
            brush.SetUniforms(shaderProgram, textureManager);

            Mesh.Draw();
        }
    }
}
