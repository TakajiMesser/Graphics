using OpenTK;

namespace SpiceEngine.Physics
{
    public class EntityTranslation
    {
        public int EntityID { get; private set; }
        public Vector3 Translation { get; private set; }

        public EntityTranslation(int entityID, Vector3 translation)
        {
            EntityID = entityID;
            Translation = translation;
        }
    }
}
