using SpiceEngine.Entities;
using SpiceEngine.Maps.Builders;

namespace SauceEditorCore.Models.Entities
{
    public class EditorEntity
    {
        public IEntity Entity { get; private set; }
        public IEntityBuilder MapEntity { get; private set; }

        public EditorEntity(IEntity entity, IEntityBuilder mapEntity)
        {
            Entity = entity;
            MapEntity = mapEntity;
        }
    }
}
