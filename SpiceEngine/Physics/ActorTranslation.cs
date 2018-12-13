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
    public class ActorTranslation
    {
        public int ActorID { get; private set; }
        public Vector3 Translation { get; private set; }

        public ActorTranslation(int actorID, Vector3 translation)
        {
            ActorID = actorID;
            Translation = translation;
        }
    }
}
