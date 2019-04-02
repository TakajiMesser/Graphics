using SpiceEngine.Entities;
using SpiceEngine.Maps;

namespace SauceEditor.Models
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
