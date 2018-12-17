using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Entities.Actors;
using SpiceEngine.Entities.Cameras;
using SpiceEngine.Inputs;
using SpiceEngine.Physics;
using SpiceEngine.Physics.Collision;
using SpiceEngine.Physics.Shapes;
using SpiceEngine.Scripting.Behaviors;
using SpiceEngine.Scripting.Properties;
using SpiceEngine.Scripting.StimResponse;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Scripting
{
    public interface IStimulusProvider
    {
        IEnumerable<Stimulus> GetStimuli(int entityID);
    }
}
