using SpiceEngine.Entities;
using SpiceEngine.Entities.Brushes;
using SpiceEngine.Entities.Volumes;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Vertices;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Rendering.Batches
{
    public class MeshBatch : Batch
    {
        public IMesh Mesh { get; }

        public MeshBatch(IMesh mesh) => Mesh = mesh;

        public override IBatch Duplicate() => new MeshBatch(Mesh.Duplicate());

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

        public override void Load() => Mesh.Load();

        public override void Draw(IEntityProvider entityProvider, ShaderProgram shaderProgram, ITextureProvider textureProvider = null)
        {
            var entity = entityProvider.GetEntity(EntityIDs.First());
            
            /*if (textureProvider != null && entity is Brush brush)
            {
                brush.SetUniforms(shaderProgram, textureProvider);
            }
            else
            {*/
                entity.SetUniforms(shaderProgram);
            //}
            
            Mesh.Draw();
        }
    }
}
