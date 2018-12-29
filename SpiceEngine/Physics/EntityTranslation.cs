using OpenTK;
using SpiceEngine.Physics.Collision;
using SpiceEngine.Physics.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
