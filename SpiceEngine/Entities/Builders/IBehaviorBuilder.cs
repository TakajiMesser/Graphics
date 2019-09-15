using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Physics.Shapes;
using SpiceEngine.Rendering;
using SpiceEngine.Scripting;
using SpiceEngine.Scripting.Properties;
using SpiceEngine.Scripting.Scripts;
using SpiceEngine.Scripting.StimResponse;
using System;
using System.Collections.Generic;

namespace SpiceEngine.Entities.Builders
{
    public interface IBehaviorBuilder
    {
        IEnumerable<Script> Scripts { get; }
        List<Stimulus> Stimuli { get; }
        List<Property> Properties { get; }

        Behavior ToBehavior();
        Type GetEntityType();
    }
}
