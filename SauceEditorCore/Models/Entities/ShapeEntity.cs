using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Maps;
using SpiceEngine.Rendering.Meshes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SauceEditorCore.Models.Entities
{
    public class ShapeEntity : IEntity
    {
        public int ID { get; set; }
        public Vector3 Position { get; set; }
        public MeshBuild MeshBuild { get; set; }

        public ShapeEntity(MeshShape meshShape)
        {
            MeshBuild = new MeshBuild(meshShape);
        }
    }
}
