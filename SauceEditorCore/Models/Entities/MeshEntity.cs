﻿using OpenTK;
using SpiceEngine.Entities.Selection;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Meshes;
using SpiceEngineCore.Rendering.Models;
using SpiceEngineCore.Rendering.Textures;
using SpiceEngineCore.Rendering.Vertices;
using System.Linq;

namespace SauceEditorCore.Models.Entities
{
    public class MeshEntity : TexturedModelEntity<ModelMesh>, /*ITextureBinder, ITexturePath, */IDirectional
    {
        public override Vector3 XDirection => Vector3.UnitX;
        public override Vector3 YDirection => Vector3.UnitY;
        public override Vector3 ZDirection => Vector3.UnitZ;

        public MeshEntity(ModelMesh modelMesh, TexturePaths texturePaths) : base(modelMesh, texturePaths) { }

        /*public override bool CompareUniforms(IEntity entity) => entity is MeshEntity
            && base.CompareUniforms(entity);*/

        public override IRenderable ToComponent()
        {
            var meshBuild = new ModelBuilder(ModelShape);
            var meshVertices = meshBuild.GetVertices();

            var mesh = meshVertices.Any(v => v.IsAnimated)
                ? (ITexturedMesh)new TexturedMesh<AnimatedVertex3D>(meshBuild.GetVertices().Select(v => v.ToJointVertex3D()).ToList(), meshBuild.TriangleIndices.AsEnumerable().Reverse().ToList())
                : new TexturedMesh<Vertex3D>(meshBuild.GetVertices().Select(v => v.ToVertex3D()).ToList(), meshBuild.TriangleIndices.AsEnumerable().Reverse().ToList());

            mesh.Material = _material;
            mesh.TextureMapping = _textureMapping;

            mesh.Transform(_modelMatrix.WorldTransform);
            return mesh;
        }
    }
}
