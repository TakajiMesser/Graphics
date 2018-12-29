using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Physics.Collision;

namespace SpiceEngine.Maps
{
    public class EntityMapping
    {
        public List<int> ActorIDs { get; } = new List<int>();
        public List<int> BrushIDs { get; } = new List<int>();
        public List<int> VolumeIDs { get; } = new List<int>();
        public List<int> LightIDs { get; } = new List<int>();

        public EntityMapping(IEnumerable<int> actorIDs, IEnumerable<int> brushIDs, IEnumerable<int> volumeIDs, IEnumerable<int> lightIDs)
        {
            ActorIDs.AddRange(actorIDs);
            BrushIDs.AddRange(brushIDs);
            VolumeIDs.AddRange(volumeIDs);
            LightIDs.AddRange(lightIDs);
        }
    }
}
