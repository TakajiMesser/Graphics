using OpenTK;
using SauceEditor.Models.Entities;
using SpiceEngine.Maps;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Vertices;
using SpiceEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SauceEditor.Models.Components
{
    public class ModelComponent : Component//, IGameViewable
    {
        public Model Model { get; set; }

        public IEnumerable<ShapeEntity> GetShapeEntities()
        {
            foreach (var mesh in Model.Meshes)
            {
                //mesh.
            }

            yield return null;
        }

        public IEnumerable<FaceEntity> GetFaceEntities() => throw new NotImplementedException();

        public IEnumerable<TriangleEntity> GetTriangleEntities() => throw new NotImplementedException();

        public IEnumerable<VertexEntity> GetVertexEntities() => throw new NotImplementedException();

        public override void Save()
        {
            /*using (var reader = XmlReader.Create(path))
            {
                var serializer = new NetDataContractSerializer();

                var project = serializer.ReadObject(reader, true) as Project;
                project.Path = path;

                return project;
            }*/
        }

        public override void Load() => Model = new Model(Path);
    }
}
