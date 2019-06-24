using SpiceEngine.Entities;
using SpiceEngine.Maps;
using System;
using System.Collections.Generic;
using System.Text;

namespace SauceEditorCore.Models.Entities
{
    public class EditorEntity
    {
        public IEntity Entity { get; private set; }
        public IMapEntity3D MapEntity { get; private set; }

        public EditorEntity(IEntity entity, IMapEntity3D mapEntity)
        {
            Entity = entity;
            MapEntity = mapEntity;
        }
    }
}
