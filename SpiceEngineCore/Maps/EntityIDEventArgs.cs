using System;
using System.Collections.Generic;

namespace SpiceEngineCore.Maps
{
    public class EntityIDEventArgs : EventArgs
    {
        public List<int> ActorIDs { get; } = new List<int>();
        public List<int> BrushIDs { get; } = new List<int>();
        public List<int> VolumeIDs { get; } = new List<int>();
        public List<int> LightIDs { get; } = new List<int>();

        public EntityIDEventArgs(IEnumerable<int> actorIDs, IEnumerable<int> brushIDs, IEnumerable<int> volumeIDs, IEnumerable<int> lightIDs)
        {
            ActorIDs.AddRange(actorIDs);
            BrushIDs.AddRange(brushIDs);
            VolumeIDs.AddRange(volumeIDs);
            LightIDs.AddRange(lightIDs);
        }
    }
}
