using OpenTK;
using SauceEditorCore.Helpers;
using SpiceEngine.Entities;
using SpiceEngine.Maps;
using SpiceEngine.Rendering;
using SpiceEngine.Rendering.Matrices;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using System;
using System.Collections.Generic;
using System.Text;

namespace SauceEditorCore.Models.Entities
{
    public class VertexEntity : ModelEntity
    {
        public MeshVertex Vertex { get; }

        public VertexEntity(MeshVertex meshVertex) => Vertex = meshVertex;

        public override bool CompareUniforms(IEntity entity) => base.CompareUniforms(entity) && entity is VertexEntity;

        public override IRenderable ToRenderable()
        {
            return new TextureID(FilePathHelper.VERTEX_TEXTURE_PATH);
            /*var meshBuild = new MeshBuild(Triangle);
            var meshVertices = meshBuild.GetVertices();

            if (meshVertices.Any(v => v.IsAnimated))
            {
                var vertices = meshBuild.GetVertices().Select(v => v.ToVertex3D());
                return new Mesh<Vertex3D>(vertices, meshBuild.TriangleIndices);
            }
            else
            {
                var vertices = meshBuild.GetVertices().Select(v => v.ToJointVertex3D());
                return new Mesh<JointVertex3D>(vertices, meshBuild.TriangleIndices);
            }*/
        }
    }
}
