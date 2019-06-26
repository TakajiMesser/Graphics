using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Maps;
using SpiceEngine.Rendering.Meshes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SauceEditorCore.Models.Entities
{
    public class FaceEntity : IEntity
    {
        public int ID { get; set; }
        public Vector3 Position { get; set; }
        public MeshFace MeshFace { get; set; }

        public FaceEntity(MeshFace meshFace)
        {

        }

        public IMesh ToMesh()
        {
            throw new NotImplementedException();
        }
    }
}
